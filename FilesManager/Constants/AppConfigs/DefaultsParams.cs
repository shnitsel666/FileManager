namespace FilesManager.Constants.AppConfigs
{
    using System;
    using System.IO;

    public class DefaultsParams
    {
        public static readonly string LocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string FilesManagerFolder = "FilesManager";
        public static readonly string UploadsFolder = "uploads";
        public static readonly string ConfigFileName = "config.json";
        public static readonly string HistoryFilePath = "filesHistory.json";
        public static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
    }
}
