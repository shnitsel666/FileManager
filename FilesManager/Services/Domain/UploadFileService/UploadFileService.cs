namespace FilesManager.Services.Domain.UploadFileService
{
    using System;
    using System.IO;
    using FilesManager.Constants;
    using FilesManager.HelpersMethods;
    using FilesManager.Models;
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Application.AppConfigService;
    using FilesManager.Services.Domain.TrackFileService;

    public class UploadFileService : IUploadFileService
    {
        private readonly AppConfig _config;

        private readonly ITrackFileService _trackFileService;

        #region .ctor
        public UploadFileService(IAppConfigService appConfigService, ITrackFileService trackFileService)
        {
            _config = appConfigService.GetConfig();
            _trackFileService = trackFileService;
        }
        #endregion

        #region Upload()
        public Response<UploadResponse> Upload(UploadRequest uploadRequest) =>
            Response<UploadResponse>.DoMethod(resp =>
            {
                string newFileName = Helpers.GetFileName(uploadRequest.FileName, _config.FileNamePrefix, uploadRequest.FileId, uploadRequest.FileVersionId);
                string filePath = _trackFileService.GetSavedFilePath(newFileName).GetResultIfNotError();
                Logger.Log.Info($"Uploading file {uploadRequest.FileName} to webclient.");
                string fileUploadPath = Path.Combine(filePath, newFileName);
                if (!string.IsNullOrEmpty(filePath) && uploadRequest.FileId > 0 && !string.IsNullOrEmpty(uploadRequest.FileName) && uploadRequest.FileVersionId > 0)
                {
                    if (File.Exists(fileUploadPath))
                    {
                        DateTime lastDownloadTime = File.GetLastWriteTime(fileUploadPath);
                        bool fileWasChanged = _trackFileService.WasFileChanged(newFileName, lastDownloadTime).GetResultIfNotError();
                        if (fileWasChanged)
                        {
                            byte[] bytesFileToUpload = File.ReadAllBytes(fileUploadPath);
                            string base64FileToUpload = Convert.ToBase64String(bytesFileToUpload);
                            UploadResponse uploadResponse = new()
                            {
                                FileName = uploadRequest.FileName,
                                DownloadTime = lastDownloadTime,
                                FileLocalPath = fileUploadPath,
                                FileBase64 = base64FileToUpload,
                                FileId = uploadRequest.FileId,
                                FileVersionId = uploadRequest.FileVersionId
                            };
                            resp.Data = uploadResponse;
                            resp.Message = $"File {uploadRequest.FileName} has been sucessfully returned to webclient.";
                            Logger.Log.Info($"File {uploadRequest.FileName} has been sucessfully returned to webclient.");
                        }
                        else
                        {
                            resp.Code = Convert.ToInt32(ErrorCodes.FileHasntBeenChanged);
                            resp.Message = $"File {uploadRequest.FileName} hasn't been changed.";
                            Logger.Log.Info($"File {uploadRequest.FileName} hasn't been changed.");
                        }
                    }
                    else
                    {
                        Logger.Log.Error($"File {uploadRequest.FileName} hasn't been found.");
                        resp.Throw(Convert.ToInt32(ErrorCodes.FileHasntBeenFound), $"File {uploadRequest.FileName} hasn't been found.");
                    }
                }
                else
                {
                    Logger.Log.Error($"Incorrect upload request {Convert.ToString(uploadRequest)}");
                    resp.Throw(Convert.ToInt32(ErrorCodes.IncorrectUploadRequest), $"Incorrect upload request {Convert.ToString(uploadRequest)}");
                }
            });
        #endregion
    }
}
