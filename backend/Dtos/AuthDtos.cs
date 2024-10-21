
using System.Text.Json.Serialization;
using backend.Helpers;
using Newtonsoft.Json;

namespace backend.Dtos
{
    public class CreateAdminDto
    {
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        [JsonPropertyName("email_address")]
        public string? Email { get; set; }
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }
        [JsonPropertyName("other_name")]
        public string? OtherName { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("lga")]
        public string? LGA { get; set; }
        [JsonPropertyName("address")]
        public string? Address { get; set; }
        [JsonPropertyName("gender")]
        public string? Gender { get; set; }
        [JsonPropertyName("security_question")]
        public string? SecurityQuestion { get; set; }
        [JsonPropertyName("security_answer")]
        public string? SecurityAnswer { get; set; }
        [JsonPropertyName("user_type")]
        public UserType UserType { get; set; } 
        [JsonPropertyName("password")]
        public string? Password { get; set; }

    }

    public class VerifyOtpDto
    {
        public string? Id { get; set; }
        public int Token { get; set; }

    }


}