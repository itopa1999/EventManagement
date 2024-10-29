using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Models;

namespace backend.Mappers
{
    public static class AttendeeMapper
    {
        public static AttendeeTicketListDto ToAttendeeTicketListDto(this Ticket ticket){
            return new AttendeeTicketListDto{
                Id = ticket.Id,
                TicketType = ticket.TicketType.ToString(),
                Price = ticket.Price,
                IsCheckedIn = ticket.IsCheckedIn,
                CheckedInAt = ticket.CheckedInAt,
                Payments = ticket.Payments.Select(x=>x.ToAttendeeTicketPaymentDto()).ToList()
            };
        }

        public static AttendeeTicketPaymentDto ToAttendeeTicketPaymentDto(this Payment payment){
            return new AttendeeTicketPaymentDto{
                Id = payment.Id,
                Method = payment.Method.ToString(),
                Amount = payment.Amount,
                Status = payment.Status.ToString(),
                TransactionId = payment.TransactionId,
                TicketId = payment.TicketId,
                PaymentDate = payment.PaymentDate
            };
        }


        public static AttendeeEventsListDto ToAttendeeEventsListDto(this Event listEvent){
            return new AttendeeEventsListDto{
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


         public static AttendeeSessionListDto ToAttendeeSessionListDto(this Session session){
            return new AttendeeSessionListDto{
                Id = session.Id,
                Title = session.Title,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Speaker = session.Speaker,

            };
         }

         public static AttendeeFeedbackListDto ToAttendeeFeedbackListDto(this Feedback feedback){
            return new AttendeeFeedbackListDto {
                Id = feedback.Id,
                AttendeeEmail = feedback.AttendeeEmail,
                Comments = feedback.Comments,
                Rating = feedback.Rating,
                SubmittedAt = feedback.SubmittedAt,

            };
        }

        public static AttendeeEventDetailsDto ToAttendeeEventDetailsDto(this Event listEvent){
            return new AttendeeEventDetailsDto{
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
                Sessions = listEvent.Sessions.Select(x=>x.ToAttendeeSessionListDto()).ToList(),
                Feedbacks = listEvent.Feedbacks.Select(x=>x.ToAttendeeFeedbackListDto()).ToList()
            };
        }




    }
}