namespace FilesManager.Controllers
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.FilesService;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class FilesManagerController : Controller
    {
        private IFilesService _filesService;

        #region .ctor
        public FilesManagerController(IFilesService filesService)
        {
            _filesService = filesService;
        }
        #endregion

        #region Download()

        /// <summary>
        /// Downloads file to local machine.
        /// </summary>
        /// <param name="downloadRequest">Download request.</param>
        /// <returns>Always return HTTP code 200 with custom error codes.</returns>
        /// <response code="200">
        /// <para>If operation will be performed successfully, information about downloaded file will be returned.</para>
        /// <para>If operation won't be performed successfully, error message and code will be returned.</para>
        /// <para>Errors codes:</para>
        /// <para>-1 - unhandled exception.</para>
        /// <para>0 - success operation.</para>
        /// </response>
        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Response<DownloadResponse>), StatusCodes.Status200OK)]
        public IActionResult Download(DownloadRequest downloadRequest) =>
            Ok(_filesService.Download(downloadRequest));
        #endregion

        #region Upload()

        /// <summary>
        /// Returns files to webclient.
        /// </summary>
        /// <param name="uploadRequest">Upload request.</param>
        /// <returns>Always return HTTP code 200 with custom error codes.</returns>
        /// <response code="200">
        /// <para>If operation will be performed successfully, file will be returned to webclient.</para>
        /// <para>If operation won't be performed successfully, error message and code will be returned.</para>
        /// <para>Errors codes:</para>
        /// <para>-1 - unhandled exception.</para>
        /// <para>0 - success operation.</para>
        /// </response>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Response<UploadResponse>), StatusCodes.Status200OK)]
        public IActionResult Upload(UploadRequest uploadRequest) =>
            Ok(_filesService.Upload(uploadRequest));
        #endregion

        #region Delete()

        /// <summary>
        /// Deletes file from local machine.
        /// </summary>
        /// <param name="deleteRequest">Delete request.</param>
        /// <returns>Always return HTTP code 200 with custom error codes.</returns>
        /// <response code="200">
        /// <para>If operation will be performed successfully, file will be deleted from local machine.</para>
        /// <para>If operation won't be performed successfully, error message and code will be returned.</para>
        /// <para>Errors codes:</para>
        /// <para>-1 - unhandled exception.</para>
        /// <para>0 - success operation.</para>
        /// </response>
        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public IActionResult Delete(DeleteRequest deleteRequest) =>
            Ok(_filesService.Delete(deleteRequest));
        #endregion

        #region Connect()

        /// <summary>
        /// Connect application with webclient.
        /// </summary>
        /// <param name="appVersion">App version.</param>
        /// <returns>Always return HTTP code 200 with custom error codes.</returns>
        /// <response code="200">
        /// <para>If operation will be performed successfully, connection will be established.</para>
        /// <para>If operation won't be performed successfully, error message and code will be returned.</para>
        /// <para>Errors codes:</para>
        /// <para>-1 - unhandled exception.</para>
        /// <para>0 - success operation.</para>
        /// </response>
        [HttpGet("[action]/appVersion={appVersion}")]
        [ProducesResponseType(typeof(Response<ConnectResponse>), StatusCodes.Status200OK)]
        public IActionResult Connect(string appVersion) =>
            Ok(_filesService.Connect(appVersion));
        #endregion
    }
}