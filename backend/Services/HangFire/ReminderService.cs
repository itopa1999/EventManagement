using backend.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class ReminderService
{
    private readonly DBContext _context;
    private readonly EmailService _emailSender;

    public ReminderService(DBContext context, EmailService emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    public async Task CheckAndSendRemindersAsync()
    {
        var currentTime = TimeHelper.GetNigeriaTime();
    var reminders = await _context.Reminders
        .Include(r => r.Event)
            .ThenInclude(e => e.Attendees)
        .Where(r => r.ReminderTime.Hour == currentTime.Hour &&
                 r.ReminderTime.Minute == currentTime.Minute &&
                 !r.HasSent)
        .ToListAsync();

    foreach (var reminder in reminders)
        {
            if (reminder.Event?.Attendees != null)
            {
                var attendeeEmails = reminder.Event.Attendees
                    .Select(attendee => attendee.Email)
                    .ToList();
                    foreach (var email in attendeeEmails)
                    {
                        await _emailSender.SendEmailAsync(email, "Reminder Subject", "This is the reminder body.");
                    }

                // Mark the reminder as sent
                reminder.HasSent = true;
                _context.Reminders.Update(reminder);
            }
        }

        await _context.SaveChangesAsync();
        BackgroundJob.Enqueue(() => CheckAndSendRemindersAsync());

    }
}
