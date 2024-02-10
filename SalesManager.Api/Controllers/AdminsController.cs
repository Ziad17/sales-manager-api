using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManager.Application.Base;
using SalesManager.Application.Requests.Admins.Login;
using SalesManager.Application.Requests.Results;

namespace SalesManager.Api.Controllers
{
    /// <summary>
    /// manage admins operations
    /// </summary>
    /// <seealso cref="SalesManager.Application.Base.BaseController" />
    public class AdminsController : BaseController
    {
        /// <summary>
        /// admin login with username or email and password
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>new jwt response</returns>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Admins.Login)]
        public async Task<ActionResult<AccessTokenResult>> Login(AdminLoginCommand command, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

    }
}
