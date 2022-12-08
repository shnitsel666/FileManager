namespace FilesManager.HelpersMethods
{
    using System;
    using System.IO;
    using System.Text;
    using FilesManager.Models.ApiModels;

    public static class Helpers
    {
        #region IsLinux()

        /// <summary>
        /// Определяет текущую платформу (скрывает функционал сканирования в вебклиенте).
        /// </summary>
        public static bool IsLinux()
        {
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }
        #endregion

        #region IsFileLocked()
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                FileStream fs = File.OpenWrite(filePath);
                fs.Close();
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
        #endregion

        #region GetFileName()

        /// <summary>
        /// Формирует новое имя файла.
        /// </summary>
        public static string GetFileName(string fileName, string fileNamePrefix, long fileID, long fileVersionID)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }

            string nameCleared = fileName.Replace(' ', '_').Replace('(', '_').Replace(')', '_').Replace('{', '_').Replace('}', '_').Replace('"', '_').Replace('+', '_').Replace('#', '_').Replace('[', '_').Replace(']', '_').Replace('@', '_').Replace('&', '_').Replace(';', '_').Replace('№', '_').Replace('=', '_').Replace('$', '_').Replace('*', '_').Replace('%', '_').Replace(',', '_');
            string[] fileNameExploded = nameCleared.Split('.');
            StringBuilder newFileName = new();
            StringBuilder baseFileName = new();
            for (int count = 0; count < fileNameExploded.Length - 1; count++)
            {
                baseFileName.Append(fileNameExploded[count]);
            }

            newFileName.Append(fileNamePrefix).Append(baseFileName).Append('_').Append(fileID).Append("_v_").Append(fileVersionID).Append('.').Append(fileNameExploded[^1]);
            return newFileName.ToString();
        }
        #endregion
    }
}
