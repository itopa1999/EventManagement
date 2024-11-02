using backend.Data;
using backend.Interface;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // For creating scope
using System.Threading.Tasks;

public class BlockAllUserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public BlockAllUserMiddleware(
        RequestDelegate next,
        IServiceScopeFactory scopeFactory
    )
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var adminRepo = scope.ServiceProvider.GetRequiredService<IAdminInterface>();

            var result = await adminRepo.BlockAccessDtoAsync(context);
            var _context = scope.ServiceProvider.GetRequiredService<DBContext>();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            // Log or process the result
            if (result != null && result.Events?.Any() == true)
            {
                var eventId = result.Events.FirstOrDefault()?.eventId;
                var eventInstance = await _context.Events.SingleOrDefaultAsync(e => e.Id == eventId);
                if (eventInstance != null && eventInstance.IsBlock)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new { errorCode = 4000, message = "This event is blocked." });
                    return;
                }
            }
            if (result != null && result.Users?.Any() == true)
            {
                var userId = result.Users.FirstOrDefault()?.userId;
                var userInstance = await _userManager.Users.SingleOrDefaultAsync(e => e.Id == userId);
                if (userInstance != null && userInstance.IsBlock)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new { errorCode = 4000, message = "This user is blocked." });
                    return;
                }
            }

            if (result != null && result.Attendee?.Any() == true)
            {
                var Email = result.Attendee.FirstOrDefault()?.Email;
                var attendeeInstance = await _context.Attendees.FirstOrDefaultAsync(e => e.Email == Email);
                if (attendeeInstance != null && attendeeInstance.IsBlock)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new { errorCode = 4000, message = "This Email is blocked." });
                    return;
                }
            }
        }
        await _next(context);
    }
}
