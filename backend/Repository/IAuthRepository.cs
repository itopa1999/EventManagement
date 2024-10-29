using backend.Data;
using backend.Dtos;
using backend.Helpers;
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

        
        public async Task<(string message, User? user)> CreateAdminUserAsync(CreateAdminDto createAdmin)
        {
            var properties = typeof(CreateAdminDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(createAdmin);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            if (!StringHelpers.IsValidGender(createAdmin.Gender.Trim()))
                return ("Gender must be either 'Male' or 'Female'.", null);
            if (!Enum.IsDefined(typeof(UserType), createAdmin.UserType))
            {
                return ("Invalid user type.", null);
            }
            string formattedPhone = StringHelpers.FormatPhoneNumber(createAdmin.Phone.Trim());
            bool isValidPhone = StringHelpers.IsValidPhoneNumber(formattedPhone);
            if (!isValidPhone)
            {
                return (formattedPhone, null);
            }
            if (!StringHelpers.IsValidEmail(createAdmin.Email))
            {
                return ($"Invalid email {createAdmin.Email}", null);
            }
            var existingEmail = await _userManager.Users.FirstOrDefaultAsync(x=>x.Email == createAdmin.Email.ToString());
            if (existingEmail != null)
            {
                return ("Email is already taken.", null);
            }
            var user = new User 
            {
                UserName = createAdmin.Username,
                Email = createAdmin.Email,
                PhoneNumber = formattedPhone,
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
                return ($"Failed to create user: {errors}", null);
            }

            // Add role to the user
            var role = await _userManager.AddToRoleAsync(user, "Admin");
            if (!role.Succeeded)
            {
                var roleErrors = string.Join(", ", role.Errors.Select(e => e.Description));
                return ($"Failed to add Admin role: {roleErrors}", null);
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

            return ("User created successfully.", user);

        }

        public async Task<(string message, bool isSuccess)> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var properties = typeof(ForgotPasswordDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(forgotPasswordDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", false);
                    }
                }
            }

            if (!StringHelpers.IsValidEmail(forgotPasswordDto.Email))
            {
                return ($"Invalid email {forgotPasswordDto.Email}", false);
            }
            var userModel = await _userManager.Users.FirstOrDefaultAsync(x=> x.Email == forgotPasswordDto.Email);
            if (userModel == null){
                return ("User Not Found", false);
            }
            var existingOtp = await _context.Otps.FirstOrDefaultAsync(x=> x.UserId == userModel.Id
                        && x.Question == forgotPasswordDto.Question && x.Answer == forgotPasswordDto.Answer);
            if (existingOtp == null){
                return ("Answer is Incorrect", false);
            }else{
                existingOtp.Token = _token.GenerateToken();
                existingOtp.CreatedAt = TimeHelper.GetNigeriaTime();
                existingOtp.IsActive = true;
                await _context.SaveChangesAsync();
                Console.WriteLine($"Token is {existingOtp.Token}");
                return ($"Otp has been sent and here is your otp {existingOtp.Token}",true);
            }
        }

        public async Task<(string message, User? user)> LoginUserAsync(UserLoginDto loginDto)
        {
            var properties = typeof(UserLoginDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(loginDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x=> x.UserName == loginDto.Username);
            if (user == null){
                return ("incorrect credentials", null);
            }else{
                var loginUser = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!loginUser.Succeeded){
                    return ("incorrect credentials", null);
                }else{
                    return ("login successfully", user);
                }
            }
        }

        public async Task<(string message, bool isSuccess)> ResendOtpAsync(ResendOtpDto resendOtp)
        {
            if (string.IsNullOrEmpty(resendOtp.Email?.Trim()))
            return ("Email cannot be empty", false);
            if (!StringHelpers.IsValidEmail(resendOtp.Email))
            {
                return ($"Invalid email {resendOtp.Email}", false);
            }
            var userModel = await _userManager.Users.FirstOrDefaultAsync(x=> x.Email == resendOtp.Email);
            if (userModel == null){
                return ("User Not Found, make sure email has commence registration", false);
            }
            if (userModel.EmailConfirmed == true || userModel.PhoneNumberConfirmed == true){
                return ("User has been verified", false);
            }
            else{
                var otp = await _context.Otps.FirstOrDefaultAsync(x=>x.UserId==userModel.Id);
                otp.Token =  _token.GenerateToken();
                otp.IsActive = true;
                otp.CreatedAt = TimeHelper.GetNigeriaTime();
                await _context.SaveChangesAsync();
                Console.WriteLine($"token has been resend {otp.Token}");
                return ($"token has been successfully resend {otp.Token}", true);
            }


        }

        public async Task<(string message, bool isSuccess)> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var properties = typeof(ResetPasswordDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(resetPasswordDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", false);
                    }
                }
            }
            if (!StringHelpers.IsValidEmail(resetPasswordDto.Email))
            {
                return ($"Invalid email {resetPasswordDto.Email}", false);
            }
            var userModel = await _userManager.Users.FirstOrDefaultAsync(x=> x.Email == resetPasswordDto.Email);
            if (userModel == null){
                return ("User Not Found", false);
            }

            if (resetPasswordDto.Password1 != resetPasswordDto.Password2){
                return ("password mismatch", false);
            }
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(userModel);
            IdentityResult result = await _userManager.ResetPasswordAsync(userModel, resetToken, resetPasswordDto.Password1);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ($"Failed to change password: {errors}", false);
            }
            return ("Password changed successfully", true);
            
        }

        public async Task<(string message, bool isSuccess)> VerifyOtpAsync(VerifyOtpDto verifyOtp)
        {
            var properties = typeof(VerifyOtpDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(verifyOtp);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", false);
                    }
                }
            }
            if (!StringHelpers.IsValidPin(verifyOtp.Token.ToString())){
                return ("Token must be 6 digits", false);
            }
            var userModel = await _userManager.Users.FirstOrDefaultAsync(x=> x.Id == verifyOtp.Id);
            if (userModel == null){
                return ("User Not Found", false);
            }
            var getOtp = await _context.Otps.FirstOrDefaultAsync(x=>x.Token == verifyOtp.Token && x.UserId == userModel.Id);
            if (getOtp == null){
                return("Invalid OTP token.", false);
            }else if (getOtp.IsActive == false){
                return("OTP has already been used.", false);
            }else if (getOtp.CreatedAt.AddMinutes(10) <= TimeHelper.GetNigeriaTime() ){
                getOtp.IsActive = false;
                getOtp.CreatedAt = TimeHelper.GetNigeriaTime();
                await _context.SaveChangesAsync();
                return("token has expired", false);
            }
            else{
                getOtp.IsActive = false;
                getOtp.CreatedAt = TimeHelper.GetNigeriaTime();

                userModel.EmailConfirmed = true;
                userModel.PhoneNumberConfirmed = true;

                await _context.SaveChangesAsync();

                return ("OTP verified successfully.", true);          
            }
        }








    }
}