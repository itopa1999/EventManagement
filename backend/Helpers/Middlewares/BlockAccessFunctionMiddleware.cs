using System.Security.Claims;
using System.Text.Json;
using backend.Dtos;
using Newtonsoft.Json;

namespace backend.Helpers
{
    public class BlockAccessFunctionMiddleware
    {
        public static async Task<BlockAccessDto> BlockAccessDtoAsync(HttpContext context)
        {
            var blockAccessDto = new BlockAccessDto();

            int? eventId = null;
            string? Email = null;
            context.Request.EnableBuffering();

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
            if (eventId == null && context.Request.HasFormContentType && context.Request.Form.TryGetValue("eventId", out var formValue))
            {
                if (int.TryParse(formValue.ToString(), out int parsedEventId))
                {
                    eventId = parsedEventId;
                }
            }
            if (eventId.HasValue)
            {
                blockAccessDto.Events.Add(new EventAccessDto { eventId = eventId });
            }
            // else
            // {
            //     blockAccessDto.Events.Add(new EventAccessDto { eventId = 0 });
            // }
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                blockAccessDto.Users.Add(new OrganizerAccessDto { userId = userId });
            }
            // else
            // {
            //     blockAccessDto.Users.Add(new OrganizerAccessDto { userId = "frsaer-453434" });
            // }

            // Check for Email in route values
            if (context.Request.RouteValues.ContainsKey("Email"))
            {
                Email = context.Request.RouteValues["Email"]?.ToString();
            }

            // Check for Email in form data
            if (context.Request.HasFormContentType && context.Request.Form.ContainsKey("Email"))
            {
                Email = context.Request.Form["Email"];
            }

            // Check for Email in JSON body if not found in route values or form
            if (string.IsNullOrEmpty(Email))
            {
                using (var reader = new StreamReader(context.Request.Body, leaveOpen: true)) // Set leaveOpen to true
                {
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; // Reset body position after reading

                    if (!string.IsNullOrEmpty(body))
                    {
                        try
                        {
                            var json = JsonDocument.Parse(body);
                            if (json.RootElement.TryGetProperty("Email", out var emailElement))
                            {
                                Email = emailElement.GetString();
                            }
                        }
                        catch (JsonReaderException ex)
                        {
                            Console.WriteLine($"error {ex.Message}");
                        }
                    }
                }
            }

            // If Email has a value, add it to the Attendee list
            if (!string.IsNullOrEmpty(Email))
            {
                blockAccessDto.Attendee.Add(new AttendeeAccessDto { Email = Email });
            }
            // else
            // {
            //     blockAccessDto.Attendee.Add(new AttendeeAccessDto { Email = "default@example.com" });
            // }
            return blockAccessDto;
        }
    }
}