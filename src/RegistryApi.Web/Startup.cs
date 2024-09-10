namespace RegistryApi.Web;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

using RegistryApi.Core.CustomerGroup;
using RegistryApi.Core.Interfaces;
using RegistryApi.Core.Messages;
using RegistryApi.Core.Services;
using RegistryApi.Core.Users;
using RegistryApi.Infrastructure.Data;
using RegistryApi.SharedKernel;
using RegistryApi.Web.Attributes;
using RegistryApi.Web.Errors;
using RegistryApi.Web.Roles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using ISystemClock = Microsoft.Extensions.Internal.ISystemClock;
using SystemClock = Microsoft.Extensions.Internal.SystemClock;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(_ => _.Filters.Add(typeof(UnhandledApiExceptionFilter)));
        var includeAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => _.FullName.Contains("RegistryApi")).ToArray();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(
                typeof(Startup).Assembly,
                typeof(DomainEntityBase).Assembly,
                typeof(RegistryContext).Assembly,
                typeof(Customer).Assembly);
        });

        // Swagger UI
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Registry API", Version = "v1" });
            option.EnableAnnotations();
        });

        // API Versioning
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;

            // Now, API calls can set their own version on Headers
            options.ApiVersionReader = ApiVersionReader.Combine(
                new MediaTypeApiVersionReader("version"),
                new HeaderApiVersionReader("X-Version")
            );
        });

        services.AddScoped<SlicktextSignatureAuthorizationAttribute>();
        services.TryAddSingleton<ISystemClock, SystemClock>();
        services.AddScoped<RegistryContextFactory>();
        services.AddHttpContextAccessor();

        services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        // Authorisation
        services.AddAuthorization(options =>
        {
            foreach (var map in RolePolicyMapping.Mapping)
                options.AddPolicy(map.Key, policy => policy.RequireRole(map.Value.ToArray()));
        });

        services.AddDbContext<UsersContext>(_ => _.UseMySQL(Configuration["ConnectionStrings:Users"] ?? ""));
        services.AddDbContext<GlobalRegistryContext>(_ => _.UseMySQL(Configuration["ConnectionStrings:Global"] ?? ""));
        services.AddScoped<ApiErrorFactory>();
        services.AddTransient(typeof(IUserRepository<>), typeof(UserRepository<>));
        services.AddTransient(typeof(IRegistryRepository<>), typeof(RegistryRepository<>));
        services.AddScoped<IRegistryRepository<IncomingMessage>>(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<RegistryContextFactory>();
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var context = factory.CreateContext(httpContextAccessor.HttpContext.User);
            return new RegistryRepository<IncomingMessage>(context);
        });
        services.AddScoped(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<RegistryContextFactory>();
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var context = factory.CreateContext(httpContextAccessor.HttpContext.User);
            return new StaysReadRepository(context);
        });
        services.AddScoped<IPasswordHasher<User>>(serviceProvider =>
        {
            var options = Options.Create(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
                IterationCount = 1000
            });
            return new PasswordHasher<User>(options);
        });

        services.AddScoped<ISaveIncomingMessageService, SaveIncomingMessageService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });

        //app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        loggerFactory.AddFile("Logs/registry-api-{Date}.txt");
    }
}
