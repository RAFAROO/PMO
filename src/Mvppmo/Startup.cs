// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Mvppmo.Application;
using Mvppmo.Configuration;
using Mvppmo.Infrastructure.Configuration;
using System;

[assembly: ApiController]

namespace Mvppmo;

public class Startup : IStartup
{
    public virtual void Configure(IConfiguration configuration, IServiceCollection services)
    {
        services
            .AddAppSettingsModule(configuration);


        AddDatabase(configuration, services);
    }

    public virtual void ConfigureServices(IServiceCollection services, IHostEnvironment environment)
    {
        services.AddMediatR(typeof(ApplicationClassesAssemblyHelper));
        IdentityModelEventSource.ShowPII = true;
        services
            .AddSecurityModule()
            .AddProblemDetailsModule(environment)
            .AddAutoMapperModule()
            .AddSwaggerModule()
            .AddWebModule()
            .AddRepositoryModule()
            .AddServiceModule();
    }

    public virtual void ConfigureMiddleware(IApplicationBuilder app, IHostEnvironment environment)
    {
        IServiceProvider serviceProvider = app.ApplicationServices;
        var securitySettingsOptions = serviceProvider.GetRequiredService<IOptions<SecuritySettings>>();
        var securitySettings = securitySettingsOptions.Value;
        app
            .UseApplicationSecurity(securitySettings)
            .UseApplicationProblemDetails()
            .UseApplicationDatabase(environment);
    }

    public virtual void ConfigureEndpoints(IApplicationBuilder app, IHostEnvironment environment)
    {
        app
            .UseApplicationSwagger()
            .UseApplicationWeb(environment);
    }

    protected virtual void AddDatabase(IConfiguration configuration, IServiceCollection services)
    {
        services.AddDatabaseModule(configuration);
    }
}
