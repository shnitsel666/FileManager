namespace FilesManager
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using FilesManager.Extensions;
    using FilesManager.Hubs;
    using FilesManager.Infrastructure.Filters;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup()
        {
            Logger.InitLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                    builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
            services.AddAppConfig();
            services.AddFilesServices();
            services.AddSignalRService();
            services.AddSwaggerGen(options =>
            {
                string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            services.AddSignalR(hubOptions =>
            {
                hubOptions.KeepAliveInterval = System.TimeSpan.FromMinutes(1000);
            });

            IMvcBuilder mvcBuilder = services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });

            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<FileAgentHub>("/events");
            });
        }
    }
}