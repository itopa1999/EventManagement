using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backend.Helpers
{
    public class MessageResponse
{
    [JsonPropertyName("message")]
    [JsonProperty("message")]
    public string Message { get; set; }

    public MessageResponse(string message)
    {
        Message = string.IsNullOrEmpty(message) ? "success" : message;
    }
    }
}