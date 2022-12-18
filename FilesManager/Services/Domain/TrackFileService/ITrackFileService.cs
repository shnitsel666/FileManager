namespace FilesManager.Services.Domain.TrackFileService
{
    using System;
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Data;
    using FilesManager.Models.Infrastructure;

    /// <summary>
    /// Service is responsible for tracking files changes history.
    /// </summary>
    public interface ITrackFileService
    {
        /// <summary>
        /// Adds files to downloads history with download time tracking with custom path.
        /// </summary>
        Response<bool> TrackFileHistory(DownloadRequest downloadRequest, string selectedDownloadPath);

        /// <summary>
        /// Adds files to downloads history with download time tracking with custom path.
        /// </summary>
        Response<bool> TrackFileHistory(DownloadRequest downloadRequest);

        /// <summary>
        /// Restores downloads history file.
        /// </summary>
        Response<bool> CheckAndRestoreHistoryFile();

        /// <summary>
        /// Gets local path to saved file.
        /// </summary>
        Response<string> GetSavedFilePath(string fileName);

        /// <summary>
        /// Checks if file was changed by comparing file writing datetime.
        /// </summary>
        Response<bool> WasFileChanged(string newFileName, DateTime fileNewDownloadDateTime);

        /// <summary>
        /// Check if history file is not empty and exists, or recreate new empty history file.
        /// </summary>
        Response<FilesHistory> GetFilesHistory();

        /// <summary>
        /// Rewrites files downloading history.
        /// </summary>
        Response<bool> RewriteFileHistory(FilesHistory filesHistory);

        /// <summary>
        /// Checks file in files history.
        /// </summary>
        Response<bool> FilesHistoryItemExists(string fileName);
    }
}
