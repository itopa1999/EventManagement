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
                Status = invitation.Status.ToString()

            };
        }


        public static OrganizerAttendeeListDto ToOrganizerAttendeeListDto(this Attendee attendee){
            return new OrganizerAttendeeListDto {
                Id = attendee.Id,
                FirstName = attendee.FirstName,
                LastName = attendee.LastName,
                Email = attendee.Email,
                PhoneNumber = attendee.PhoneNumber,
                RegisteredAt = attendee.RegisteredAt

            };
        }


        public static OrganizerTicketListDto ToOrganizerTicketListDto(this Ticket ticket){
            return new OrganizerTicketListDto {
                Id = ticket.Id,
                AttendeeFirstName = ticket.Attendee?.FirstName,
                AttendeeLastName = ticket.Attendee?.LastName,
                AttendeeEmail = ticket.Attendee?.Email,
                TicketType = ticket.TicketType.ToString(),
                Price = ticket.Price,
                IsCheckedIn = ticket.IsCheckedIn,
                CheckedInAt = ticket.CheckedInAt

            };
        }


        public static OrganizerPaymentListDto ToOrganizerPaymentListDto(this Payment payment){
            return new OrganizerPaymentListDto {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Method = payment.Method.ToString(),
                Status = payment.Status.ToString(),
                TicketId = payment.TicketId,
                TransactionId = payment.TransactionId,

            };
        }


        public static OrganizerFeedbackListDto ToOrganizerFeedbackListDto(this Feedback feedback){
            return new OrganizerFeedbackListDto {
                Id = feedback.Id,
                AttendeeEmail = feedback.AttendeeEmail,
                Comments = feedback.Comments,
                Rating = feedback.Rating,
                SubmittedAt = feedback.SubmittedAt,

            };
        }

        
        public static organizerEventsDto ToOrganizerEventsDto(this Event listEvent, HttpRequest request){
            return new organizerEventsDto{
                Id = listEvent.Id,
                Name = listEvent.Name,
                EventType = listEvent.EventType.ToString(),
                State = listEvent.State,
                Location=listEvent.Location,
                StartDate = listEvent.StartDate,
                EndDate = listEvent.EndDate,
                Description = listEvent.Description,
                ImagePath = $"{request.Scheme}://{request.Host}{listEvent.ImagePath}"
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

        public static OrganizerEventDetailsDto ToOrganizerEventDetailsDto(this Event listEvent, HttpRequest request){
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
                Price = listEvent.Price,
                CreatedAt = listEvent.CreatedAt,
                ImagePath = $"{request.Scheme}://{request.Host}{listEvent.ImagePath}",
                Sessions = listEvent.Sessions.Select(x=>x.ToOrganizerReminderListDto()).ToList(),
                Reminders = listEvent.Reminders.Select(x=>x.ToOrganizerReminderListDto()).ToList()
            };
        }

        public static OrganizerTransactionsDto ToOrganizerTransactionsDto(this Transaction transaction){
            return new OrganizerTransactionsDto{
                Id = transaction.Id,
                Amount = transaction.Amount,
                Description = transaction.Description,
                Ref = transaction.Ref,
                Type = transaction.Type.ToString(),
                Date = transaction.Date
                

            };
        }
    










    }
}