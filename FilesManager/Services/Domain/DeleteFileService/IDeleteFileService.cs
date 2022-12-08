namespace FilesManager.Services.Domain.DeleteFileService
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;

    /// <summary>
    /// Service is responsible for deleting useless files from local machine.
    /// </summary>
    public interface IDeleteFileService
    {
        /// <summary>
        /// Deletes file from local machine and from files downloading history.
        /// </summary>
        Response<bool> Delete(DeleteRequest deleteRequest);
    }
}
