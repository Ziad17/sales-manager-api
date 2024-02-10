using System.Data.Common;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DateOnlyTimeOnly.AspNet.Converters;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SalesManager.Application;
using Vizage.Infrastructure.Exceptions;
using Vizage.Infrastructure.Exceptions.Models;

namespace Vizage.Infrastructure.Middleware.Exceptions
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ISerializationService _serializationService;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ISerializationService serializationService, ILogger<ExceptionMiddleware> logger)
        {
            _serializationService = serializationService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                if (e.GetBaseException() is DomainException)
                {
                    var exception = (DomainException)e;
                    var error = exception.Error;
                    error.ExceptionType = exception.ExceptionType;
                    error.Message = exception.Message;
                    error.ErrorCode = exception.Error.ErrorCode;
                    error.StackTrace = exception.StackTrace;
                    error.InnerException = exception.InnerException;

                    _logger.LogError(exception: e, message: "an exception of type {ExceptionName} has been raised", nameof(DomainException));

                    context.Response.StatusCode = exception.Error.StatusCode;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(_serializationService.Serialize(error, GetJsonSerializerOptions()), Encoding.UTF8);
                    await context.Response.CompleteAsync();
                }
                else if (e.GetBaseException() is DbException || e is DbException
                         || e.GetBaseException() is DbUpdateException || e is DbUpdateException)
                {
                    var error = new ErrorModel
                    {
                        Message = e.InnerException == null ? e.Message : e.InnerException.Message,
                        ExceptionType = e.GetType().FullName,
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };

                    _logger.LogError(exception: e, message: "an exception of type {ExceptionName} has been raised", nameof(DbUpdateException));

                    context!.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(_serializationService.Serialize(error, GetJsonSerializerOptions()), Encoding.UTF8);
                    await context.Response.CompleteAsync();
                }
                else if (e.GetBaseException() is ValidationException || e is ValidationException)
                {
                    var error = new ErrorModel();
                    var exception = (ValidationException)e;
                    error.Message = exception.Message;
                    error.ExceptionType = nameof(ValidationException);
                    error.ErrorCode = "0";
                    error.StackTrace = exception.StackTrace;
                    error.InnerException = exception.InnerException;

                    var errorProperties = exception.Errors.GroupBy(c => c.PropertyName);
                    foreach (var errorProperty in errorProperties)
                    {
                        var validationError = new ValidationError
                        {
                            PropertyName = errorProperty.Key
                        };
                        foreach (var validationFailure in errorProperty)
                        {
                            validationError.Validations.Add(new ErrorProperty(validationFailure.AttemptedValue,
                                ConvertToExceptionSuffix(validationFailure.ErrorCode), validationFailure.ErrorMessage,
                                validationFailure.Severity.ToString()));
                        }

                        error.Errors.Add(validationError);
                    }

                    _logger.LogError(exception: e, message: "an exception of type {ExceptionName} has been raised", nameof(ValidationException));

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(_serializationService.Serialize(error, GetJsonSerializerOptions()), Encoding.UTF8);
                    await context.Response.CompleteAsync();
                }
                else if (e.GetBaseException() is SecurityTokenException || e is SecurityTokenException)
                {
                    context!.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.CompleteAsync();
                }
                else
                {
                    var error = new ErrorModel
                    {
                        Message = e.Message,
                        ExceptionType = e.GetType().FullName,
                        StackTrace = e.StackTrace,
                        InnerException = e.InnerException
                    };

                    _logger.LogError(exception: e, message: "an exception of type {ExceptionName} has been raised", "UnhandledException");

                    error.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context!.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(_serializationService.Serialize(error, GetJsonSerializerOptions()), Encoding.UTF8);
                    await context.Response.CompleteAsync();
                }
            }
        }

        private string ConvertToExceptionSuffix(string validatorError)
        {
            return validatorError.Replace("Validator", "Exception");
        }

        private JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                WriteIndented = true,
                Converters =
                {
                    new TimeOnlyJsonConverter(),
                    new DateOnlyJsonConverter(),
                    new JsonStringEnumConverter()
                },
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            };
        }
    }
}
