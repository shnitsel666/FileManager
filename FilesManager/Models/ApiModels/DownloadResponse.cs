namespace FilesManager.Models.ApiModels
{
    using System;

    public class DownloadResponse
    {
        /// <summary>
        /// Имя файла после сохранения.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Время загрузки.
        /// </summary>
        public DateTime DownloadTime { get; set; }

        /// <summary>
        /// Локальный путь.
        /// </summary>
        public string FileLocalPath { get; set; }

        /// <summary>
        /// ID файла.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// ID версии файла.
        /// </summary>
        public int FileVersionId { get; set; }
    }
}
