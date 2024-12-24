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
using MerosWebApi.Application.Common.DTOs.CommonDtos.CommonDtoValidators;
using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Application.Common.DTOs.UserService.ReqDtos;
using MongoDB.Bson;
using MerosWebApi.Application.Common.Exceptions.EmailExceptions;
using MerosWebApi.Application.Common.Exceptions.Common;

namespace MerosWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IAuthHelper _authHelper;

        private const string ACCESS_COOKIE_KEY = "authToken";

        private const string REFRESH_COOKIE_KEY = "mrsRFR";

        public UserController(IUserService userService, IAuthHelper authHelper)
        {
            _userService = userService;

            _authHelper = authHelper;
        }

        /// <summary>
        /// Login the user using AuthCode by sending by Email
        /// </summary>
        /// <param name="authCode">AuthCode</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("log-in")]
        [ActionName(nameof(LogInAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AuthenticationResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AuthenticationResDto>> LogInAsync(
            [FromBody] LogInReqDto logInReq)
        {
            try
            {
                var logInResult = await _userService.LogInAsync(logInReq.AuthCode);

                SetTokenToCookie(logInResult.RefreshToken, REFRESH_COOKIE_KEY);

                SetTokenToCookie(logInResult.AccessToken, ACCESS_COOKIE_KEY);

                return Ok(logInResult.AuthenticationResDto);
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage(ex.Message));
            }
        }

        /// <summary>
        /// Gets detailed information about the user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ActionName(nameof(GetDetailsAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetDetailsResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GetDetailsResDto>> GetDetailsAsync([MustBeObjectId] string id)
        {
            try
            {
                return Ok(await _userService.GetDetailsAsync(id));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new MyResponseMessage(ex.Message));
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage(ex.Message));
            }
        }

        /// <summary>
        /// Sends the authorization code to the user's email
        /// </summary>
        /// <param name="code">Confirm email code</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("send-authcode")]
        [ActionName(nameof(SendAuthCode))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SendAuthCode([FromBody] SendEmailReqDto email)
        {
            try
            {
                await _userService.SendUserUniqueInviteCode(email.Email);
                return NoContent();
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage(ex.Message));
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
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> RefreshToken()
        {
            try
            {
                Request.Cookies.TryGetValue(REFRESH_COOKIE_KEY, out var refreshToken);

                if (string.IsNullOrWhiteSpace(refreshToken))
                    return BadRequest(new MyResponseMessage("Refresh token в куки отсутсвует"));

                var accessToken = await _userService.RefreshAccessToken(refreshToken);
                SetTokenToCookie(accessToken, ACCESS_COOKIE_KEY);

                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new MyResponseMessage(ex.Message));
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new MyResponseMessage($"Произошла ошибка на сервере {ex.Message}"));
            }
        }

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteAsync))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> DeleteAsync([MustBeObjectId] string id)
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
                return StatusCode((int)HttpStatusCode.Forbidden, new MyResponseMessage(ex.Message));
            }
        }

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="dto">DTO with update information</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPatch("{id}")]
        [ActionName(nameof(UpdateAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetDetailsResDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([MustBeObjectId] string id, [FromBody] UpdateReqDto dto)
        {
            try
            {
                var user = await _userService.UpdateAsync(id, _authHelper.GetUserId(this), dto);
                return CreatedAtAction(nameof(GetDetailsAsync), new { id = user.Id }, user);
            }
            catch (ForbiddenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new MyResponseMessage(ex.Message));
            }
            catch (EmailNotSentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MyResponseMessage(ex.Message));
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage(ex.Message));
            }
        }

        /// <summary>
        /// Returns a UserStatistic representing the user's statistics
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("statistic")]
        [ActionName(nameof(GetDetailsAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserStatisticResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<UserStatisticResDto>> GetUserStatistic([MustBeObjectId] string userId)
        {
            try
            {
                var statictic = await _userService.GetUserStatisticAsync(userId);
                return Ok(statictic);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new MyResponseMessage(ex.Message));
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage(ex.Message));
            }
        }

        /// <summary>
        /// Confirm user email after registration or email update
        /// </summary>
        /// <param name="code">Confirm email code</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("confirm-email")]
        [ActionName(nameof(ConfirmEmailAsync))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailReqDto confirmEmailReq)
        {
            try
            {
                await _userService.ConfirmEmailAsync(confirmEmailReq.ConfirmEmailCode);
                return NoContent();
            }
            catch (AppException ex)
            {
                return BadRequest(new MyResponseMessage(ex.Message));
            }
        }

        #region Private Helpers Methods

        private void SetTokenToCookie(MyToken token, string key)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = token.Expires
            };

            Response.Cookies.Append(key, token.Token, cookieOptions);
        }

        #endregion
    }
}
