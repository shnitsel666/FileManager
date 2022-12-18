namespace FilesManager.Services.Application.SignalRService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using FilesManager.HelpersMethods;
    using FilesManager.Hubs;
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Data;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Application.AppConfigService;
    using FilesManager.Services.Domain.TrackFileService;
    using Microsoft.AspNetCore.SignalR;

    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<FileAgentHub> _hubContext;

        private readonly ITrackFileService _trackFileService;

        private readonly AppConfig _config;

        #region .ctor
        public SignalRService(IAppConfigService appConfigService, IHubContext<FileAgentHub> hubContext, ITrackFileService trackFileService)
        {
            _config = appConfigService.GetConfig();
            _hubContext = hubContext;
            _trackFileService = trackFileService;
        }
        #endregion

        public void StartFilesTracking()
        {
            _trackFileService.CheckAndRestoreHistoryFile();
            CleanHistory();
            int num = 0;
            TimerCallback tm = new(FilesTracking);
            Timer timer = new(tm, num, 3000, 5000);
        }

        public void CleanHistory()
        {
            try
            {
                if (!Helpers.IsFileLocked(_config.FilesHistoryPath))
                {
                    FilesHistory filesHistory1 = _trackFileService.GetFilesHistory().GetResultIfNotError();
                    FilesHistory filesHistory2 = _trackFileService.GetFilesHistory().GetResultIfNotError();
                    foreach (KeyValuePair<string, FilesHistoryItem> file in filesHistory1.Files)
                    {
                        string fileUploadPath = Path.Combine(file.Value.SelectedPath, file.Key);
                        if (!File.Exists(fileUploadPath) || file.Value.WasSent == true)
                        {
                            filesHistory2.Files.Remove(file.Key);
                        }
                    }

                    _trackFileService.RewriteFileHistory(filesHistory2);
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Ошибка считывания истории файлов в SignalR: {e.Message}");
                Logger.Log.Error($"Ошибка считывания истории файлов в SignalR: {e.StackTrace}");
                Console.WriteLine($"Ошибка считывания истории файлов в SignalR: {e.Message}");
                Console.WriteLine($"Ошибка считывания истории файлов в SignalR: {e.StackTrace}");
            }
        }

        private void FilesTracking(object obj)
        {
            try
            {
                if (!Helpers.IsFileLocked(_config.FilesHistoryPath))
                {
                    FilesHistory filesHistory = _trackFileService.GetFilesHistory().GetResultIfNotError();
                    foreach (KeyValuePair<string, FilesHistoryItem> file in filesHistory.Files)
                    {
                        string fileUploadPath = Path.Combine(file.Value.SelectedPath, file.Key);
                        DateTime lastDownloadTime2 = file.Value.DownloadTime;
                        DateTime fileEditTime = File.GetLastWriteTime(fileUploadPath);
                        bool fileWasChanged = _trackFileService.WasFileChanged(file.Key, fileEditTime).GetResultIfNotError();
                        if (File.Exists(fileUploadPath) && fileWasChanged)
                        {
                            bool fileIsLocked = Helpers.IsFileLocked(fileUploadPath);
                            DateTime fileAccessTime = File.GetLastAccessTime(fileUploadPath);
                            DateTime lastDownloadTime = file.Value.DownloadTime;

                            Console.WriteLine("file: " + file.Key);
                            Console.WriteLine("fileIsLocked: " + fileIsLocked);
                            Console.WriteLine("wasOpened: " + file.Value.WasOpened);
                            Console.WriteLine("wasSent: " + file.Value.WasSent);
                            Console.WriteLine("wasClosed: " + file.Value.WasClosed);
                            Console.WriteLine("downloadTime: " + lastDownloadTime);
                            Console.WriteLine("fileAccessTime: " + fileAccessTime);
                            Console.WriteLine("fileEditTime: " + fileEditTime);
                            Console.WriteLine("*******************");
                            if (DateTime.Now > lastDownloadTime.AddSeconds(10) && lastDownloadTime.AddSeconds(5) < fileAccessTime)
                            {
                                file.Value.WasClosed = true;
                            }

                            if (!fileIsLocked && file.Value != null && file.Value.WasSent == false && file.Value.WasOpened == true && file.Value.WasClosed == true)
                            {
                                if (!string.IsNullOrEmpty(file.Value.OriginalName) && file.Value.FileId > 0 && file.Value.FileVersionId > 0)
                                {
                                    UploadRequest uploadRequest = new()
                                    {
                                        FileName = file.Value.OriginalName,
                                        FileId = file.Value.FileId,
                                        FileVersionId = file.Value.FileVersionId,
                                        Uid = file.Value.UID,
                                    };
                                    file.Value.WasSent = true;
                                    Thread.Sleep(1000);
                                    _hubContext.Clients.All.SendAsync($"FileAgentFileResponse_{uploadRequest.Uid}", uploadRequest);
                                }
                            }
                        }
                    }

                    _trackFileService.RewriteFileHistory(filesHistory);
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error($"Ошибка считывания истории файлов в SignalR: {e.Message}");
                Logger.Log.Error($"Ошибка считывания истории файлов в SignalR: {e.StackTrace}");
                Console.WriteLine($"Ошибка считывания истории файлов в SignalR: {e.Message}");
                Console.WriteLine($"Ошибка считывания истории файлов в SignalR: {e.StackTrace}");
            }
        }

    }
}
