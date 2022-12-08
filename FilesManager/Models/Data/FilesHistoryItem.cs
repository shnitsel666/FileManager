namespace FilesManager.Models.Data
{
    using System;
    using System.Text.Json.Serialization;

    public class FilesHistoryItem
    {
        /// <summary>
        /// Путь куда был скачан файл.
        /// </summary>
        [JsonPropertyName("selectedPath")]
        public string SelectedPath { get; set; }

        [JsonPropertyName("downloadTime")]
        public DateTime DownloadTime { get; set; }

        [JsonPropertyName("fileId")]
        public int FileId { get; set; }

        [JsonPropertyName("fileVersionId")]
        public int FileVersionId { get; set; }

        [JsonPropertyName("originalName")]
        public string OriginalName { get; set; }

        /// <summary>
        /// Файл был отправлен в вебклиент через SignalR.
        /// </summary>
        [JsonPropertyName("wasSent")]
        public bool WasSent { get; set; }

        /// <summary>
        /// Файл был закрыт ассоциированным приложением.
        /// </summary>
        [JsonPropertyName("wasClosed")]
        public bool WasClosed { get; set; }

        /// <summary>
        /// Файл был открыт ассоциированным приложением.
        /// </summary>
        [JsonPropertyName("wasOpened")]
        public bool WasOpened { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла для события SignalR.
        /// </summary>
        [JsonPropertyName("UID")]
        public string UID { get; set; }
    }
}
