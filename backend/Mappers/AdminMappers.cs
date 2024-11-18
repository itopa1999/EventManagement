using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Models;

namespace backend.Mappers
{
    public static class AdminMappers
    {
        public static AdminListOrganizerDto ToAdminListOrganizerDto(this User user){
            return new AdminListOrganizerDto{
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType.ToString(),
            };
        }


        public static AdminOrganizerEventListDto ToAdminOrganizerEventListDto(this Event listEvent, HttpRequest request){
            return new AdminOrganizerEventListDto{
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



        public static AdminOrganizerDetailsDto ToAdminOrganizerDetailsDto(this User user, HttpRequest request){
            return new AdminOrganizerDetailsDto{
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OtherName = user.OtherName,
                State = user.State,
                LGA = user.LGA,
                Address = user.Address,
                Gender = user.Gender,
                CreatedAt = user.CreatedAt,
                IsBlock = user.IsBlock,
                UserType = user.UserType.ToString(),
                Event = user.Events?.Select(x=>x.ToAdminOrganizerEventListDto(request)).ToList()
            };
        }

        public static AdminEventReminderListDto ToAdminEventReminderListDto(this Reminder reminder){
            return new AdminEventReminderListDto{
                Id = reminder.Id,
                ReminderTime = reminder.ReminderTime,
                Type = reminder.Type.ToString(),
                HasSent = reminder.HasSent

            };
         }


         public static AdminEventSessionListDto ToAdminEventReminderListDto(this Session session){
            return new AdminEventSessionListDto{
                Id = session.Id,
                Title = session.Title,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Speaker = session.Speaker,

            };
         }

        public static AdminEventDetailsDto ToAdminEventDetailsDto(this Event listEvent, HttpRequest request){
            return new AdminEventDetailsDto{
                Id = listEvent.Id,
                OrganizerId = listEvent.OrganizerId,
                OrganizerFName = listEvent.Organizer.FirstName,
                OrganizerLName = listEvent.Organizer.LastName,
                OrganizerUName = listEvent.Organizer.UserName,
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
                IsBlock = listEvent.IsBlock,
                ImagePath = $"{request.Scheme}://{request.Host}{listEvent.ImagePath}",
                Sessions = listEvent.Sessions.Select(x=>x.ToAdminEventReminderListDto()).ToList(),
                Reminders = listEvent.Reminders.Select(x=>x.ToAdminEventReminderListDto()).ToList()
            };
        }


        public static AdminEventInvitationListDto ToAdminEventInvitationListDto(this Invitation invitation){
            return new AdminEventInvitationListDto {
                Id = invitation.Id,
                AttendeeEmail = invitation.AttendeeEmail,
                SentAt = invitation.SentAt,
                Status = invitation.Status.ToString()

            };
        }


        public static AdminEventAttendeeListDto ToAdminEventAttendeeListDto(this Attendee attendee){
            return new AdminEventAttendeeListDto {
                Id = attendee.Id,
                FirstName = attendee.FirstName,
                LastName = attendee.LastName,
                Email = attendee.Email,
                PhoneNumber = attendee.PhoneNumber,
                RegisteredAt = attendee.RegisteredAt

            };
        }


        public static AdminEventTicketListDto ToAdminEventTicketListDto(this Ticket ticket){
            return new AdminEventTicketListDto {
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


        public static AdminEventPaymentListDto ToAdminEventPaymentListDto(this Payment payment){
            return new AdminEventPaymentListDto {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Method = payment.Method.ToString(),
                Status = payment.Status.ToString(),
                TicketId = payment.TicketId,
                TransactionId = payment.TransactionId,

            };
        }


        public static AdminEventFeedbackListDto ToAdminEventFeedbackListDto(this Feedback feedback){
            return new AdminEventFeedbackListDto {
                Id = feedback.Id,
                AttendeeEmail = feedback.AttendeeEmail,
                Comments = feedback.Comments,
                Rating = feedback.Rating,
                SubmittedAt = feedback.SubmittedAt,

            };
        }









    }
}