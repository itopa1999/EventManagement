using backend.Data;
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


    }


    
}