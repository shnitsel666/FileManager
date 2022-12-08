namespace FilesManager.Models.Data
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class FilesHistory
    {
        [JsonPropertyName("files")]
        public Dictionary<string, FilesHistoryItem> Files { get; set; }
    }
}
