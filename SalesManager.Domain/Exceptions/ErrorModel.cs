using System;
using System.Collections.Generic;
using System.Net;

namespace Vizage.Infrastructure.Exceptions.Models
{
    /// <summary>
    /// the exception response model
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Gets or sets the error code should implement the business model dictionary
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the response status code
        /// </summary>
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;

        /// <summary>
        /// Gets or sets the exception type normally it will be nameof(YourCustomException)
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets exception message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets error list
        /// </summary>
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

        /// <summary>
        /// Gets or sets inner exception
        /// </summary>
        public Exception InnerException { get; set; }

        /// <summary>
        /// Gets or sets stackTrace
        /// </summary>
        public string StackTrace { get; set; }
    }
}
