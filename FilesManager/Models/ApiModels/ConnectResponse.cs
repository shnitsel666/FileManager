namespace FilesManager.Models.ApiModels
{
    using FilesManager.Constants;

    public class ConnectResponse
    {
        /// <summary>
        /// Current platform, Windows or Linux
        /// </summary>
        public Platforms Platform { get; set; }
    }
}
