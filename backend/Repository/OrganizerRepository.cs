using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Mappers;
using backend.Models;
using backend.Services;
using CsvHelper;
using ExcelDataReader;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class OrganizerRepository : IOrganizerInterfaces
    {
        public readonly DBContext _context;
        public OrganizerRepository(
            DBContext context
        )
        {
            _context = context;
        }

        public async Task<List<organizerEventsDto>?> OrganizerEventsAsync(string userId, OrganizerListEventQuery query)
        {
            var eventLists = _context.Events
            .Where(x=>x.OrganizerId == userId)
            .OrderByDescending(x=>x.CreatedAt)
            .AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Name))
            {
                eventLists = eventLists.Where(x=>x.Name.Contains(query.Name));
            };
            if(query.EventType != null)
            {
                eventLists = eventLists.Where(x=>x.EventType ==query.EventType);
            };
            if(!string.IsNullOrWhiteSpace(query.State))
            {
                    eventLists = eventLists.Where(x=>x.State.Contains(query.State));
            };
            if(!string.IsNullOrWhiteSpace(query.Location))
            {
                    eventLists = eventLists.Where(x=>x.Location.Contains(query.Location));
            };

            if (query.StartDate.HasValue)
            {
                eventLists = eventLists.Where(x =>x.StartDate.Date == query.StartDate.Value.Date);
            };
            if (query.EndDate.HasValue)
            {
                eventLists = eventLists.Where(x => x.EndDate.Date == query.EndDate.Value.Date);
            }

            var eventListDtos = eventLists
                .Select(x => x.ToOrganizerEventsDto());

            var SkipNumber = (query.PageNumber - 1) * query.PageSize;
            return await eventListDtos.Skip(SkipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<(string message, Event? createEvent)> CreateEventAsync (CreateEventDto createEventDto, string userId)
        {
            var properties = typeof(CreateEventDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(createEventDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
             if (!Enum.IsDefined(typeof(EventType), createEventDto.EventType))
                return ("Invalid Event type.", null);
            
            var existingEvent = await _context.Events.FirstOrDefaultAsync(x=>x.Name == createEventDto.Name
             && x.EventType == createEventDto.EventType
             && x.OrganizerId == userId);
            if (existingEvent != null)
            {
                return ($"Event already exists.", null);
            }
            else{
                var eventModel = new Event{
                    OrganizerId = userId,
                    Name = createEventDto.Name,
                    EventType = createEventDto.EventType,
                    State = createEventDto.State,
                    Location=createEventDto.Location,
                    StartDate = createEventDto.StartDate,
                    EndDate = createEventDto.EndDate,
                    Description = createEventDto.Description,
                    HasPayment = createEventDto.HasPayment,
                    IsInvitationOnly = createEventDto.IsInvitationOnly
                };
                if (eventModel.HasPayment && createEventDto.Price <= 0)
                {
                    return ($"Amount cannot be 0.00 when 'HasPayment' is true", null);
                }
                eventModel.TicketType = eventModel.HasPayment ? TicketType.Paid : TicketType.Free;
                eventModel.Price = createEventDto.Price;
                await _context.Events.AddAsync(eventModel);
                
            var checkEventTemplateExists = await _context.EventTemplates.FirstOrDefaultAsync(x=>x.Events.Any(e => e.OrganizerId == userId)
            && x.EventType == createEventDto.EventType || x.Name == createEventDto.Name);
            if (checkEventTemplateExists == null){
            var eventTemplate = new EventTemplate{
                Name = createEventDto.Name,
                EventType = createEventDto.EventType,
                DefaultLocation=createEventDto.Location,
                DefaultStartDate = createEventDto.StartDate,
                DefaultEndDate = createEventDto.EndDate,
                Description = createEventDto.Description,
            };
            await _context.EventTemplates.AddAsync(eventTemplate);

            }
            await _context.SaveChangesAsync();
            return ($"successfully added {createEventDto.Name}", eventModel);
            }
             
            
        }

        public async Task<(string message, Session? session)> CreateEventSessionAsync(CreateEventSessionDto createEventSessionDto, int id, string userId)
        {
            var properties = typeof(CreateEventSessionDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(createEventSessionDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }

            if (!await id.IsValidEvent(userId, _context))
                return ($"Event with Id: {id} doesn't exists.", null);
            var existingEventSession = await _context.Sessions.FirstOrDefaultAsync(x=>x.Title == createEventSessionDto.Title
            && x.EventId == id && x.Event.OrganizerId == userId);
            if (existingEventSession != null)
                return ($"EventSession already exists.", null);
            var eventSession = new Session{
                Title = createEventSessionDto.Title,
                StartTime = createEventSessionDto.StartTime,
                EndTime = createEventSessionDto.EndTime,
                Speaker = createEventSessionDto.Speaker,
                EventId = id
            };
            await _context.Sessions.AddAsync(eventSession);
            await _context.SaveChangesAsync();
            return ($"Successfully added {eventSession.Title} to Event: {eventSession.Event.Name}", eventSession);


        }

        public async Task<(string message, Invitation? invitation)> CreateEventIvAsync(CreateEventIvDto createEventIvDto, int id, string userId)
        {
            var properties = typeof(CreateEventIvDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(createEventIvDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            if (!StringHelpers.IsValidEmail(createEventIvDto.AttendeeEmail.Trim()))
            {
                return ($"Invalid email {createEventIvDto.AttendeeEmail}", null);
            }
            if (!await id.IsValidEvent(userId, _context))
                return ($"Event with Id: {id} doesn't exists.", null);
            var existingEventIv = await _context.Invitations.FirstOrDefaultAsync(x=>x.AttendeeEmail == createEventIvDto.AttendeeEmail
            && x.EventId == id && x.Event.OrganizerId == userId);
            if (existingEventIv != null)
                return ($"EventInvitation already exists for {existingEventIv.AttendeeEmail}.", null);
            var eventInvitation = new Invitation{
                AttendeeEmail = createEventIvDto.AttendeeEmail,
                SentAt = createEventIvDto.SentAt,
                Status = 0,
                EventId = id
            };
            await _context.Invitations.AddAsync(eventInvitation);
            await _context.SaveChangesAsync();
            return ($"Successfully invites {eventInvitation.AttendeeEmail} to Event: {eventInvitation.Event.Name}", eventInvitation);

        }

        public async Task<(string message, bool IsSuccess)> UploadBulkEventIvAsync(IFormFile file, int id, string userId)
        {
            if (file == null || file.Length == 0)
            return ("No file uploaded.", false);
            if (!await id.IsValidEvent(userId, _context))
                return ($"Event with Id: {id} doesn't exists.", false);
            List<CreateEventIvDto> eventIvs = new List<CreateEventIvDto>();
            var extension = Path.GetExtension(file.FileName).ToLower();
            int successfulImports = 0,duplicateCount = 0,nullDataCount = 0,invalidDataCount = 0,inValidEmail = 0,updatedRecord = 0;
            if (!StringHelpers.IsValidExtension(extension))
                return ("Unsupported file format. Please upload a CSV or Excel file.", false);
            if (extension == ".csv")
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    csv.Read();
                    csv.ReadHeader();
                    string[] expectedHeaders = new string[] { "Email", "Date"};
                    foreach (var header in expectedHeaders)
                    {
                        if (!csv.HeaderRecord.Contains(header))
                        {
                            return ($"Invalid header found. Expected '{header}' but not found in CSV.", false);
                        }
                        while (csv.Read())
                        {
                            var email = csv.GetField("Email")?.Trim();
                            var date = csv.GetField("Date")?.Trim();

                            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(date))
                            {
                                nullDataCount++;
                                continue; // Skip if any field is null
                            }
                            if (DateTime.TryParse(date, out var parsedDate))
                            {
                                var eventIv = new CreateEventIvDto
                                {
                                    AttendeeEmail = email,
                                    SentAt = parsedDate
                                };
                                eventIvs.Add(eventIv);
                            }
                                invalidDataCount++;
                                continue;

                        }
                    }
                }
            }
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    bool isFirstRow = true;
                    string[] expectedHeaders = new string[] { "Email", "Date" };
                    string[] actualHeaders = new string[expectedHeaders.Length];
                    while (reader.Read())
                    {
                        if (isFirstRow)
                        {
                            for (int i = 0; i < expectedHeaders.Length; i++)
                            {
                                actualHeaders[i] = reader.GetValue(i)?.ToString().Trim();
                            }
                            for (int i = 0; i < expectedHeaders.Length; i++)
                            {
                                if (!string.Equals(actualHeaders[i], expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                                {
                                    return ($"Invalid header '{actualHeaders[i]}' found. Expected '{expectedHeaders[i]}'.", false);
                                }
                            }
                            isFirstRow = false;
                            continue;
                        }
                        var email = reader.GetValue(0)?.ToString().Trim();
                        var date = reader.GetValue(1)?.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(date))
                        {
                            nullDataCount++;
                            continue;
                        }
                        if (DateTime.TryParse(date, out var parsedDate))
                        {
                            var eventIv = new CreateEventIvDto
                            {
                                AttendeeEmail = email,
                                SentAt = parsedDate
                            };
                            eventIvs.Add(eventIv);
                        }
                            invalidDataCount++;
                            continue;
                    }
                }

            }
            foreach (var invitationDto in eventIvs)
            {
                if (!StringHelpers.IsValidEmail(invitationDto.AttendeeEmail))
                    inValidEmail ++;
                var existingEventIv = await _context.Invitations.FirstOrDefaultAsync(x=>x.AttendeeEmail == invitationDto.AttendeeEmail
                && x.EventId == id && x.Event.OrganizerId == userId);
                if (existingEventIv != null)
                {
                    if (existingEventIv.SentAt == invitationDto.SentAt)
                    {
                        duplicateCount++;
                        continue;
                    }else{
                        existingEventIv.AttendeeEmail = invitationDto.AttendeeEmail;
                        existingEventIv.SentAt = invitationDto.SentAt;
                        existingEventIv.Status = 0;
                        await _context.SaveChangesAsync();
                        updatedRecord ++;
                        return ($"file processed: createdRecord: {successfulImports}, duplicateRecord: {duplicateCount}, nullData: {nullDataCount}, invalidDate: {invalidDataCount}, invalidEmail: {inValidEmail}, updatedRecord: {updatedRecord}", true);
                    }

                }else{
                    var eventInvitation = new Invitation{
                        AttendeeEmail = invitationDto.AttendeeEmail,
                        SentAt = invitationDto.SentAt,
                        Status = 0,
                        EventId = id
                    };
                    await _context.Invitations.AddAsync(eventInvitation);
                    await _context.SaveChangesAsync();
                    successfulImports ++;
                }
            }
            return ($"file processed: createdRecord: {successfulImports}, duplicateRecord: {duplicateCount}, nullData: {nullDataCount}, invalidDate: {invalidDataCount}, invalidEmail: {inValidEmail}, updatedRecord: {updatedRecord}", true);
        }

        public async Task<(string message, Reminder? reminder)> CreateReminderAsync(CreateEventReminderDto eventReminderDto, int id, string userId)
        {
            var properties = typeof(CreateEventReminderDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(eventReminderDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }

            if (!await id.IsValidEvent(userId, _context))
                return ($"Event with Id: {id} doesn't exists.", null);
            var existingEventReminder = await _context.Reminders.FirstOrDefaultAsync(x=>x.ReminderTime == eventReminderDto.ReminderTime
            && x.EventId == id && x.Event.OrganizerId == userId && x.Type == eventReminderDto.Type);
            if (existingEventReminder != null)
                return ($"EventReminder already set for {existingEventReminder.Event.Name}.", null);
            var reminderModel = new Reminder{
                Type = eventReminderDto.Type,
                ReminderTime = eventReminderDto.ReminderTime,
                EventId = id
            };
            await _context.Reminders.AddAsync(reminderModel);
            await _context.SaveChangesAsync();
            return ($"Reminder is now schedule to be send.", reminderModel);
        }

        public async Task<(string message, Event? UpdateEvent)> UpdateEventAsync(UpdateEventDto updateEventDto, int id, string userId)
        {
            var properties = typeof(UpdateEventDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(updateEventDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            if (!await id.IsValidEvent(userId, _context))
                return ($"Event with Id: {id} doesn't exists.", null);
            var existingEvent = await _context.Events.FindAsync(id);
            existingEvent.Name = updateEventDto.Name;
            existingEvent.EventType = updateEventDto.EventType;
            existingEvent.State = updateEventDto.State;
            existingEvent.Location=updateEventDto.Location;
            existingEvent.StartDate = updateEventDto.StartDate;
            existingEvent.EndDate = updateEventDto.EndDate;
            existingEvent.Description = updateEventDto.Description;
            existingEvent.HasPayment = updateEventDto.HasPayment;
            existingEvent.IsInvitationOnly = updateEventDto.IsInvitationOnly;
            existingEvent.TicketType = existingEvent.HasPayment ? TicketType.Paid : TicketType.Free;
            if (existingEvent.HasPayment && updateEventDto.Price <= 0)
                return ($"Amount cannot be 0.00 when 'HasPayment' is true", null);
            existingEvent.Price = updateEventDto.Price;
            await _context.SaveChangesAsync();
            return ($"{existingEvent.Name} has been updated successfully", existingEvent);
        }

        public async Task<(string message, bool IsSuccess)> DeleteEventAsync(int id, string userId)
        {
            if (!await id.IsValidEvent(userId, _context))
                return ($"Event with Id: {id} doesn't exists.", false);
            var existingEvent = await _context.Events.FindAsync(id);
            
             _context.Events.Remove(existingEvent);
             await _context.SaveChangesAsync();
             return ($"Event Deleted successfully", true);
        }

        public async Task<(OrganizerEventDetailsDto? eventDetailsDto, bool IsSuccess)> GetEventDetailsAsync(int id, string userId)
        {
            if (!await id.IsValidEvent(userId, _context))
                return (null, false);
            var existingEvent = await _context.Events
            .Include(x=>x.Sessions)
            .Include(x=>x.Reminders)
            .FirstOrDefaultAsync(x=>x.Id == id);

            var eventDetailsDto = existingEvent?.ToOrganizerEventDetailsDto();
            return (eventDetailsDto, true);
            
        }

        public async Task<(string message, Session? session)> UpdateEventSessionAsync(UpdateEventSessionDto eventSessionDto, int id, int eventId, string userId)
        {
            var properties = typeof(UpdateEventSessionDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(eventSessionDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            if (!await id.IsValidEventSession(eventId, userId, _context))
                return ($"EventSession doesn't exists.", null);
            var existingEventSession = await _context.Sessions.FindAsync(id);
            existingEventSession.Title = eventSessionDto.Title;
            existingEventSession.StartTime = eventSessionDto.StartTime;
            existingEventSession.EndTime = eventSessionDto.EndTime;
            existingEventSession.Speaker = eventSessionDto.Speaker;
            await _context.SaveChangesAsync();
            return ($"Successfully updated {existingEventSession.Title} for Event: {existingEventSession.Event?.Name}", existingEventSession);
        }

       public async Task<(string message, bool IsSuccess)> DeleteEventSessionAsync(int id, int eventId, string userId)
        {
            if (!await id.IsValidEventSession(eventId, userId, _context))
                return ($"EventSession doesn't exists.", false);
            var existingEventSession = await _context.Sessions.FindAsync(id);
            _context.Sessions.Remove(existingEventSession);
            await _context.SaveChangesAsync();
            return ($"EventSession Deleted successfully", true);

        }

        public async Task<(string message, Invitation? invitation)> UpdateEventInvitationAsync(UpdateEventIvDto updateEventIvDto, int id, int eventId, string userId)
        {
            var properties = typeof(UpdateEventIvDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(updateEventIvDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            if (!await id.IsValidEventInvitation(eventId, userId, _context))
                return ($"EventInvitation doesn't exists.", null);
                var existingEventIV = await _context.Invitations.FindAsync(id);
                existingEventIV.AttendeeEmail = updateEventIvDto.AttendeeEmail;
                existingEventIV.SentAt = updateEventIvDto.SentAt;
                await _context.SaveChangesAsync();
                return ($"Successfully updated {existingEventIV.AttendeeEmail} for Event: {existingEventIV.Event?.Name}", existingEventIV);


        }

        public async Task<(List<OrganizerInvitationListDto>? invitationListDto, bool IsSuccess)> GetEventIvDetailsAsync(int id, string userId)
        {
            if (!await id.IsValidEvent(userId, _context))
                return (null, false);
            var invitations =await _context.Invitations
            .Where(x=>x.EventId == id)
            .ToListAsync();

            var invitationDto = invitations?.Select(x=>x.ToOrganizerInvitationListDto()).ToList();

            return (invitationDto, true);

            
        }

        public async Task<(string message, bool IsSuccess)> DeleteEventIvAsync(int id, int eventId, string userId)
        {
            if (!await id.IsValidEventInvitation(eventId, userId, _context))
                return ($"EventInvitation doesn't exists.", false);
            var existingEventIv = await _context.Invitations.FindAsync(id);
            _context.Invitations.Remove(existingEventIv);
            await _context.SaveChangesAsync();
            return ($"EventInvitation Deleted successfully", true);

        
        }

        public async Task<(string message, Reminder? reminder)> UpdateEventReminderAsync(UpdateEventReminderDto eventReminderDto, int id, int eventId, string userId)
        {
            var properties = typeof(UpdateEventReminderDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(eventReminderDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            if (!await id.IsValidEventReminder(eventId, userId, _context))
                return ($"EventReminder doesn't exists.", null);
            var existingEventReminder = await _context.Reminders.FindAsync(id);
            existingEventReminder.Type = eventReminderDto.Type;
            existingEventReminder.ReminderTime = eventReminderDto.ReminderTime;
            await _context.SaveChangesAsync();
            return ($"Successfully updated Reminder for Event: {existingEventReminder.Event?.Name}", existingEventReminder);

        }

        public async Task<(string message, bool IsSuccess)> DeleteEventReminderAsync(int id, int eventId, string userId)
        {
            if (!await id.IsValidEventReminder(eventId, userId, _context))
                return ($"EventReminder doesn't exists.", false);
            var existingEventReminder = await _context.Reminders.FindAsync(id);
            _context.Reminders.Remove(existingEventReminder);
            await _context.SaveChangesAsync();
            return ($"EventReminder Deleted successfully", true);
        }

        public async Task<(List<OrganizerAttendeeListDto>? attendeeListDto, bool IsSuccess)> GetEventAttendeeDetailsAsync(int id, string userId)
        {
            if (!await id.IsValidEvent(userId, _context))
                return (null, false);
            var attendees =await _context.Attendees
            .Where(x=>x.EventId == id)
            .ToListAsync();

            var attendeesDto = attendees?.Select(x=>x.ToOrganizerAttendeeListDto()).ToList();

            return (attendeesDto, true);
        }

        public async Task<(List<OrganizerTicketListDto>? ticketListDto, bool IsSuccess)> GetEventTicketDetailsAsync(int id, string userId)
        {
            if (!await id.IsValidEvent(userId, _context))
                return (null, false);
            var tickets =await _context.Tickets
            .Where(x=>x.Attendee.EventId == id)
            .ToListAsync();

            var ticketsDto = tickets?.Select(x=>x.ToOrganizerTicketListDto()).ToList();

            return (ticketsDto, true);
            
        }

        public async Task<(List<OrganizerPaymentListDto>? paymentListDto, bool IsSuccess)> GetEventPaymentDetailsAsync(int id, string userId)
        {
            if (!await id.IsValidEvent(userId, _context))
                return (null, false);
            var payments =await _context.Payments
            .Where(x=>x.EventId == id)
            .ToListAsync();

            var paymentsDto = payments?.Select(x=>x.ToOrganizerPaymentListDto()).ToList();

            return (paymentsDto, true);
        }

        public async Task<(List<OrganizerFeedbackListDto>? feedbackListDto, bool IsSuccess)> GetEventRatingDetailsAsync(int id, string userId)
        {
            if (!await id.IsValidEvent(userId, _context))
                return (null, false);
            var feedbacks =await _context.Feedbacks
            .Where(x=>x.EventId == id)
            .ToListAsync();

            var feedbacksDto = feedbacks?.Select(x=>x.ToOrganizerFeedbackListDto()).ToList();

            return (feedbacksDto, true);
        }

        public async Task<(OrganizerWalletTransactionsDto?, bool IsSuccess)> GetWalletTransactionsAsync(string userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(x=>x.UserId == userId);
            if (wallet == null)
                return (null, false);
            decimal balance = wallet.Balance;
            var transactions = await _context.Transactions
            .Where(x=>x.UserId == userId)
            .ToListAsync();
            var transactionsDto = transactions.Select(x=>x.ToOrganizerTransactionsDto()).ToList();
            var walletTransactionsDto = new OrganizerWalletTransactionsDto
            {
                Balance = balance,
                Transactions = transactionsDto
            };

            return (walletTransactionsDto, true);
        }
    }
}
