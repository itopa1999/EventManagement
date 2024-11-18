using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Helpers;
using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreatedAt { get; set; } = TimeHelper.GetNigeriaTime();
        public bool IsBlock { get; set; } = false;
        public ICollection<Event>? Events { get; set; }
        public string? OtpId { get; set; }
        public Otp? Otp { get; set; }
        
    }

    public class AccessToken
    {
        public int Id { get; set; }
        public string? AccessTok { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = TimeHelper.GetNigeriaTime();
    }


    public class Otp
    {
        public int Id { get; set; }
        public int Token { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public DateTime CreatedAt { get; set; } = TimeHelper.GetNigeriaTime();
        public bool IsActive { get; set; } = true;
        public string? UserId { get; set; }
        public User? User { get; set; }
    }


    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public EventType EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsInvitationOnly  { get; set; } = false;
        public bool HasPayment { get; set; } = false;
        public TicketType TicketType { get; set; } // ticket type
        public decimal Price { get; set; } = 0; // set to 0 if free.
        public string? OrganizerId { get; set; } // Foreign key to User
        public User? Organizer { get; set; } // Navigation property
        public int? TemplateId { get; set; } // Foreign key for EventTemplate
        public EventTemplate? Template { get; set; }
        public ICollection<Session>? Sessions { get; set; } = new List<Session>(); // Related sessions
        public ICollection<EventTemplate>? Templates { get; set; } =  new List<EventTemplate>(); // Related templates
        public ICollection<Attendee>? Attendees { get; set; } = new List<Attendee>(); // Related Attendees
        public ICollection<Invitation>? Invitations { get; set; } = new List<Invitation>();// Related Invitations
        public ICollection<Reminder>? Reminders { get; set; } = new List<Reminder>();// Related Reminders
        public ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();// Related Feedbacks
        public DateTime CreatedAt { get; set; } = TimeHelper.GetNigeriaTime();
        public bool IsBlock { get; set; } = false;
        public string? ImagePath { get; set; }
    }


    public class Session
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Speaker { get; set; }
        public int EventId { get; set; } // Foreign key to Event
        public Event? Event { get; set; } // Navigation property
        public DateTime CreatedAt { get; set; } = TimeHelper.GetNigeriaTime();
    }


    public class EventTemplate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public EventType EventType { get; set; }
        public string? Description { get; set; }
        public string? DefaultLocation { get; set; }
        public DateTime DefaultStartDate { get; set; }
        public DateTime DefaultEndDate { get; set; }
        public ICollection<Event>? Events { get; set; } = new List<Event>(); // Related events created from this template
    }



    public class Attendee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int EventId { get; set; } // Foreign key for the associated event
        public Event? Event { get; set; } // Navigation property to the Event
        public ICollection<Ticket>? Tickets { get; set; } = new List<Ticket>();// Related Tickets
        public string? QRCode { get; set; } // QR Code string for check-in
        public DateTime RegisteredAt { get; set; } = TimeHelper.GetNigeriaTime(); // Date and time of registration
        public bool IsBlock { get; set; } = false;
    }


    public class Ticket
    {
        public int Id { get; set; }
        public int? AttendeeId { get; set; } // Foreign key for the associated attendee
        public TicketType? TicketType { get; set; }
        public Attendee? Attendee { get; set; } // Navigation property to the Attendee
        public decimal Price { get; set; } = 0; // Price of the ticket (0 for free tickets)
        public bool IsCheckedIn { get; set; } = false; // Indicates if the attendee has checked in
        public ICollection<Payment>? Payments { get; set; } = new List<Payment>();// Related Payment
        public DateTime CheckedInAt { get; set; } // Date and time of check-in
    }


    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; } // Enum for payment methods
        public PaymentStatus Status { get; set; } // Enum for payment status
        public int TicketId { get; set; } // FK to Ticket if the payment is for a ticket
        public int EventId { get; set; } // FK to Event if the payment is for an event
        public string? TransactionId { get; set; } // External transaction ID from payment gateway

        
        public Ticket? Ticket { get; set; }
        public Event? Event { get; set; }
    }


    public class Invitation
    {
        public int Id { get; set; }
        public int EventId { get; set; } // FK to Event
        public string? AttendeeEmail { get; set; }
        public DateTime SentAt { get; set; }
        public InvitationStatus? Status { get; set; } // Enum to track invitation status
        
        // Navigation property
        public Event? Event { get; set; }
    }
 
    public class Reminder
    {
        public int Id { get; set; }
        public int EventId { get; set; } // FK to Event
        public DateTime ReminderTime { get; set; }
        public ReminderType Type { get; set; }
        public bool HasSent { get; set; } = false;
        // Navigation property
        public Event? Event { get; set; }
    }


    public class Feedback
    {
        public int Id { get; set; }
        public int EventId { get; set; } // FK to Event
        public string? AttendeeEmail { get; set; }
        public string? Comments { get; set; }
        public int Rating { get; set; } // Rating scale, e.g., 1-5
        public DateTime SubmittedAt { get; set; } = TimeHelper.GetNigeriaTime();

        // Navigation property
        public Event? Event { get; set; }
    }


    public class LoginAttempt
    {
        public int Id { get; set; } // Primary Key
        public string? UserId { get; set; } // Foreign key for the User
        public DateTime AttemptedAt { get; set; } = TimeHelper.GetNigeriaTime(); // Timestamp of the attempt
        public bool IsSuccessful { get; set; } // Indicates if the login was successful
        public string? IpAddress { get; set; } // IP address of the user attempting to log in

        // Navigation property
        public User? User { get; set; } // Navigation property to the User
    }


    public class Wallet
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public decimal Balance { get; set; } = 0;
        public User? User { get; set; }
        public bool IsBlock { get; set; } = false;
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public decimal Amount { get; set; } = 0;
        public string? Description { get; set; }
        public string? Ref { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }

    }

    public class UserDevice
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string? Client { get; set; }
        public string? OS { get; set; }
        public string? Device { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public DateTime LoginDate { get; set; } = TimeHelper.GetNigeriaTime();
    }









}