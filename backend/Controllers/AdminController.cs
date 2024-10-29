using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Interface;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("admin/api/")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly DBContext _context;
        public readonly UserManager<User> _userManager;
        public readonly IAdminInterface _adminRepo;
        private readonly EmailService _emailSender;

        public AdminController(
            DBContext context,
            UserManager<User> userManager,
            IAdminInterface adminRepo,
            EmailService emailSender
        )
        {
            _context = context;
            _userManager = userManager;
            _adminRepo = adminRepo;
            _emailSender = emailSender;
        }

        [HttpGet("get/reminder")]
public async Task<IActionResult> GetReminder()
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

        await _context.SaveChangesAsync(); // Save changes to the database
        return Ok();
    }





        

 
















    }
}