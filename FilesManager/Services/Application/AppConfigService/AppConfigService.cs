namespace FilesManager.Services.Application.AppConfigService
{
    using System;
    using System.IO;
    using FilesManager.Constants.AppConfigs;
    using FilesManager.Models.Infrastructure;

    public class AppConfigService : IAppConfigService
    {
        private AppConfig _appConfig;
        private IConfiguration _configuration;

        #region .ctor
        public AppConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
            SetConfig();
        }
        #endregion

        #region GetConfig()

        public AppConfig GetConfig()
        {
            return _appConfig;
        }
        #endregion

        #region SetConfig()

        private AppConfig SetConfig()
        {
            _appConfig = new()
            {
                CurrentVersion = !string.IsNullOrEmpty(_configuration["CurrentVersion"]) ? _configuration["CurrentVersion"] : DefaultsConfigs.CurrentVersion,
                FileNamePrefix = !string.IsNullOrEmpty(_configuration["FileNamePrefix"]) ? _configuration["FileNamePrefix"] : DefaultsConfigs.FileNamePrefix,
                FilesUploadBasePath = DefaultsConfigs.FilesUploadBasePath,
                FilesHistoryPath = DefaultsConfigs.FilesHistoryPath,
            };
            CheckFolders(_appConfig);
            return _appConfig;
        }
        #endregion

        #region CheckFolders()

        /// <summary>
        /// Проверяет существование папок и по возможности создаёт их.
        /// </summary>
        private static void CheckFolders(AppConfig appConfig)
        {
            try
            {
                Logger.Log.Info("Проверка существование папки загрузок...");
                if (!Directory.Exists(appConfig.FilesUploadBasePath))
                {
                    Logger.Log.Info("Папка загрузок не найдена...");
                    Logger.Log.Info("Создание папки загрузок...");
                    Directory.CreateDirectory(appConfig.FilesUploadBasePath);
                    Logger.Log.Info("Папка загрузок создана...");
                }
                else
                {
                    Logger.Log.Info("Папка загрузок уже существует.");
                }
            }
            catch (Exception error)
            {
                Logger.Log.Error($"Ошибка проверки папки загрузки: {error.Message}");
                Logger.Log.Error($"Ошибка проверки папки загрузки: {error.StackTrace}");
            }
        }
        #endregion
    }
}
