using System;
using System.Reflection;
using Azure.Identity;
//using Fabillio.Common.Actors;
//using Fabillio.Common.Actors.CronActors;
//using Fabillio.Ordering.API.Controllers;
using Fabillio.Ordering.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(
    typeof(Program).FullName, LogLevel.Warning);

builder.Host.UseDefaultServiceProvider(opt => opt.ValidateScopes = false);

if (!builder.Environment.IsDevelopment())
{
    var keyVaultName = builder.Configuration.GetValue<string>("KeyVaultName");
    var keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
}

builder.Host.ConfigureServices(services =>
{
    services.AddApplicationInsightsTelemetry();

    services
        .AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });


    services.AddDaprClient();

    services.AddHttpContextAccessor();

    services.AddSwaggerDocumentation();

    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            b => b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    // services.AddActors(options =>
    // {
    //     options.Actors.RegisterActor<OrderingOutboxEventsCronActor>();
    // });
    //
    // services.AddCronActors();
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("CorsPolicy");
}

app.UseSwaggerDocumentation();

app.UseStaticFiles();

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

app.UseRouting();
app.UseCloudEvents();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", context => context.Response.WriteAsync("Ordering Service"))
        .WithMetadata(new AllowAnonymousAttribute());
    endpoints.MapControllers();
    //endpoints.MapActorsHandlers();
});

// if (!app.Environment.IsDevelopment())
// {
//     app.Lifetime.ApplicationStarted.Register(async () => 
//     {
//         var actorsScheduler = app.Services.GetRequiredService<ICronActorsScheduler>();
//         await actorsScheduler.ScheduleCronActors();
//     });
// }

await app.RunAsync();