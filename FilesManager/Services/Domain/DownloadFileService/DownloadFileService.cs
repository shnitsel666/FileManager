namespace FilesManager.Services.Domain.DownloadFileService
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using FilesManager.HelpersMethods;
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Data;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Application.AppConfigService;
    using FilesManager.Services.Application.SignalRService;
    using FilesManager.Services.Domain.TrackFileService;

    public class DownloadFileService : IDownloadFileService
    {
        private readonly ISignalRService _signalRService;

        private readonly ITrackFileService _trackFileService;

        private readonly AppConfig _config;

        private Thread threadStartFilesTracking;

        #region .ctor
        public DownloadFileService(IAppConfigService appConfigService, ISignalRService signalRService, ITrackFileService trackFileService)
        {
            _config = appConfigService.GetConfig();
            _signalRService = signalRService;
            _trackFileService = trackFileService;

            // Запускаем тред с сервисом SignalR с высоким приоритетом, чтобы на слабых машинах сервис не отлетел.
            // Запускаем его в конструкторе и потом проеряем что тред жив, если он упал - пересоздаём его.
            threadStartFilesTracking = new(_signalRService.StartFilesTracking)
            {
                Name = "Thread StartFilesTracking",
                Priority = ThreadPriority.Highest,
            };
            threadStartFilesTracking.Start();
        }
        #endregion

        #region Download()
        public Response<DownloadResponse> Download(DownloadRequest downloadRequest) =>
            Response<DownloadResponse>.DoMethod(resp =>
            {
                if(downloadRequest.UseSignalR)
                {
                    try
                    {
                        // На случай, если поток с сервисом SignalR упал, мы его пересоздаём.
                        if (!threadStartFilesTracking.IsAlive)
                        {
                            threadStartFilesTracking = new(_signalRService.StartFilesTracking)
                            {
                                Name = "Thread StartFilesTracking",
                                Priority = ThreadPriority.Highest,
                            };
                            threadStartFilesTracking.Start();
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Info($"DownloadSignalR threadStartFilesTracking error: {e.Message}");
                        Logger.Log.Info($"DownloadSignalR threadStartFilesTracking error: {e.StackTrace}");
                    }
                }
                string newFileName = Helpers.GetFileName(downloadRequest.FileName, _config.FileNamePrefix, downloadRequest.FileId, downloadRequest.FileVersionId);
                string fileFullPath = Path.Combine(_config.FilesUploadBasePath, newFileName);
                Logger.Log.Info($"New file local name: {downloadRequest.FileName}");
                if (!Directory.Exists(_config.FilesUploadBasePath))
                {
                    Directory.CreateDirectory(_config.FilesUploadBasePath);
                }

                if (File.Exists(fileFullPath))
                {
                    Logger.Log.Info($"Найден дубликат файла: {fileFullPath}");
                    Logger.Log.Info($"Удаление дубликата: {fileFullPath}");
                    File.SetAttributes(fileFullPath, File.GetAttributes(fileFullPath) & ~FileAttributes.ReadOnly);
                    File.Delete(fileFullPath);
                    Logger.Log.Info($"Удаление дубликата завершено: {fileFullPath}");
                }

                Logger.Log.Info($"File saving: {fileFullPath}");
                File.WriteAllBytes(fileFullPath, Convert.FromBase64String(downloadRequest.FileBase64));
                if (downloadRequest.Readonly == true)
                {
                    File.SetAttributes(fileFullPath, FileAttributes.ReadOnly);
                }
                Logger.Log.Info($"File was saved as: {fileFullPath}");

                // Small pause for Linux file system.
                if (Helpers.IsLinux())
                {
                    Thread.Sleep(1500);
                }
                if (downloadRequest.OpenForView == true) {
                    Task.Run(() => OpenFileTask(downloadRequest));
                }
                if (!downloadRequest.OpenForView && downloadRequest.TrackHistory == true)
                {
                    AddFileToHistory(downloadRequest);
                }
                Thread.Sleep(1000);
                DateTime fileDownloadTime = File.GetLastWriteTime(fileFullPath);

                DownloadResponse downloadedData = new()
                {
                    FileName = newFileName,
                    DownloadTime = fileDownloadTime,
                    FileLocalPath = fileFullPath,
                    FileId = downloadRequest.FileId,
                    FileVersionId = downloadRequest.FileVersionId,
                };
                resp.Data = downloadedData;
                resp.Message = $"File was saved as: {fileFullPath}";
            });
        #endregion

        #region OpenFileTask(downloadRequest)
        private void OpenFileTask(DownloadRequest downloadRequest)
        {
            string newFileName = Helpers.GetFileName(downloadRequest.FileName, _config.FileNamePrefix, downloadRequest.FileId, downloadRequest.FileVersionId);
            var fileFullPath = Path.Combine(_config.FilesUploadBasePath, newFileName);
            Logger.Log.Info($"Trying to open file: {fileFullPath}");
            try
            {
                if (File.Exists(fileFullPath))
                {
                    if (!Helpers.IsLinux())
                    {
                        OpenFileWindows(fileFullPath);
                    }
                    else
                    {
                        OpenFileLinux(fileFullPath);
                    }

                    if (downloadRequest.TrackHistory == true && !string.IsNullOrEmpty(newFileName))
                    {
                        AddFileToHistory(downloadRequest);
                    }
                }
            }
            catch (Exception error)
            {
                Logger.Log.Error($"Ошибка при открытии файла {fileFullPath}: {error.Message}, {error.StackTrace}");
            }
        }
        #endregion

        #region AddFileToHistory()
        private void AddFileToHistory(DownloadRequest downloadRequest)
        {
            string newFileName = Helpers.GetFileName(downloadRequest.FileName, _config.FileNamePrefix, downloadRequest.FileId, downloadRequest.FileVersionId);
            _trackFileService.TrackFileHistory(downloadRequest);
            FilesHistory filesHistory = _trackFileService.GetFilesHistory().GetResultIfNotError();
            FilesHistoryItem file = filesHistory.Files[@newFileName];
            if (file != null)
            {
                filesHistory.Files[@newFileName].WasSent = false;
                filesHistory.Files[@newFileName].WasOpened = true;
                filesHistory.Files[@newFileName].UID = downloadRequest.UID;
                _trackFileService.RewriteFileHistory(filesHistory);

                Thread.Sleep(6000);
                filesHistory.Files[@newFileName].WasClosed = true;
                _trackFileService.RewriteFileHistory(filesHistory);
            }
        }
        #endregion

        #region OpenFileWindows()
        private void OpenFileWindows(string downloadedFilePath)
        {
            Process process = Process.Start(@"cmd.exe ", "/c " + downloadedFilePath);
            process.PriorityClass = ProcessPriorityClass.High;
        }
        #endregion

        #region OpenFileLinux()
        private void OpenFileLinux(string downloadedFilePath)
        {
            string cmd = "xdg-open " + @downloadedFilePath;
            string escapedArgs = cmd.Replace("\"", "\\\"");
            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };
            process.Start();
            process.PriorityClass = ProcessPriorityClass.High;
            process.StandardOutput.ReadToEnd();
        }
        #endregion
    }
}
