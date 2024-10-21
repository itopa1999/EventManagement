using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Models;
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

        public AuthController(
            DBContext context,
            UserManager<User> userManager,
            IAuthInterface authRepo

        )
        {
            _context = context;
            _userManager = userManager;
            _authRepo = authRepo;
        }


        [HttpPost("create/admin")]
        [ProducesResponseType(typeof(CreateAdminDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> CreateAdminEP([FromBody] CreateAdminDto adminDto)
        {

            if (!Enum.IsDefined(typeof(UserType), adminDto.UserType))
            {
                return BadRequest(ErrorResponse.CustomError("Invalid user type."));
            }

            try
            {
                var user = await _authRepo.CreateAdminUserAsync(adminDto);
                return StatusCode(201, new {message="Account has been created successfully"});
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An error occurred while creating an admin.");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { ErrorDescription = $"{ex}" });
            }
        }

        // [HttpPost("verify/otp/")]
        // [ProducesResponseType(typeof(VerifyOtpDto), (int)HttpStatusCode.OK)]
        // [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        // public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto){
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ErrorResponse.GetModelStateErrors(ModelState));
        //     }
            
        //     try
        //     {
        //         var result = _authRepo.VerifyOtpAsync(verifyOtpDto);
        //         return Ok(new { message = result });
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(new { error = ex });
        //     }

        // }








    }
}