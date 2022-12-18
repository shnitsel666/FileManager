namespace FilesManager.Services.Application.TestService
{
    using FilesManager.HelpersMethods;
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Application.AppConfigService;
    using FilesManager.Services.Domain.TrackFileService;

    public class TestService : ITestService
    {
        private readonly AppConfig _config;
        private readonly ITrackFileService _trackFileService;

        #region .ctor
        public TestService(IAppConfigService appConfigService, ITrackFileService trackFileService)
        {
            _config = appConfigService.GetConfig();
            _trackFileService = trackFileService;
        }
        #endregion

        public Response<bool> IsFileOpened(DownloadRequest downloadRequest) =>
            Response<bool>.DoMethod(resp =>
            {
                string newFileName = Helpers.GetFileName(downloadRequest.FileName, _config.FileNamePrefix, downloadRequest.FileId, downloadRequest.FileVersionId);
                string fileFullPath = Path.Combine(_config.FilesUploadBasePath, newFileName);
                resp.Data = Helpers.IsFileLocked(fileFullPath);
            });

        public Response<bool> IsFileExists(DownloadRequest downloadRequest) =>
            Response<bool>.DoMethod(resp =>
            {
                string newFileName = Helpers.GetFileName(downloadRequest.FileName, _config.FileNamePrefix, downloadRequest.FileId, downloadRequest.FileVersionId);
                string fileFullPath = Path.Combine(_config.FilesUploadBasePath, newFileName);
                resp.Data = File.Exists(fileFullPath);
            });

        public Response<bool> IsFileReadonly(DownloadRequest downloadRequest) =>
            Response<bool>.DoMethod(resp =>
            {
                string newFileName = Helpers.GetFileName(downloadRequest.FileName, _config.FileNamePrefix, downloadRequest.FileId, downloadRequest.FileVersionId);
                string fileFullPath = Path.Combine(_config.FilesUploadBasePath, newFileName);
                FileAttributes attributes = File.GetAttributes(fileFullPath);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    resp.Data = true;
                }
                else
                {
                    resp.Data = false;
                }
            });

        public Response<bool> IsFileNotTracking(DownloadRequest downloadRequest) =>
            Response<bool>.DoMethod(resp =>
            {
                string newFileName = Helpers.GetFileName(downloadRequest.FileName, _config.FileNamePrefix, downloadRequest.FileId, downloadRequest.FileVersionId);
                resp.Data = _trackFileService.FilesHistoryItemExists(newFileName).GetResultIfNotError();
            });
    }
}
