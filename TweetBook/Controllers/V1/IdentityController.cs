using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Response;
using TweetBook.Services.Identity;

namespace TweetBook.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegistrationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            var authResult = await _identityService.RegisterAsync(userRegistrationRequest.Email, userRegistrationRequest.Password);

            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResult.ErrorMessages
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token,
                RefreshToken = authResult.RefreshToken
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            var authResult = await _identityService.LoginAsync(userLoginRequest.Email, userLoginRequest.Password);

            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResult.ErrorMessages
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token,
                RefreshToken = authResult.RefreshToken
            });
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest userLoginRequest)
        {
            var authResult = await _identityService.RefreshTokenAsync(userLoginRequest.Token, userLoginRequest.RefreshToken);

            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResult.ErrorMessages
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token,
                RefreshToken = authResult.RefreshToken
            });
        }
    }
}
