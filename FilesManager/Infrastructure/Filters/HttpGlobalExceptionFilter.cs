namespace FilesManager.Infrastructure.Filters
{
    using System.Collections.Generic;
    using FilesManager.Infrastructure.Exceptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Hosting;

    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            Logger.Log.Error($"HttpGlobalExceptionFilter error: {context.Exception.Message} {context.Exception.StackTrace}");

            if (context.Exception.GetType() == typeof(ApiException))
            {
                var apiException = context.Exception as ApiException;
                var errorMessages = apiException.GetCustomErrorsMessage();

                if (errorMessages == null)
                {
                    errorMessages = new List<string> { "An error ocurred." }.AsReadOnly();
                }

                var result = new ErrorResponse
                {
                    Messages = errorMessages,
                };

                if (_env.IsDevelopment())
                {
                    result.DeveloperMessage = context.Exception.Message;
                    result.StackTrace = context.Exception.StackTrace;
                }

                context.Result = new ObjectResult(result);

                if (apiException.GetHttpStatusCode() > 0)
                {
                    context.HttpContext.Response.StatusCode = apiException.GetHttpStatusCode();
                }
                else
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                var result = new ErrorResponse
                {
                    Messages = new[] { "An error ocurred." },
                };

                if (_env.IsDevelopment())
                {
                    result.DeveloperMessage = context.Exception.Message;
                    result.StackTrace = context.Exception.StackTrace;
                }

                context.Result = new ObjectResult(result);
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                Logger.Log.Error($"HttpGlobalExceptionFilter error: {context.Exception.Message} {context.Exception.StackTrace}");
            }

            context.ExceptionHandled = true;
        }

        private class ErrorResponse
        {
            public IReadOnlyCollection<string> Messages { get; set; }

            public string DeveloperMessage { get; set; }

            public string StackTrace { get; set; }
        }
    }
}
