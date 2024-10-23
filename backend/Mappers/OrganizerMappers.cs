using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Models;

namespace backend.Mappers
{
    public static class OrganizerMappers
    {

        public static OrganizerInvitationListDto ToOrganizerInvitationListDto(this Invitation invitation){
            return new OrganizerInvitationListDto {
                Id = invitation.Id,
                AttendeeEmail = invitation.AttendeeEmail,
                SentAt = invitation.SentAt,
                Status = invitation.Status

            };
        }

        
        public static organizerEventsDto ToOrganizerEventsDto(this Event listEvent){
            return new organizerEventsDto{
                Id = listEvent.Id,
                Name = listEvent.Name,
                EventType = listEvent.EventType.ToString(),
                State = listEvent.State,
                Location=listEvent.Location,
                StartDate = listEvent.StartDate,
                EndDate = listEvent.EndDate,
                Description = listEvent.Description
            };
        }


         public static OrganizerReminderListDto ToOrganizerReminderListDto(this Reminder reminder){
            return new OrganizerReminderListDto{
                Id = reminder.Id,
                ReminderTime = reminder.ReminderTime,
                Type = reminder.Type.ToString(),
                HasSent = reminder.HasSent

            };
         }


         public static OrganizerSessionListDto ToOrganizerReminderListDto(this Session session){
            return new OrganizerSessionListDto{
                Id = session.Id,
                Title = session.Title,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Speaker = session.Speaker,

            };
         }

        public static OrganizerEventDetailsDto ToOrganizerEventDetailsDto(this Event listEvent){
            return new OrganizerEventDetailsDto{
                Id = listEvent.Id,
                Name = listEvent.Name,
                EventType = listEvent.EventType.ToString(),
                State = listEvent.State,
                Location=listEvent.Location,
                StartDate = listEvent.StartDate,
                EndDate = listEvent.EndDate,
                Description = listEvent.Description,
                IsInvitationOnly = listEvent.IsInvitationOnly,
                HasPayment = listEvent.HasPayment,
                CreatedAt = listEvent.CreatedAt,
                Sessions = listEvent.Sessions.Select(x=>x.ToOrganizerReminderListDto()).ToList(),
                Reminders = listEvent.Reminders.Select(x=>x.ToOrganizerReminderListDto()).ToList()
            };
        }










    }
}