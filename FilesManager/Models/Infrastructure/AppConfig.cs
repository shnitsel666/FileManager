namespace FilesManager.Models.Infrastructure
{
    public class AppConfig
    {
        /// <summary>
        /// Current version of application.
        /// </summary>
        public string CurrentVersion { get; init; }

        /// <summary>
        /// Prefix for downloaded files names.
        /// </summary>
        public string FileNamePrefix { get; init; }

        /// <summary>
        /// Path to downloaded files folder.
        /// </summary>
        public string FilesUploadBasePath { get; init; }

        /// <summary>
        /// Path to files history file.
        /// </summary>
        public string FilesHistoryPath { get; init; }
    }
}
