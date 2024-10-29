using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging;
using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Models;
using backend.Services;
using backend.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("auth/api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly DBContext _context;
        public readonly UserManager<User> _userManager;
        public readonly IAuthInterface _authRepo;
        public readonly IJWTService _token;
        private readonly StateLgaService _stateLgaService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            DBContext context,
            UserManager<User> userManager,
            IAuthInterface authRepo,
            IJWTService token,
            ILogger<AuthController> logger

        )
        {
            _context = context;
            _userManager = userManager;
            _authRepo = authRepo;
            _token = token;
            _stateLgaService = new StateLgaService();
            _logger = logger;
        }


        [HttpPost("create/admin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> CreateAdminEP([FromBody] CreateAdminDto adminDto)
        {
            try
            {
                var (message, user) = await _authRepo.CreateAdminUserAsync(adminDto);
                if (user == null) // Check if user creation failed
                {
                    _logger.LogError($"Create Admin: An error occurred: {message}");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }

                return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
            }catch (InvalidInputException ex)
            {
                _logger.LogError($"Create Admin: An error occurred: {ex.Message}");
                return BadRequest(new { error_description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create Admin: An error occurred: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { ErrorDescription = $"{ex}" });
            }
        }

        [HttpPost("verify/otp/")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto){
      
            var (message, isSuccess) = await _authRepo.VerifyOtpAsync(verifyOtpDto);
            if (isSuccess)
            {
                return Ok(new MessageResponse(message));
            }
            _logger.LogError($"Verify Otp: An error occurred: {message}");
            return BadRequest(new ErrorResponse { ErrorDescription = message });

        }

        [HttpPost("resend/otp")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto resendOtp){
            var (message, isSuccess) = await _authRepo.ResendOtpAsync(resendOtp);
            if (isSuccess)
            {
                return Ok(new MessageResponse(message));
            }
             _logger.LogError($"Resend Otp: An error occurred: {message}");
            return BadRequest(new ErrorResponse { ErrorDescription = message });
        }


        [HttpPost("forgot/password/")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto){
            var (message, isSuccess) = await _authRepo.ForgotPasswordAsync(forgotPasswordDto);
            if (isSuccess)
            {
                return Ok(new MessageResponse(message));
            }
             _logger.LogError($"Forgot Password: An error occurred: {message}");
            return BadRequest(new ErrorResponse { ErrorDescription = message });

        }

        [HttpPost("reset/password/")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto){
            var (message, isSuccess) = await _authRepo.ResetPasswordAsync(resetPasswordDto);
            if (isSuccess)
            {
                return Ok(new MessageResponse(message));
            }
             _logger.LogError($"Reset Password: An error occurred: {message}");
            return BadRequest(new ErrorResponse { ErrorDescription = message });
        }


        [HttpPost("user/login")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginDto loginDto){
            var (message, user) = await _authRepo.LoginUserAsync(loginDto);
            if (user != null)
            {
                return Ok(new {
                    message = message,
                    firstname = user.FirstName,
                    lastname = user.LastName,
                    userType = user.UserType,
                    username = user.UserName,
                    email = user.Email,
                    token = _token.CreateJwtTokenAsync(user),
                    });
            }
             _logger.LogError($"Login: An error occurred: {message}");
            return BadRequest(new ErrorResponse { ErrorDescription = message });
            
        }

        [HttpGet("list/states/")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public ActionResult<List<StateDto>> GetAllStates()
        {
            var states = _stateLgaService.GetAllStates();
            return Ok(states);
        }

        [HttpGet("list/states/{stateId}/lgas")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public ActionResult<List<string>> GetLgasByStateId(int stateId)
        {
            var lgas = _stateLgaService.GetLgasByStateId(stateId);
            if (lgas == null || !lgas.Any())
            {
                 _logger.LogError($"List State Lga: An error occurred: No Lgas found");
                return BadRequest(new ErrorResponse { ErrorDescription = $"No LGAs found" });
            }
            return Ok(lgas);
        }







    }
}