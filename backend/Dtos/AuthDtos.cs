
using System.Text.Json.Serialization;
using backend.Helpers;
using Newtonsoft.Json;

namespace backend.Dtos
{
    public class CreateAdminDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? SecurityQuestion { get; set; }
        public string? SecurityAnswer { get; set; }
        public UserType UserType { get; set; } 
        public string? Password { get; set; }
        public string? AccessToken { get; set; }

    }

    public class VerifyOtpDto
    {
        public string? Id { get; set; }
        public int Token { get; set; }

    }

    public class ResendOtpDto
    {
        public string? Email { get; set; }

    }

    public class ForgotPasswordDto
    {
        public string? Email { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }

    }

    public class ResetPasswordDto
    {
        public string? Email { get; set; }
        public string? Password1 { get; set; }
        public string? Password2 { get; set; }
    }

    public class UserLoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

    }

    public class StateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string>? LGAs { get; set; }
    }


    public class SmtpSettingsDto
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string? FromEmail { get; set; }
        public string? FromPassword { get; set; }
    }



}