using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos;
using backend.Interface;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class IAuthRepository : IAuthInterface
    {
        public readonly DBContext _context;
        public readonly UserManager<User> _userManager;
        public readonly SignInManager<User> _signInManager;
        public readonly IJWTService _token;
        public IAuthRepository(
            DBContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IJWTService token

        )
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _token = token;
        }

        
        public async Task<User> CreateAdminUserAsync(CreateAdminDto createAdmin)
        {
            var user = new User
            {
                UserName = createAdmin.Username,
                Email = createAdmin.Email,
                FirstName = createAdmin.FirstName,
                LastName = createAdmin.LastName,
                OtherName = createAdmin.OtherName,
                State = createAdmin.State,
                LGA = createAdmin.LGA,
                Address = createAdmin.Address,
                Gender = createAdmin.Gender,
                UserType = createAdmin.UserType
            };

            var userModel = await _userManager.CreateAsync(user, createAdmin.Password);
            if (!userModel.Succeeded)
            {
                var errors = string.Join(", ", userModel.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }

            // Add role to the user
            var role = await _userManager.AddToRoleAsync(user, "Admin");
            if (!role.Succeeded)
            {
                var roleErrors = string.Join(", ", role.Errors.Select(e => e.Description));
                throw new Exception($"Failed to add Admin role: {roleErrors}");
            }
            var otp = new Otp
            {
                UserId = user.Id,
                Question = createAdmin.SecurityQuestion,
                Answer = createAdmin.SecurityAnswer,
                Token = _token.GenerateToken()
            };
            await _context.Otps.AddAsync(otp);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Your otp code is {otp.Token}");

            return user;

        }

        // public async Task<string> VerifyOtpAsync(VerifyOtpDto verifyOtp)
        // {
        //     var userModel = await _userManager.Users.FirstOrDefaultAsync(x=> x.Id == verifyOtp.Id) 
        //         ?? throw new Exception($"User Not Found");
        //     var getOtp = await _context.Otps.FirstOrDefaultAsync(x=>x.Token == verifyOtp.Token && x.UserId == userModel.Id);
        //     if (getOtp == null){
        //         throw new Exception("Invalid OTP token.");
        //     }else if (getOtp.IsActive == false){
        //         throw new Exception("OTP has already been used.");
        //     }else if (getOtp.CreatedAt.AddMinutes(10) <= DateTime.Now ){
        //         getOtp.IsActive = false;
        //         getOtp.CreatedAt = DateTime.Now;
        //         await _context.SaveChangesAsync();
        //         throw new Exception("token has expired");
        //     }
        //     else{
        //         getOtp.IsActive = false;
        //         getOtp.CreatedAt = DateTime.Now;

        //         userModel.EmailConfirmed = true;
        //         userModel.PhoneNumberConfirmed = true;

        //         await _context.SaveChangesAsync();

        //         return "OTP verified successfully.";          
        //     }
        // }








    }
}