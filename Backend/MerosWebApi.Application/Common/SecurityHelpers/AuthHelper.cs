using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MerosWebApi.Application.Common.SecurityHelpers
{
    public class AuthHelper : IAuthHelper
    {
        public string GetUserId(ControllerBase controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            var identity = controller.HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                throw new UnauthorizedException();

            var nameClaim = identity.FindFirst(ClaimTypes.Name);
            if (nameClaim == null) throw new ApplicationException("Cannot get claim 'Name'.");

            return nameClaim.Value;
        }
    }
}