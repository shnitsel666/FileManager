namespace FilesManager.Services.FilesService
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Domain.ConnectService;
    using FilesManager.Services.Domain.DeleteFileService;
    using FilesManager.Services.Domain.DownloadFileService;
    using FilesManager.Services.Domain.UploadFileService;

    public class FilesService : IFilesService
    {
        private readonly IConnectService _connectService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IDownloadFileService _downloadFileService;
        private readonly IDeleteFileService _deleteFileService;

        #region .ctor
        public FilesService(
            IConnectService connectService,
            IUploadFileService uploadFileService,
            IDownloadFileService downloadFileService,
            IDeleteFileService deleteFileService)
        {
            _connectService = connectService;
            _uploadFileService = uploadFileService;
            _downloadFileService = downloadFileService;
            _deleteFileService = deleteFileService;
        }
        #endregion

        public Response<DownloadResponse> Download(DownloadRequest downloadRequest) =>
            _downloadFileService.Download(downloadRequest);

        public Response<ConnectResponse> Connect(string appVersion) =>
            _connectService.Connect(appVersion);

        public Response<UploadResponse> Upload(UploadRequest uploadRequest) =>
            _uploadFileService.Upload(uploadRequest);

        public Response<bool> Delete(DeleteRequest deleteRequest) =>
            _deleteFileService.Delete(deleteRequest);
    }
}
