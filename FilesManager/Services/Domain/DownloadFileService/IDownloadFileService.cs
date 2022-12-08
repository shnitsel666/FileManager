namespace FilesManager.Services.Domain.DownloadFileService
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;

    /// <summary>
    /// Service is responsible for downloading files from webclient and tracking their changes.
    /// </summary>
    public interface IDownloadFileService
    {
        /// <summary>
        /// Downloads file to local machine.
        /// </summary>
        Response<DownloadResponse> Download(DownloadRequest downloadRequest);
    }
}
