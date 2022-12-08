namespace FilesManager.Models.ApiModels
{
    public class UploadRequest
    {
        public string FileName { get; set; }

        public int FileId { get; set; }

        public int FileVersionId { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла для события SignalR.
        /// </summary>
        public string Uid { get; set; }
    }
}
