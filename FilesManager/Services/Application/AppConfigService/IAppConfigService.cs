namespace FilesManager.Services.Application.AppConfigService
{
    using FilesManager.Models.Infrastructure;

    public interface IAppConfigService
    {
        AppConfig GetConfig();
    }
}
