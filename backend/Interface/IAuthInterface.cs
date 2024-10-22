using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Models;

namespace backend.Interface
{
    
    public interface IAuthInterface
    {
        Task<(string message, User? user)> CreateAdminUserAsync(CreateAdminDto createAdmin);
        Task<(string message, bool isSuccess)> VerifyOtpAsync (VerifyOtpDto verifyOtp);
        Task<(string message, bool isSuccess)> ResendOtpAsync (ResendOtpDto resendOtp);
        Task<(string message, bool isSuccess)> ForgotPasswordAsync (ForgotPasswordDto forgotPasswordDto);
        Task<(string message, bool isSuccess)> ResetPasswordAsync (ResetPasswordDto resetPasswordDto);
        Task<(string message, User? user)> LoginUserAsync (UserLoginDto loginDto);
    }
}   