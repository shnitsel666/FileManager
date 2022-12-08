namespace FilesManager.Models.ApiModels
{
    using System;

    public class UploadResponse
    {
        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Время загрузки.
        /// </summary>
        public DateTime DownloadTime { get; set; }

        /// <summary>
        /// Локальный путь к файлу.
        /// </summary>
        public string FileLocalPath { get; set; }

        /// <summary>
        /// File Base64.
        /// </summary>
        public string FileBase64 { get; set; }

        /// <summary>
        /// ID файла.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// ID версии файла.
        /// </summary>
        public int FileVersionId { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла для события SignalR.
        /// </summary>
        public string UID { get; set; }
    }
}
