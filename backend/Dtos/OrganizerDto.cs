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








}