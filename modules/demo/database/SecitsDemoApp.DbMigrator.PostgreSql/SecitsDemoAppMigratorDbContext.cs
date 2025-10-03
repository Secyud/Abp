﻿using Microsoft.EntityFrameworkCore;
using SecitsDemoApp.EntityFrameworkCore;
using Secyud.Abp.Permissions.EntityFrameworkCore;
using Secyud.Abp.Settings.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SecitsDemoApp;

public class SecitsDemoAppMigratorDbContext(DbContextOptions<SecitsDemoAppMigratorDbContext> options) : AbpDbContext<SecitsDemoAppMigratorDbContext>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureSecitsDemoApp();
    }
}