using backend.Dtos;
using backend.Interface;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace backend.AdminRepository
{
    public class AdminRepository : IAdminInterface
    {

        public async Task<BlockAccessDto> BlockAccessDtoAsync(HttpContext context)
        {
            var blockAccessDto = new BlockAccessDto();

            int? eventId = null;
            string? Email = null;

            if (context.Request.RouteValues.ContainsKey("eventId"))
            {
                if (int.TryParse(context.Request.RouteValues["eventId"].ToString(), out int parsedEventId))
                {
                    eventId = parsedEventId;
                }
            }
            if (eventId == null && context.Request.Query.ContainsKey("eventId"))
            {
                if (int.TryParse(context.Request.Query["eventId"], out int parsedEventId))
                {
                    eventId = parsedEventId;
                }
            }
            if (eventId == null && context.Request.HasFormContentType && context.Request.Form.ContainsKey("eventId"))
            {
                if (int.TryParse(context.Request.Form["eventId"], out int parsedEventId))
                {
                    eventId = parsedEventId;
                }
            }
            if (eventId.HasValue)
            {
                blockAccessDto.Events.Add(new EventAccessDto { eventId = eventId });
            }
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            blockAccessDto.Users.Add(new OrganizerAccessDto { userId = userId });


            if (context.Request.RouteValues.ContainsKey("Email"))
            {
                Email = context.Request.RouteValues["Email"]?.ToString();
            }

            // if (context.Request.ContentType == "application/json")
            // {
            //     using (var reader = new StreamReader(context.Request.Body))
            //     {
            //         var body = await reader.ReadToEndAsync();
            //         context.Request.Body.Position = 0; // Reset the stream position for further use

            //         // Deserialize the JSON into a dynamic object
            //         var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            //         // Attempt to extract the Email
            //         if (jsonData != null && jsonData.TryGetValue("Email", out var emailValue))
            //             Email = emailValue.ToString(); // Convert to string
            //             blockAccessDto.Attendee.Add(new AttendeeAccessDto { Email = Email });
                    
            //     }
            // }
                

            return blockAccessDto;
        }
    }
}
