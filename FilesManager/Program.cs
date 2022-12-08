namespace FilesManager
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        private static string baseAddress = "http://localhost:9000";

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int SW_HIDE = 0;

        public static void Main(string[] args)
        {
            try
            {
                // Скрываем консольное окно
                IntPtr handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);

                Process myProcess = Process.GetCurrentProcess();
                myProcess.PriorityClass = ProcessPriorityClass.High;
                CreateHostBuilder(args).Build().Run();
                Logger.Log.Info("Application successfully started");
            }
            catch (Exception error)
            {
                Logger.Log.Error("Program Main method error: " + error.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(baseAddress);
                });
    }
}