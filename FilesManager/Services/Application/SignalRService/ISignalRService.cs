namespace FilesManager.Services.Application.SignalRService
{
    public interface ISignalRService
    {
        void CleanHistory();

        void StartFilesTracking();
    }
}
