using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Helpers;

namespace backend.Dtos
{
    public class CreateEventDto
    {
        public string? Name { get; set; }
        public EventType EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsInvitationOnly  { get; set; } = false;
        public bool HasPayment { get; set; } = false;
    }

    public class organizerEventsDto
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


    public class CreateEventSessionDto
    {
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Speaker { get; set; }
        
    }


    public class UpdateEventSessionDto
    {
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Speaker { get; set; }
        
    }

    public class UpdateEventIvDto
    {
        public string? AttendeeEmail { get; set; }
        public DateTime SentAt { get; set; }
    }

    public class CreateEventIvDto
    {
        public string? AttendeeEmail { get; set; }
        public DateTime SentAt { get; set; }
    }

    public class CreateEventReminderDto
    {
        public DateTime ReminderTime { get; set; }
        public ReminderType Type { get; set; }
    }


    public class UpdateEventReminderDto
    {
        public DateTime ReminderTime { get; set; }
        public ReminderType Type { get; set; }
    }

    


    public class UpdateEventDto
    {
        public string? Name { get; set; }
        public EventType EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsInvitationOnly  { get; set; } = false;
        public bool HasPayment { get; set; } = false;
    }

    public class OrganizerInvitationListDto
    {
        public int Id { get; set; }
        public string? AttendeeEmail { get; set; }
        public DateTime SentAt { get; set; }
        public InvitationStatus? Status { get; set; }
    }




    public class OrganizerSessionListDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Speaker { get; set; }
    }

    public class OrganizerReminderListDto
    {
        public int Id { get; set; }
        public DateTime ReminderTime { get; set; }
        public string? Type { get; set; }
        public bool HasSent { get; set; } = false;
    }


    public class OrganizerEventDetailsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsInvitationOnly  { get; set; } = false;
        public bool HasPayment { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<OrganizerSessionListDto>? Sessions {get; set;} = new List<OrganizerSessionListDto>();
        public List<OrganizerReminderListDto>? Reminders {get; set;} = new List<OrganizerReminderListDto>();
    }








}