namespace FilesManager.Services.FilesService
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;

    public interface IFilesService
    {
        Response<DownloadResponse> Download(DownloadRequest downloadRequest);

        Response<ConnectResponse> Connect(string appVersion);

        Response<UploadResponse> Upload(UploadRequest uploadRequest);

        Response<bool> Delete(DeleteRequest deleteRequest);
    }
}
