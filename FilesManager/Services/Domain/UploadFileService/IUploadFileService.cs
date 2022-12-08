namespace FilesManager.Services.Domain.UploadFileService
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;

    /// <summary>
    /// Service is responsible for files returning to webclient.
    /// </summary>
    public interface IUploadFileService
    {
        /// <summary>
        /// Returns files to webclient.
        /// </summary>
        Response<UploadResponse> Upload(UploadRequest uploadRequest);
    }
}
