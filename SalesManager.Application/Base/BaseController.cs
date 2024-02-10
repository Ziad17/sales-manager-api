using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SalesManager.Application.Base
{
    public class BaseController : ControllerBase
    {
        [ApiController]
        [Produces(MediaTypeNames.Application.Json)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public class DefaultController : ControllerBase
        {
            private ICurrentUserService _currentUserService;
            private IMediator _mediator;
            private IHttpContextAccessor _contextAccessor;

            protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

            protected ICurrentUserService CurrentUserService =>
                _currentUserService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

            protected IHttpContextAccessor HttpContextAccessor =>
                _contextAccessor ??= HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
        }
    }
}
