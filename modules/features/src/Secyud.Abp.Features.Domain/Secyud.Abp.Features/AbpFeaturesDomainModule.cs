using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Secyud.Abp.Features.Localization;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpFeaturesDomainSharedModule),
    typeof(AbpFeaturesModule),
    typeof(AbpCachingModule)
)]
public class AbpFeaturesDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FeaturesOptions>(options =>
        {
            options.Providers.Add<DefaultValueFeaturesProvider>();
            options.Providers.Add<EditionFeaturesProvider>();

            //TODO: Should be moved to the Tenants module
            options.Providers.Add<TenantFeaturesProvider>();
            options.ProviderPolicies[TenantFeatureValueProvider.ProviderName] = "AbpTenants.Tenants.ManageFeatures";
        });

        Configure<AbpExceptionLocalizationOptions>(options => { options.MapCodeNamespace("AbpFeatures", typeof(AbpFeaturesResource)); });

        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<FeaturesOptions>(options =>
            {
                options.SaveStaticFeaturesToDatabase = false;
                options.IsDynamicFeatureStoreEnabled = false;
            });
        }
    }

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Task? _initializeDynamicFeaturesTask;

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        InitializeDynamicFeatures(context);
        return Task.CompletedTask;
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    public Task GetInitializeDynamicFeaturesTask()
    {
        return _initializeDynamicFeaturesTask ?? Task.CompletedTask;
    }

    private void InitializeDynamicFeatures(ApplicationInitializationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<FeaturesOptions>>()
            .Value;

        if (options is { SaveStaticFeaturesToDatabase: false, IsDynamicFeatureStoreEnabled: false })
        {
            return;
        }

        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        _initializeDynamicFeaturesTask = Task.Run(async () =>
        {
            using var scope = rootServiceProvider.CreateScope();
            var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationTokenProvider = scope.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            var cancellationToken = applicationLifetime?.ApplicationStopping ?? _cancellationTokenSource.Token;

            try
            {
                using (cancellationTokenProvider.Use(cancellationToken))
                {
                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await SaveStaticFeaturesToDatabaseAsync(options, scope, cancellationTokenProvider);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch
            {
            }
        });
    }

    private static async Task SaveStaticFeaturesToDatabaseAsync(
        FeaturesOptions options,
        IServiceScope scope,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        if (!options.SaveStaticFeaturesToDatabase)
        {
            return;
        }

        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                8,
                retryAttempt => TimeSpan.FromSeconds(
                    RandomHelper.GetRandom(
                        (int)Math.Pow(2, retryAttempt) * 8,
                        (int)Math.Pow(2, retryAttempt) * 12)
                )
            )
            .ExecuteAsync(async _ =>
            {
                try
                {
                    // ReSharper disable once AccessToDisposedClosure
                    await scope
                        .ServiceProvider
                        .GetRequiredService<IStaticFeatureSaver>()
                        .SaveAsync();
                }
                catch (Exception ex)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    scope.ServiceProvider
                        .GetService<ILogger<AbpFeaturesDomainModule>>()?
                        .LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationTokenProvider.Token);
    }
}