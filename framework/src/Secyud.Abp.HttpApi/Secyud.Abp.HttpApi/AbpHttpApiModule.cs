using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Secyud.Abp.HttpApi;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class AbpHttpApiModule : AbpModule
{
}