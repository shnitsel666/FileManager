namespace FilesManager.Services.Domain.TrackFileService
{
    using System;
    using System.IO;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text.Unicode;
    using FilesManager.HelpersMethods;
    using FilesManager.Models;
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Data;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Application.AppConfigService;

    public class TrackFileService : ITrackFileService
    {
        public static AppConfig _config { get; private set; }

        private JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        #region .ctor
        public TrackFileService(IAppConfigService appConfigService)
        {
            _config = appConfigService.GetConfig();
        }
        #endregion

        #region TrackFileHistory(downloadRequest, selectedDownloadPath)
        public Response<bool> TrackFileHistory(DownloadRequest downloadRequest, string selectedDownloadPath) =>
            Response<bool>.DoMethod(resp =>
            {
                Logger.Log.Info("Запись файла в историю скачиваемых файлов...");
                CheckAndRestoreHistoryFile();
                string newFileName = Helpers.GetFileName(downloadRequest.FileName, _config.FileNamePrefix, downloadRequest.FileId, downloadRequest.FileVersionId);
                string downloadPath = !string.IsNullOrEmpty(selectedDownloadPath) ? selectedDownloadPath : _config.FilesUploadBasePath;
                string fullDownloadPath = Path.Combine(_config.FilesUploadBasePath, newFileName);
                DateTime fileDownloadTime = File.GetLastWriteTime(fullDownloadPath);
                FilesHistory filesHistory = GetFilesHistory().GetResultIfNotError();
                if (FilesHistoryItemExists(@newFileName).GetResultIfNotError())
                {
                    filesHistory.Files.Remove(@newFileName);
                    filesHistory.Files.Add(newFileName, new FilesHistoryItem()
                    {
                        SelectedPath = @downloadPath,
                        DownloadTime = @fileDownloadTime,
                        FileId = downloadRequest.FileId,
                        FileVersionId = downloadRequest.FileVersionId,
                        OriginalName = downloadRequest.FileName,
                        WasSent = false,
                        WasClosed = false,
                        WasOpened = false,
                        UID = downloadRequest.UID,
                    });
                }
                else
                {
                    filesHistory.Files.Add(newFileName, new FilesHistoryItem()
                    {
                        SelectedPath = @downloadPath,
                        DownloadTime = @fileDownloadTime,
                        FileId = downloadRequest.FileId,
                        FileVersionId = downloadRequest.FileVersionId,
                        OriginalName = downloadRequest.FileName,
                        WasSent = false,
                        WasClosed = false,
                        WasOpened = false,
                        UID = downloadRequest.UID,
                    });
                }

                RewriteFileHistory(filesHistory);
                Logger.Log.Info("Запись файла в историю скачиваемых файлов завершена");
            });
        #endregion

        #region TrackFileHistory(downloadRequest)
        public Response<bool> TrackFileHistory(DownloadRequest downloadRequest) =>
            TrackFileHistory(downloadRequest, string.Empty);
        #endregion

        #region RewriteFileHistory()
        public Response<bool> RewriteFileHistory(FilesHistory filesHistory) =>
            Response<bool>.DoMethod(resp =>
            {
                File.WriteAllText(_config.FilesHistoryPath, JsonSerializer.Serialize(filesHistory, _jsonOptions));
            });
        #endregion

        #region WasFileChanged()
        public Response<bool> WasFileChanged(string newFileName, DateTime fileNewDownloadDateTime) =>
            Response<bool>.DoMethod(resp =>
            {
                Logger.Log.Info($"Проверка файла {newFileName} в истории изменений файлов...");
                CheckAndRestoreHistoryFile();
                FilesHistory filesHistory = GetFilesHistory().GetResultIfNotError();
                if (FilesHistoryItemExists(newFileName).GetResultIfNotError())
                {
                    DateTime downloadDateTime = filesHistory.Files[@newFileName].DownloadTime;
                    if (downloadDateTime != fileNewDownloadDateTime)
                    {
                        Logger.Log.Info($"Файл {newFileName} был изменён.");
                        resp.Data = true;
                    }
                    else
                    {
                        Logger.Log.Info($"Изменений файла {newFileName} не обнаружено.");
                        resp.Data = false;
                    }
                }
                else
                {
                    Logger.Log.Info($"Файла {newFileName} нет в истории скачиваний.");
                    resp.Data = false;
                }
            });
        #endregion

        #region GetSavedFilePath()
        public Response<string> GetSavedFilePath(string fileName) =>
            Response<string>.DoMethod(resp =>
            {
                Logger.Log.Info("Получение пути файла в истории изменений файлов...");
                FilesHistory filesHistory = GetFilesHistory().GetResultIfNotError();
                bool fileExists = FilesHistoryItemExists(fileName).GetResultIfNotError();
                resp.Data = fileExists ? filesHistory.Files[@fileName].SelectedPath : string.Empty;
            });
        #endregion

        #region FilesHistoryItemExists()
        public Response<bool> FilesHistoryItemExists(string fileName) =>
            Response<bool>.DoMethod(resp =>
            {
                FilesHistory filesHistory = GetFilesHistory().GetResultIfNotError();
                resp.Data = filesHistory.Files != null && filesHistory.Files.ContainsKey(@fileName);
            });
        #endregion

        #region GetFilesHistory()
        public Response<FilesHistory> GetFilesHistory() =>
            Response<FilesHistory>.DoMethod(resp =>
            {
                CheckAndRestoreHistoryFile();
                string filesHistory = File.ReadAllText(_config.FilesHistoryPath);
                FilesHistory filesHistoryParsed = JsonSerializer.Deserialize<FilesHistory>(filesHistory, _jsonOptions);
                resp.Data = filesHistoryParsed;
            });
        #endregion

        #region CheckAndRestoreHistoryFile()
        public Response<bool> CheckAndRestoreHistoryFile() =>
            Response<bool>.DoMethod(resp =>
            {
                Logger.Log.Info($"Проверка файла истории загрузок: {_config.FilesHistoryPath}");
                if (!File.Exists(_config.FilesHistoryPath))
                {
                    Logger.Log.Info($"Файл истории загрузок {_config.FilesHistoryPath} не найден.");
                    FilesHistory filesHistory = RestoreHistoryFileObject();
                    FileStream file = File.Create(_config.FilesHistoryPath);
                    file.Close();
                    RewriteFileHistory(filesHistory);
                    Logger.Log.Info($"Файл истории загрузок {_config.FilesHistoryPath} восстановлен.");
                }
                else
                {
                    Logger.Log.Info($"Проверка файла истории загрузок {_config.FilesHistoryPath} завершена успешно.");
                }
            });
        #endregion

        #region RestoreHistoryFileObject()
        private static FilesHistory RestoreHistoryFileObject()
        {
            FilesHistory filesHistory = new()
            {
                Files = new(),
            };
            return filesHistory;
        }
        #endregion
    }
}
