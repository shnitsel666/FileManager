namespace FilesManager.Services.Domain.ConnectService
{
    using FilesManager.Constants;
    using FilesManager.HelpersMethods;
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Application.AppConfigService;
    using FilesManager.Services.Domain.TrackFileService;
    using System;

    public class ConnectService : IConnectService
    {
        private readonly AppConfig _config;
        private readonly ITrackFileService _trackFileService;

        #region .ctor
        public ConnectService(IAppConfigService appConfigService, ITrackFileService trackFileService)
        {
            _config = appConfigService.GetConfig();
            _trackFileService = trackFileService;
        }
        #endregion

        #region Connect()
        public Response<ConnectResponse> Connect(string appVersion) =>
            Response<ConnectResponse>.DoMethod(resp =>
            {
                ConnectResponse connectResponse = new();
                Logger.Log.Info("Trying to connect...");
                if (_config.CurrentVersion == appVersion && !string.IsNullOrEmpty(appVersion))
                {
                    connectResponse.Platform = Helpers.IsLinux() ? Platforms.Linux : Platforms.Windows;
                    _trackFileService.CheckAndRestoreHistoryFile();
                    resp.Data = connectResponse;
                    resp.Message = "Successfully connected";
                }
                else
                {
                    Logger.Log.Error($"Application version: {_config.CurrentVersion} is not compatible with WebClient version {appVersion}");
                    resp.Throw(Convert.ToInt32(ErrorCodes.IncompatibleVersions), $"Application version: {_config.CurrentVersion} is not compatible with WebClient version {appVersion}");
                }
            });
        #endregion
    }
}
