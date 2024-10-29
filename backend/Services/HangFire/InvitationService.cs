using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Helpers;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.HangFire
{
    public class InvitationService
    {
    private readonly DBContext _context;
    private readonly EmailService _emailSender;

    public InvitationService(DBContext context, EmailService emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    public async Task CheckAndSendInvitationsAsync()
    {
        var currentTime = TimeHelper.GetNigeriaTime();
        var invitations = await _context.Invitations
        .Where(x=>x.SentAt.Hour == currentTime.Hour &&
        x.SentAt.Minute == currentTime.Minute &&
        x.Status == InvitationStatus.Pending)
        .ToListAsync();

        foreach (var invitation in invitations)
        {
            if (invitation.AttendeeEmail != null)
            {
                var email = invitation.AttendeeEmail;
                await _emailSender.SendEmailAsync(email, "Invitation Subject", "This is the Invitation body.");
            }

            _context.Invitations.Update(invitation);
        }

        await _context.SaveChangesAsync();
        
        BackgroundJob.Enqueue(() => CheckAndSendInvitationsAsync());

    }

    }
}