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
        Task<User> CreateAdminUserAsync (CreateAdminDto createAdmin);
        // Task<string> VerifyOtpAsync (VerifyOtpDto verifyOtp);
    }
}