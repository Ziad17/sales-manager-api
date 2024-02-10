namespace SalesManager.Domain.Exceptions
{
    /// <summary>
    /// the base domain exception
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// the ctor that's requires to define error message only
        /// </summary>
        /// <param name="message"></param>
        protected DomainException(string message)
            : base(message)
        {
            Message = message;
        }

        public DomainException(string message = "an exception raised", string code = "0000")
            : base(message)
        {
            ExceptionType = nameof(DomainException);
            Error.ExceptionType = nameof(DomainException);
            Error.Message = message;
            Error.ErrorCode = code;
        }

        /// <summary>
        /// the error model
        /// </summary>
        public ErrorModel Error { get; set; } = new ErrorModel();

        /// <summary>
        /// the exceptionType normally you can  define it as the nameof(YourExceptionName)
        /// </summary>
        public string ExceptionType { get; set; } = string.Empty;

        /// <summary>
        /// the error message it self, should filled with the object initialization
        /// </summary>
        public override string Message { get; }

        private void Initial(string parameterName, string message, object value, string errorCode, Severity severity)
        {
            SetError(parameterName, message, value, errorCode, severity);
        }

        private void SetError(string parameterName,
            string message,
            object value,
            string errorCode,
            Severity severity)
        {
            Error.Errors.Add(new ValidationError(parameterName, new List<ErrorProperty>
            {
                new ErrorProperty(value, errorCode, message, severity.ToString())
            }));
        }
    }
}
