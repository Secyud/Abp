using AutoMapper;
using Secyud.Abp.Settings.Components;

namespace Secyud.Abp.Settings;

public class SettingsBlazorAutoMapperProfile : Profile
{
    public SettingsBlazorAutoMapperProfile()
    {
        CreateMap<EmailSettingGroupViewComponent.UpdateEmailSettingsViewModel, UpdateEmailSettingsDto>();
        CreateMap<EmailSettingsDto, EmailSettingGroupViewComponent.UpdateEmailSettingsViewModel>();
        CreateMap<EmailSettingGroupViewComponent.SendTestEmailViewModel, SendTestEmailInput>();
    }
}