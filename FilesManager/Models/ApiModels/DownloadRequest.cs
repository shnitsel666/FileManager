namespace FilesManager.Models.ApiModels
{
    public class DownloadRequest
    {
        /// <summary>
        /// Original file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Base64 file encoded.
        /// </summary>
        public string FileBase64 { get; set; }

        /// <summary>
        /// File ID.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Unique file ID for SignalR event.
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// File version ID.
        /// </summary>
        public int FileVersionId { get; set; }

        /// <summary>
        /// Either should file be opened only for view.
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// Either should files changes be tracked or not.
        /// </summary>
        public bool TrackHistory { get; set; } = true;

        /// <summary>
        /// Either file should be opened in associated programm or not.
        /// </summary>
        public bool OpenForView { get; set; } = true;

        /// <summary>
        /// Either SignalR should be used for returning or not.
        /// </summary>
        public bool UseSignalR { get; set; }
    }
}
