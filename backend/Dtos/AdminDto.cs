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
}