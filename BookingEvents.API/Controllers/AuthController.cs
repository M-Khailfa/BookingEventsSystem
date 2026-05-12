using Azure;
using BookingEvents.Core.DTOs;
using BookingEvents.Core.Interfaces;
using BookingEvents.Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var response = new ApiResponse();
            if (!ModelState.IsValid)
            {
                response = ApiResponse.BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList());
                return BadRequest(response);
            }

            var result = await _authService.RegisterAsync(registerDto);
            if (!result.IsAuthenticated)
            {
                response = ApiResponse.BadRequest(new List<string> { result.Message });
                return BadRequest(response);
            }

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            }

            response = ApiResponse.Success(result.Email, result.Message, HttpStatusCode.Created);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = new ApiResponse();
            if (!ModelState.IsValid)
            {
                response = ApiResponse.BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList());
                return BadRequest(response);
            }

            var result = await _authService.LoginAsync(loginDto);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if(!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            }

            response = ApiResponse.Success(result, result.Message, HttpStatusCode.OK);
            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost("roles")]
        //public async Task<IActionResult> AddRole([FromBody] AddRoleDto addRoleDto)
        //{
        //    var response = new ApiResponse();
        //    if (!ModelState.IsValid)
        //    {
        //        response = ApiResponse.BadRequest(ModelState.Values
        //            .SelectMany(v => v.Errors)
        //            .Select(e => e.ErrorMessage)
        //            .ToList());
        //        return BadRequest(response);
        //    }
        //    var result = await _authService.AddRoleAsync(addRoleDto);
        //    if (result != "Done")
        //    {
        //        response = ApiResponse.BadRequest(result);
        //        return BadRequest(response);
        //    }
        //    response = ApiResponse.Success(result, "Role added successfully", HttpStatusCode.OK);
        //    return Ok(response);
        //}

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var response = new ApiResponse();
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                response = ApiResponse.BadRequest("No refresh token provided.");
                return BadRequest(response);
            }

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
            {
                response = ApiResponse.BadRequest(result.Message);
                return BadRequest(response);
            }

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            }

            response = ApiResponse.Success(result, "Token refreshed successfully", HttpStatusCode.OK);
            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto revokeTokenDto)
        {
            var response = new ApiResponse();
            var token = revokeTokenDto.token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
            {
                response = ApiResponse.BadRequest("No token provided.");
                return BadRequest(response);
            }
            var result = await _authService.RevokeTokenAsync(token);
            if (!result)
            {
                response = ApiResponse.BadRequest("Token revocation failed.");
                return BadRequest(response);
            }
            Response.Cookies.Delete("refreshToken");
            response = ApiResponse.Success(null, "Token revoked successfully.", HttpStatusCode.OK);
            return Ok(response);
        }


        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
