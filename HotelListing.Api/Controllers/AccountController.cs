﻿using HotelListing.Api.Contracts;
using HotelListing.Api.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAuthManager authManager,ILogger<AccountController> logger)
        {
            this._authManager = authManager;
            this._logger = logger;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            _logger.LogInformation($"Registration Attempt for {apiUserDto.Email}");
            var errors = await _authManager.Register(apiUserDto);
           
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }
            return Ok();

        }



        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation($"login attempt for {loginDto.Email} ");
            var authResponse = await _authManager.Login(loginDto);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return  Ok(authResponse);
        }


        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            var authResponse = await _authManager.VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }

    }
}
