namespace FilesManager.Integration.Test.Models
{
    public class ConfigSettings
    {
        /// <summary>
        /// Integration settings.
        /// </summary>
        public Development DevelopmentSettings { get; set; } = new();
    }

    public class Development
    {
        /// <summary>
        /// Application host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Application port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Application version.
        /// </summary>
        public string Version { get; set; }
    }
}
