namespace FilesManager.Services.Domain.DeleteFileService
{
    using System;
    using System.IO;
    using FilesManager.Constants;
    using FilesManager.HelpersMethods;
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Data;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Application.AppConfigService;
    using FilesManager.Services.Domain.TrackFileService;

    public class DeleteFileService : IDeleteFileService
    {
        private readonly AppConfig _config;

        private readonly ITrackFileService _trackFileService;

        #region .ctor
        public DeleteFileService(IAppConfigService appConfigService, ITrackFileService trackFileService)
        {
            _config = appConfigService.GetConfig();
            _trackFileService = trackFileService;
        }
        #endregion

        #region Delete()
        public Response<bool> Delete(DeleteRequest deleteRequest) =>
            Response<bool>.DoMethod(resp =>
            {
                Logger.Log.Info($"Deleting file: {deleteRequest.FileName}");
                string newFileName = Helpers.GetFileName(deleteRequest.FileName, _config.FileNamePrefix, deleteRequest.FileId, deleteRequest.FileVersionId);
                string filePath = _trackFileService.GetSavedFilePath(newFileName).GetResultIfNotError();
                string fileUploadPath = Path.Combine(filePath, newFileName);
                FilesHistory filesHistory = _trackFileService.GetFilesHistory().GetResultIfNotError();
                Logger.Log.Info($"Path for file to delete: {fileUploadPath}");
                if (File.Exists(fileUploadPath) && filesHistory.Files.ContainsKey(@newFileName))
                {
                    File.SetAttributes(fileUploadPath, File.GetAttributes(fileUploadPath) & ~FileAttributes.ReadOnly);
                    File.Delete(fileUploadPath);
                    filesHistory.Files.Remove(@newFileName);
                    _trackFileService.RewriteFileHistory(filesHistory);
                    resp.Data = true;
                    resp.Message = $"File {fileUploadPath} has been deleted.";
                    Logger.Log.Info($"File {fileUploadPath} has been deleted.");
                }
                else
                {
                    Logger.Log.Error($"File {fileUploadPath} hasn't been found and hasn't been deleted.");
                    resp.Throw(Convert.ToInt32(ErrorCodes.CouldntDeleteFile), $"File {fileUploadPath} hasn't been found and hasn't been deleted.");
                }
            });
        #endregion
    }
}
