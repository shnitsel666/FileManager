namespace FilesManager.Services.FilesService
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;

    public interface IFilesService
    {
        /// <summary>
        /// Downloads file to local machine.
        /// </summary>
        /// <param name="downloadRequest">Download request.</param>
        Response<DownloadResponse> Download(DownloadRequest downloadRequest);

        /// <summary>
        /// Connect application with webclient.
        /// </summary>
        /// <param name="appVersion">App version.</param>
        Response<ConnectResponse> Connect(string appVersion);

        /// <summary>
        /// Returns files to webclient.
        /// </summary>
        /// <param name="uploadRequest">Upload request.</param>
        Response<UploadResponse> Upload(UploadRequest uploadRequest);

        /// <summary>
        /// Deletes file from local machine.
        /// </summary>
        /// <param name="deleteRequest">Delete request.</param>
        Response<bool> Delete(DeleteRequest deleteRequest);
    }
}
