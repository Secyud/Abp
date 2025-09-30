using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using SecitsDemoApp.EntityFrameworkCore;
using SecitsDemoApp.HealthChecks;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Studio;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace SecitsDemoApp;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SecitsDemoAppEntityFrameworkCoreModule),
    typeof(SecitsDemoAppHttpApiModule),
    typeof(SecitsDemoAppApplicationModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpDistributedLockingModule),
    typeof(AbpEventBusRabbitMqModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule)
)]
public class SecitsDemoAppHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (!configuration.GetValue<bool>("App:DisablePII"))
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        }

        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        // ConfigureAuthentication(context, configuration);
        // ConfigureSwagger(context, configuration);
        ConfigureCache(configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureDataProtection(context, configuration, hostingEnvironment);
        ConfigureDistributedLocking(context, configuration);
        ConfigureCors(context, configuration);
        ConfigureHealthChecks(context);

        // Configure<Permissionop>(options => { options.IsDynamicPermissionStoreEnabled = true; });
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddSecitsDemoAppHealthChecks();
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options => { options.Applications["MVCPublic"].RootUrl = configuration["App:MVCPublicUrl"]; });
    }

    private void ConfigureCache(IConfiguration configuration)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "SecitsDemoApp:"; });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<SecitsDemoAppDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath,
                    string.Format("..{0}..{0}demo{0}SecitsDemoApp.Domain.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<SecitsDemoAppDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath,
                    string.Format("..{0}..{0}demo{0}SecitsDemoApp.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<SecitsDemoAppApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath,
                    string.Format("..{0}..{0}demo{0}SecitsDemoApp.Application.Contracts", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<SecitsDemoAppApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath,
                    string.Format("..{0}..{0}demo{0}SecitsDemoApp.Application", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<SecitsDemoAppHttpApiModule>(Path.Combine(hostingEnvironment.ContentRootPath,
                    string.Format("..{0}..{0}demo{0}SecitsDemoApp.HttpApi", Path.DirectorySeparatorChar)));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options => { options.ConventionalControllers.Create(typeof(SecitsDemoAppApplicationModule).Assembly); });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddAbpJwtBearer(options =>
        //     {
        //         options.Authority = configuration["AuthServer:Authority"];
        //         options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
        //         options.Audience = "SecitsDemoApp";
        //     });

        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options => { options.IsDynamicClaimsEnabled = true; });
    }

    private static void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOidc(
            configuration["AuthServer:Authority"]!,
            ["SecitsDemoApp"],
            [AbpSwaggerOidcFlows.AuthorizationCode],
            configuration["AuthServer:MetaAddress"],
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SecitsDemoApp API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureDataProtection(
        ServiceConfigurationContext context,
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment)
    {
        if (AbpStudioAnalyzeHelper.IsInAnalyzeMode)
        {
            return;
        }

        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("SecitsDemoApp");
        if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "SecitsDemoApp-Protection-Keys");
        }
    }

    private void ConfigureDistributedLocking(
        ServiceConfigurationContext context,
        IConfiguration configuration)
    {
        if (AbpStudioAnalyzeHelper.IsInAnalyzeMode)
        {
            return;
        }

        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
                            .ToArray() ?? []
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseAbpSecurityHeaders();
        app.UseCors();
        app.UseAuthentication();

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "SecitsDemoApp API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}