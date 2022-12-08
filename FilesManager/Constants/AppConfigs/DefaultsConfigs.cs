namespace FilesManager.Constants.AppConfigs
{
    using System.IO;

    public class DefaultsConfigs
    {
        public static readonly string CurrentVersion = "1.0.2";
        public static readonly string FileNamePrefix = "_FAD_";
        public static readonly string LanimageUrl = "http://localhost:9001";
        public static readonly string FilesUploadBasePath = Path.Combine(DefaultsParams.LocalFolder, DefaultsParams.FilesManagerFolder, DefaultsParams.UploadsFolder);
        public static readonly string FilesHistoryPath = Path.Combine(DefaultsParams.LocalFolder, DefaultsParams.FilesManagerFolder, DefaultsParams.HistoryFilePath);
    }
}
