using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System.Net;
using MerosWebApi.Application.Common.SecurityHelpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Web;
using Asp.Versioning;
using FluentValidation.Results;
using MerosWebApi.Application.Common.DTOs;
using MongoDB.Bson;

namespace MerosWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IAuthHelper _authHelper;

        public UserController(IUserService userService, IAuthHelper authHelper)
        {
            _userService = userService;

            _authHelper = authHelper;
        }

        /// <summary>
        /// Authenticates the user
        /// </summary>
        /// <param name="reqDto">The request data</param>
        /// <returns>The result containing user info and authorization token, if authentication was successful
        /// </returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ActionName(nameof(AuthenticateAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AuthenticationResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AuthenticationResDto>> AuthenticateAsync(
            [FromBody] AuthenticateReqDto reqDto)
        {
            try
            {
                var authecateResult = await _userService.AuthenticateAsync(reqDto);

                return Ok(authecateResult);
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Gets detailed information about the user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        [ActionName(nameof(GetDetailsAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetDetailsResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GetDetailsResDto>> GetDetailsAsync(string id)
        {
            try
            {
                return Ok(await _userService.GetDetailsAsync(id));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new MyResponseMessage { Message = ex.Message });
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        /// <param name="token">Refresh token string</param>
        /// <returns>Access token string</returns>
        [AllowAnonymous]
        [HttpGet("refresh-token")]
        [ActionName(nameof(RefreshToken))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> RefreshToken(string token)
        {
            try
            {
                var newToken = await _userService.RefreshAccessToken(token);

                return Ok(newToken);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new MyResponseMessage { Message = ex.Message });
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="dto">Register DTO</param>
        /// <returns>GetDetailsResDto information about user</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ActionName(nameof(RegisterAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetDetailsResDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterReqDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);

                return CreatedAtAction(nameof(GetDetailsAsync), new { id = user.Id }, user);
            }
            catch (EmailNotSentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MyResponseMessage { Message = ex.Message });
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Confirm user email after registration or email update
        /// </summary>
        /// <param name="code">Confirm email code</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("confirm-email")]
        [ActionName(nameof(ConfirmEmailAsync))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ConfirmEmailAsync(string code)
        {
            try
            {
                await _userService.ConfirmEmailAsync(code);
                return NoContent();
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage { Message = ex.Message });
            }
        }
        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteAsync))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id, _authHelper.GetUserId(this));
                if (result == true)
                    return NoContent();

                return NotFound("User not found.");
            }
            catch (ForbiddenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new MyResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="dto">DTO with update information</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("{id}")]
        [ActionName(nameof(UpdateAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetDetailsResDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateReqDto dto)
        {
            try
            {
                var user = await _userService.UpdateAsync(id, _authHelper.GetUserId(this), dto);
                return CreatedAtAction(nameof(GetDetailsAsync), new { id = user.Id }, user);
            }
            catch (ForbiddenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new MyResponseMessage { Message = ex.Message });
            }
            catch (EmailNotSentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MyResponseMessage { Message = ex.Message });
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Reset user password and send reset password code to user email
        /// </summary>
        /// <param name="dto">DTO with user Email</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("password-reset")]
        [ActionName(nameof(PasswordResetAsync))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> PasswordResetAsync([FromBody] PasswordResetDto dto)
        {
            try
            {
                await _userService.PasswordResetAsync(dto);
                return NoContent();
            }
            catch (EmailNotSentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MyResponseMessage { Message = ex.Message });
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Confirm reset password using reset password code from email
        /// </summary>
        /// <param name="query">reset password code</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("confirm-password-reset")]
        [ActionName(nameof(ConfirmPasswordResetAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConfirmResetPswdDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ConfirmPasswordResetAsync([FromQuery] ConfirmResetPasswordQuery query)
        {
            try
            {
                return Ok(await _userService.ConfirmResetPasswordAsync(query.Code,
                    HttpUtility.UrlDecode(query.Email)));
            }
            catch (AppException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        #region Private Helpers Methods

        private void SetRefreshTokenToCookie(RefreshToken token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = token.Expires
            };

            Response.Cookies.Append("refreshToken", token.Token, cookieOptions);
        }

        #endregion
    }
}
