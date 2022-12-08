namespace FilesManager.Services.Application.AppConfigService
{
    using System;
    using System.IO;
    using System.Text.Json;
    using FilesManager.Constants.AppConfigs;
    using FilesManager.Models;

    public class AppConfigService : IAppConfigService
    {
        private AppConfig _appConfig;

        #region .ctor
        public AppConfigService()
        {
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
            AppConfig appConfigJson = ReadConfig(DefaultsParams.ConfigFilePath);

            _appConfig = new()
            {
                CurrentVersion = !string.IsNullOrEmpty(appConfigJson.CurrentVersion) ? appConfigJson.CurrentVersion : DefaultsConfigs.CurrentVersion,
                FileNamePrefix = !string.IsNullOrEmpty(appConfigJson.FileNamePrefix) ? appConfigJson.FileNamePrefix : DefaultsConfigs.FileNamePrefix,
                LanimageUrl = !string.IsNullOrEmpty(appConfigJson.LanimageUrl) ? appConfigJson.LanimageUrl : DefaultsConfigs.LanimageUrl,
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

        #region ReadConfig()

        /// <summary>
        /// Считывает конфиг JSON.
        /// </summary>
        private static AppConfig ReadConfig(string path)
        {
            try
            {
                return File.Exists(@path) ? JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(@path)) : null;
            }
            catch (Exception error)
            {
                Logger.Log.Error($"Cannot read config.json {error.Message}, {error.StackTrace}");
                return null;
            }
        }
        #endregion
    }
}
