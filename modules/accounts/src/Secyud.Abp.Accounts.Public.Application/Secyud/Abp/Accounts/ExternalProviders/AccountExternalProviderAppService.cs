using Secyud.Abp.Account.ExternalProviders;
using Volo.Abp.Application.Services;
using Volo.Abp.Security.Encryption;

namespace Secyud.Abp.Accounts.ExternalProviders;

public class AccountExternalProviderAppService(
    ExternalProviderSettingsHelper externalProviderSettingsHelper,
    IStringEncryptionService stringEncryptionService)
    : ApplicationService, IAccountExternalProviderAppService
{
    protected ExternalProviderSettingsHelper ExternalProviderSettingsHelper { get; } = externalProviderSettingsHelper;

    protected IStringEncryptionService StringEncryptionService { get; } = stringEncryptionService;

    public virtual async Task<ExternalProviderDto> GetAllAsync()
    {
        return new ExternalProviderDto
        {
            Providers = ObjectMapper.Map<List<ExternalProviderSettings>, List<ExternalProviderItemDto>>(await ExternalProviderSettingsHelper.GetAllAsync())
        };
    }

    public virtual async Task<ExternalProviderItemWithSecretDto> GetByNameAsync(GetByNameInput input)
    {
        using (CurrentTenant.Change(input.TenantId))
        {
            var definition = ExternalProviderSettingsHelper.GetDefinitionsByNameOrNull(input.Name);
            if (definition != null)
            {
                var settings = await ExternalProviderSettingsHelper.GetByNameAsync(definition.Name, fallBackToHost: true);
                //Encrypt the secret values.
                settings.SecretProperties = settings.SecretProperties
                    .Select(secretValue => new ExternalProviderSettingsProperty(secretValue.Name, StringEncryptionService.Encrypt(secretValue.Value)))
                    .ToList();

                return ObjectMapper.Map<ExternalProviderSettings, ExternalProviderItemWithSecretDto>(settings);
            }

            return new ExternalProviderItemWithSecretDto
            {
                Success = false
            };
        }
    }
}
