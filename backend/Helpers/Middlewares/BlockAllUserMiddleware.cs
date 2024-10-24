using backend.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection; // For creating scope
using System.Threading.Tasks;

public class BlockAllUserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory; // Inject the scope factory

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

            // Log or process the result
            if (result != null && result.Events?.Any() == true)
            {
                var eventId = result.Events.FirstOrDefault()?.eventId;
                Console.WriteLine($"Blocked event with eventId: {eventId}");
            }
            else 
            {
                Console.WriteLine("No eventId found in the request.");
            }
            if (result != null && result.Users?.Any() == true)
            {
                var userId = result.Users.FirstOrDefault()?.userId;
                Console.WriteLine($"Blocked user with userId: {userId}");
            }
            else
            {
                Console.WriteLine("No userId found in the request.");
            }

            if (result != null && result.Attendee?.Any() == true)
            {
                var Email = result.Attendee.FirstOrDefault()?.Email;
                Console.WriteLine($"Blocked Attendee with email: {Email}");
            }
            else
            {
                Console.WriteLine("No Attendee   found in the request.");
            }
        }

        // Proceed to the next middleware
        await _next(context);
    }
}
