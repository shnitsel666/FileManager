namespace FilesManager.Infrastructure.Exceptions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Исключение связанное с работой Api.
    /// </summary>
    public class ApiException : Exception
    {
        private readonly int _statusCode = 0;
        private readonly IReadOnlyCollection<string> _customErrorsMessage = Array.Empty<string>();

        public ApiException()
        {
        }

        public ApiException(string message, int httpStatusCode = 0, IReadOnlyCollection<string> customErrorsMessage = null)
            : base(message)
        {
            _statusCode = httpStatusCode;
            _customErrorsMessage = customErrorsMessage;
        }

        public ApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public int GetHttpStatusCode() => _statusCode;

        /// <summary>
        /// Возвращает кастомное сообщение ошибки.
        /// </summary>
        public IReadOnlyCollection<string> GetCustomErrorsMessage() => _customErrorsMessage;

        public ApiException(string message)
            : base(message)
        {
        }
    }
}
