using backend.Helpers;
using backend.Models;

namespace backend.Dtos
{
    public class AttendeeRegisterDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }


    public class AttendeeBuyTicketDto
    {
        public string? Email { get; set; }
    }

    public class FlutterwaveResponseDto
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public Data? Data { get; set; }

    }

    public class Data
    {
        public Customer? Customer { get; set; }
    }

    public class Customer
    {
        public string? Name { get; set; }
        public string? Phone_number { get; set; }
        public string? Email { get; set; }
        public DateTime Created_at { get; set; }

    }

    public class ReBuyTicketDto
    {
        public string? Email { get; set; }
        public string? TransactionId { get; set; }
    }


    public class AttendeeTicketPaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Method { get; set; }
        public string? Status { get; set; }
        public int TicketId { get; set; }
        public string? TransactionId { get; set; } 
    }


    public class AttendeeTicketListDto
    {
        public int Id { get; set; }
        public string? TicketType { get; set; }
        public decimal Price { get; set; }
        public bool IsCheckedIn { get; set; }
        public DateTime CheckedInAt { get; set; }
        public List<AttendeeTicketPaymentDto>? Payments {get; set;} = new List<AttendeeTicketPaymentDto>();
    }

    public class AttendeeEmailDto
    {
        public string? Email { get; set; }
    }


    public class AttendeeEventsListDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
    }

    public class AttendeeSessionListDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Speaker { get; set; }
    }

    public class AttendeeFeedbackListDto
    {
        public int Id { get; set; }
        public string? AttendeeEmail { get; set; }
        public string? Comments { get; set; }
        public int Rating { get; set; }
        public DateTime SubmittedAt { get; set; }
    }



    public class AttendeeEventDetailsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsInvitationOnly  { get; set; }
        public bool HasPayment { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public List<AttendeeSessionListDto>? Sessions {get; set;} = new List<AttendeeSessionListDto>();
        public List<AttendeeFeedbackListDto>? Feedbacks {get; set;} = new List<AttendeeFeedbackListDto>();
    }

    public class AttendeeCreateFeedback
    {
        public string? Email { get; set; }
        public string? Comments { get; set; }
        public int Rating { get; set; }
    }





}











