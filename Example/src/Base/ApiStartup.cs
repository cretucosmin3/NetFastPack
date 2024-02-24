using Newtonsoft.Json;

namespace Example.Base;

public abstract class ApiStartup
{
    protected readonly IConfiguration Configuration;

    protected ApiStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            // builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Critical);
        });

        AddWebApiInitialization(services);

        FluentValidation.ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddHttpContextAccessor();
    }

    public virtual void AddWebApiInitialization(IServiceCollection services)
    {
        services.AddMvcCore()
            .AddFormatterMappings()
            .AddNewtonsoftJson(_ => _.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

        services.AddCors();
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            var controllerMap = endpoints.MapControllers();
        });

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
            await next();
        });
    }
}