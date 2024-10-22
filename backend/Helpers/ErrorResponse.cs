using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace backend.Helpers
{
    public class ErrorResponse
    {
        [JsonProperty("error_description")]
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; } = "successful";

        public static ErrorResponse GetModelStateErrors(ValueEnumerable errors)
        {
            var message = string.Join(" | ", errors
                               .SelectMany(v => v.Errors)
                               .Select(e => e.ErrorMessage));
            return new ErrorResponse { ErrorDescription = !string.IsNullOrEmpty(message) ? message : "Fill required values" };
        }

        public static ErrorResponse CustomError(string message)
        {
            return new ErrorResponse { ErrorDescription = message };
        }

        
    }
}