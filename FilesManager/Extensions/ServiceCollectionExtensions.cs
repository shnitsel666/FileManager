namespace FilesManager.Extensions
{
    using FilesManager.Services.Application.AppConfigService;
    using FilesManager.Services.Application.SignalRService;
    using FilesManager.Services.Application.TestService;
    using FilesManager.Services.Domain.ConnectService;
    using FilesManager.Services.Domain.DeleteFileService;
    using FilesManager.Services.Domain.DownloadFileService;
    using FilesManager.Services.Domain.TrackFileService;
    using FilesManager.Services.Domain.UploadFileService;
    using FilesManager.Services.FilesService;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfig(this IServiceCollection services)
        {
            services.AddSingleton<IAppConfigService, AppConfigService>();
            return services;
        }

        public static IServiceCollection AddSignalRService(this IServiceCollection services)
        {
            services.AddSingleton<ISignalRService, SignalRService>();
            return services;
        }

        public static IServiceCollection AddFilesServices(this IServiceCollection services)
        {
            services.AddSingleton<ITrackFileService, TrackFileService>();
            services.AddTransient<IConnectService, ConnectService>();
            services.AddTransient<IUploadFileService, UploadFileService>();
            services.AddSingleton<IDownloadFileService, DownloadFileService>();
            services.AddTransient<IDeleteFileService, DeleteFileService>();
            services.AddSingleton<IFilesService, FilesService>();
            services.AddSingleton<ITestService, TestService>();
            return services;
        }
    }
}