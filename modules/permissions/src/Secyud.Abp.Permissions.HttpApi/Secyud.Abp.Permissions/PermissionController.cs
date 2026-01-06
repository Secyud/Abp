using Microsoft.AspNetCore.Mvc;
using Secyud.Abp.HttpApi;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Permissions;

[RemoteService(Name = PermissionsRemoteServiceConsts.RemoteServiceName)]
[Area(PermissionsRemoteServiceConsts.ModuleName)]
[AutoController(typeof(IPermissionAppService))]
public partial class PermissionController : AbpControllerBase
{
}