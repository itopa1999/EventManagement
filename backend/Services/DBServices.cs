using backend.Data;
using backend.Models;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public static class DBServices
    {

        // FOR ORGANIZER
     
        public static async Task<bool> IsValidEvent(this int id, string userId, DBContext context)
        {
            var existingEvent = await context.Events.FirstOrDefaultAsync(x => x.Id == id && x.OrganizerId == userId);
            return existingEvent != null;
        }

        public static async Task<bool> IsValidEventSession(this int id, int eventId, string userId, DBContext context)
        {
            var existingEventSession = await context.Sessions.FirstOrDefaultAsync(x => x.Id == id && x.Event.OrganizerId == userId && x.EventId == eventId);
            return existingEventSession != null;
        }

        public static async Task<bool> IsValidEventInvitation(this int id, int eventId, string userId, DBContext context)
        {
            var existingEventIv = await context.Invitations.FirstOrDefaultAsync(x => x.Id == id && x.Event.OrganizerId == userId && x.EventId == eventId);
            return existingEventIv != null;
        }

        public static async Task<bool> IsValidEventReminder(this int id, int eventId, string userId, DBContext context)
        {
            var existingEventReminder = await context.Reminders.FirstOrDefaultAsync(x => x.Id == id && x.Event.OrganizerId == userId && x.EventId == eventId);
            return existingEventReminder != null;
        }

        public static async Task<bool> IsValidOrganizer(this string id, UserManager<User> userManager)
        {
            var existingOrganizer = await userManager.FindByIdAsync(id);
            return existingOrganizer != null;
        }

        public static async Task<bool> DeviceDetails(this string Id, IHttpContextAccessor httpContextAccessor, DBContext _context)
        {
            var userAgent = httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString();
            var deviceDetector = new DeviceDetector(userAgent);
            deviceDetector.Parse();
            var deviceDetails = new UserDevice
            {
                UserId = Id,
                Client = deviceDetector.GetClient()?.ToString(),
                OS = deviceDetector.GetOs()?.ToString(),
                Device = deviceDetector.GetDeviceName(),
                Brand = deviceDetector.GetBrandName(),
                Model = deviceDetector.GetModel(),
            };
            var existingDevice = await _context.UserDevices
            .FirstOrDefaultAsync(d => d.UserId == Id && 
            d.Device == deviceDetails.Device &&
            d.Brand == deviceDetails.Brand &&
            d.Model == deviceDetails.Model);
            if (existingDevice == null)
            {
                await _context.UserDevices.AddAsync(deviceDetails);
                await _context.SaveChangesAsync();
            }

            await _context.UserDevices.AddRangeAsync(deviceDetails);
            await _context.SaveChangesAsync();

            return true;
        }


        // FOR ATTENDEE
        public static async Task<bool> IsValidEventRegistered(this string email, int eventId, DBContext context)
        {
            var existingEventAttendee = await context.Attendees.FirstOrDefaultAsync(x => x.Email == email && x.EventId == eventId);
            return existingEventAttendee != null;
        }

        public static async Task<bool> IsValidEventAttendee(this int id, DBContext context)
        {
            var existingEvent = await context.Events.FirstOrDefaultAsync(x => x.Id == id);
            return existingEvent != null;
        }



        // FOR ADMIN

        public static async Task<bool> IsValidEmailAdmin(this string email, DBContext context)
        {
            var existingEmail = await context.Attendees.FirstOrDefaultAsync(x => x.Email == email);
            return existingEmail != null;
        }

        public static async Task<bool> IsValidEventAdmin(this int id, DBContext context)
        {
            var existingEvent = await context.Events.FirstOrDefaultAsync(x => x.Id == id);
            return existingEvent != null;
        }



    }


    
}