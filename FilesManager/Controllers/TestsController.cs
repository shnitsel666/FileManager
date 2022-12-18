namespace FilesManager.Controllers
{
    using FilesManager.Models.ApiModels;
    using FilesManager.Models.Infrastructure;
    using FilesManager.Services.Application.TestService;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class TestsController : Controller
    {
        private ITestService _testService;

        #region .ctor
        public TestsController(ITestService testService)
        {
            _testService = testService;
        }
        #endregion

        #region IsFileOpened()

        /// <summary>
        /// Checks if file is opened.
        /// </summary>
        /// <param name="downloadRequest">Downloaded file information.</param>
        /// <returns>Always return HTTP code 200 with custom error codes.</returns>
        /// <response code="200">
        /// <para>If operation will be performed successfully, connection will be established.</para>
        /// <para>If operation won't be performed successfully, error message and code will be returned.</para>
        /// <para>Errors codes:</para>
        /// <para>-1 - unhandled exception.</para>
        /// <para>0 - success operation.</para>
        /// </response>
        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public IActionResult IsFileOpened(DownloadRequest downloadRequest) =>
            Ok(_testService.IsFileOpened(downloadRequest));
        #endregion

        #region IsFileExists()

        /// <summary>
        /// Checks if file exists.
        /// </summary>
        /// <param name="downloadRequest">Downloaded file information.</param>
        /// <returns>Always return HTTP code 200 with custom error codes.</returns>
        /// <response code="200">
        /// <para>If operation will be performed successfully, connection will be established.</para>
        /// <para>If operation won't be performed successfully, error message and code will be returned.</para>
        /// <para>Errors codes:</para>
        /// <para>-1 - unhandled exception.</para>
        /// <para>0 - success operation.</para>
        /// </response>
        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public IActionResult IsFileExists(DownloadRequest downloadRequest) =>
            Ok(_testService.IsFileExists(downloadRequest));
        #endregion

        #region IsFileReadonly()

        /// <summary>
        /// Checks if file has readonly attribute.
        /// </summary>
        /// <param name="downloadRequest">Downloaded file information.</param>
        /// <returns>Always return HTTP code 200 with custom error codes.</returns>
        /// <response code="200">
        /// <para>If operation will be performed successfully, connection will be established.</para>
        /// <para>If operation won't be performed successfully, error message and code will be returned.</para>
        /// <para>Errors codes:</para>
        /// <para>-1 - unhandled exception.</para>
        /// <para>0 - success operation.</para>
        /// </response>
        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public IActionResult IsFileReadonly(DownloadRequest downloadRequest) =>
            Ok(_testService.IsFileOpened(downloadRequest));
        #endregion

        #region IsFileNotTracking()

        /// <summary>
        /// Checks if file exists in downloads history.
        /// </summary>
        /// <param name="downloadRequest">Downloaded file information.</param>
        /// <returns>Always return HTTP code 200 with custom error codes.</returns>
        /// <response code="200">
        /// <para>If operation will be performed successfully, connection will be established.</para>
        /// <para>If operation won't be performed successfully, error message and code will be returned.</para>
        /// <para>Errors codes:</para>
        /// <para>-1 - unhandled exception.</para>
        /// <para>0 - success operation.</para>
        /// </response>
        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public IActionResult IsFileNotTracking(DownloadRequest downloadRequest) =>
            Ok(_testService.IsFileNotTracking(downloadRequest));
        #endregion
    }
}