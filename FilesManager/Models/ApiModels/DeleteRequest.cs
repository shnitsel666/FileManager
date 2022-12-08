namespace FilesManager.Models.ApiModels
{
    public class DeleteRequest
    {
        /// <summary>
        /// Original file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File ID.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// File version ID.
        /// </summary>
        public int FileVersionId { get; set; }
    }
}
