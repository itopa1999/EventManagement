using backend.Helpers;

namespace backend.Dtos
{
    public class OrganizerAccessDto
    {
        public string? userId { get; set; }
    }
    public class WalletAccessDto
    {
        public string? userId { get; set; }
    }
    public class EventAccessDto
    {
        public int? eventId { get; set; }
    }
    public class AttendeeAccessDto
    {
        public string? Email { get; set; }
    }

    public class BlockAccessDto
    {
        public List<OrganizerAccessDto>? Users {get; set;} = new List<OrganizerAccessDto>();
        public List<WalletAccessDto>? Wallets {get; set;} = new List<WalletAccessDto>();
        public List<EventAccessDto>? Events {get; set;} = new List<EventAccessDto>();
        public List<AttendeeAccessDto>? Attendee {get; set;} = new List<AttendeeAccessDto>();
    }


    public class CreateOrganizerDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? SecurityQuestion { get; set; }
        public string? SecurityAnswer { get; set; }
        public UserType UserType { get; set; } 
        public string? Password { get; set; }

    }

    public class AdminListOrganizerDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserType { get; set; }
        public bool IsBlock { get; set; }

    }

    public class AdminOrganizerDetailsDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? UserType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsBlock { get; set; }
        public List<AdminOrganizerEventListDto>? Event {get; set;} = new List<AdminOrganizerEventListDto>();

    }

    public class AdminOrganizerEventListDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public bool IsBlock { get; set; }
    }


    public class AdminEventSessionListDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Speaker { get; set; }
    }

    public class AdminEventReminderListDto
    {
        public int Id { get; set; }
        public DateTime ReminderTime { get; set; }
        public string? Type { get; set; }
        public bool HasSent { get; set; }
    }

    public class AdminEventDetailsDto
    {
        public int Id { get; set; }
        public string? OrganizerFName { get; set; }
        public string? OrganizerId { get; set; }
        public string? OrganizerLName { get; set; }
        public string? OrganizerUName { get; set; }
        public string? Name { get; set; }
        public string? EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsInvitationOnly  { get; set; }
        public bool HasPayment { get; set; } = false;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ImagePath { get; set; }
        public bool IsBlock { get; set; }
        public List<AdminEventSessionListDto>? Sessions {get; set;} = new List<AdminEventSessionListDto>();
        public List<AdminEventReminderListDto>? Reminders {get; set;} = new List<AdminEventReminderListDto>();
    }

    public class AdminEventInvitationListDto
    {
        public int Id { get; set; }
        public string? AttendeeEmail { get; set; }
        public DateTime SentAt { get; set; }
        public string? Status { get; set; }
    }

    public class AdminEventAttendeeListDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime RegisteredAt { get; set; }
    }


    public class AdminEventTicketListDto
    {
        public int Id { get; set; }
        public string? AttendeeFirstName { get; set; }
        public string? AttendeeLastName { get; set; }
        public string? AttendeeEmail { get; set; }
        public string? TicketType { get; set; }
        public decimal Price { get; set; }
        public bool IsCheckedIn { get; set; }
        public DateTime CheckedInAt { get; set; }
    }


    public class AdminEventPaymentListDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Method { get; set; }
        public string? Status { get; set; }
        public string? PayerEmail { get; set; }
        public int TicketId { get; set; }
        public string? TransactionId { get; set; } 

    }


    public class AdminEventFeedbackListDto
    {
        public int Id { get; set; }
        public string? AttendeeEmail { get; set; }
        public string? Comments { get; set; }
        public int Rating { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

    public class AdminDashboardDataDto
    {
        public int TotalEvent { get; set; }
        public int TotalSession { get; set; }
        public int TotalActiveEvent { get; set; }
        public int TotalActiveSession { get; set; }
        public int TotalOrganizer { get; set; }
        public int TotalAttendee { get; set; }
        public decimal TotalTicketSold { get; set; }
        public List<MonthlyDataDto>? MonthlyUserRegistrations { get; set; }
        public List<MonthlyDataDto>? MonthlyEventRegistrations { get; set; }
        public List<MonthlyDataDto>? MonthlySessionRegistrations { get; set; }
        
    }

    public class MonthlyDataDto
    {
        public string? Month { get; set; }
        public int Count { get; set; }
    }


    public class AdminListAttendeeDto
    {
        public int Id { get; set; }
         public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsBlock { get; set; }
    }


    public class AdminListOrganizersWalletDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public decimal Balance { get; set; }
        public bool IsBlock { get; set; }
    }



    public class AdminListTransactionDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Ref { get; set; }
        public string? Type { get; set; }
        public DateTime Date { get; set; }
    }










}