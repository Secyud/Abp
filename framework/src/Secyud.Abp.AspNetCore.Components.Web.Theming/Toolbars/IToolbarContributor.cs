﻿namespace Secyud.Abp.AspNetCore.Components.Toolbars;

public interface IToolbarContributor
{
    Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
}
