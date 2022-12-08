namespace FilesManager
{
    using System;
    using System.IO;
    using log4net;
    using log4net.Config;

    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
        }
    }
}
