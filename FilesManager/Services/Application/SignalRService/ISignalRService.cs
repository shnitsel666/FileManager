namespace FilesManager.Services.Application.SignalRService
{
    public interface ISignalRService
    {
        /// <summary>
        /// Deletes uploaded files from history.
        /// </summary>
        void CleanHistory();

        /// <summary>
        /// Starts files changes tracking.
        /// </summary>
        void StartFilesTracking();
    }
}
