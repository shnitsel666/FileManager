namespace FilesManager.Services.Application.TestService
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;

    public interface ITestService
    {
        /// <summary>
        /// Checks if file is opened.
        /// </summary>
        /// <param name="downloadRequest">Downloaded file information.</param>
        Response<bool> IsFileOpened(DownloadRequest downloadRequest);

        /// <summary>
        /// Checks if file exists.
        /// </summary>
        /// <param name="downloadRequest">Downloaded file information.</param>
        Response<bool> IsFileExists(DownloadRequest downloadRequest);

        /// <summary>
        /// Checks if file has readonly attribute.
        /// </summary>
        /// <param name="downloadRequest">Downloaded file information.</param>
        Response<bool> IsFileReadonly(DownloadRequest downloadRequest);

        /// <summary>
        /// Checks if file in downloads history.
        /// </summary>
        /// <param name="downloadRequest">Downloaded file information.</param>
        Response<bool> IsFileNotTracking(DownloadRequest downloadRequest);
    }
}
