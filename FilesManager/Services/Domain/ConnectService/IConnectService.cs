namespace FilesManager.Services.Domain.ConnectService
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;

    /// <summary>
    /// Service is responsible for connection between application and webclient and defines current platform (linux или windows).
    /// </summary>
    public interface IConnectService
    {
        /// <summary>
        /// Connect to webclient with environment details
        /// </summary>
        Response<ConnectResponse> Connect(string appVersion);
    }
}
