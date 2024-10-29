using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Mappers;
using backend.Models;
using backend.Services;
using Hangfire.States;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace backend.Repository
{
    public class AttendeeRepository : IAttendeeInterface
    {
        public readonly DBContext _context;
        private readonly FlutterwaveService _flutterRepo;
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;
        private readonly EmailService _emailService;
        public AttendeeRepository(
            DBContext context,
            FlutterwaveService flutterRepo,
            IConfiguration config,
            HttpClient httpClient,
            EmailService emailService
        )
        {
            _context = context;
            _flutterRepo = flutterRepo;
            _secretKey = config["Flutter:Secret_key"];
            _httpClient = httpClient;
            _emailService = emailService;
        }

        

        public async Task<(string message, Attendee? attendee)> RegisterAttendeeAsync(AttendeeRegisterDto attendeeRegisterDto, int eventId)
        {
            var properties = typeof(AttendeeRegisterDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(attendeeRegisterDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            string formattedPhone = StringHelpers.FormatPhoneNumber(attendeeRegisterDto.PhoneNumber.Trim());
            bool isValidPhone = StringHelpers.IsValidPhoneNumber(formattedPhone);
            if (!isValidPhone)
            {
                return (formattedPhone, null);
            }
            if (!StringHelpers.IsValidEmail(attendeeRegisterDto.Email.Trim()))
            {
                return ($"Invalid email {attendeeRegisterDto.Email}", null);
            }
            if (!await eventId.IsValidEventAttendee(_context))
                return ($"Event does not exists.", null);
            if (await attendeeRegisterDto.Email.IsValidEventRegistered(eventId, _context))
                return ($"You have already registered for this event", null);
            var eventModel = await _context.Events.FindAsync(eventId);
            if (eventModel.IsInvitationOnly){
                var invitationModel = await _context.Invitations
                    .FirstOrDefaultAsync(x => x.EventId == eventId && x.AttendeeEmail == attendeeRegisterDto.Email);

                // If the attendee is not invited, return a message
                if (invitationModel == null)
                {
                    return ($"Event is strictly by invitation. Please inform the organizer to invite you.", null);
                }
            }
            var attendeeModel = new Attendee{
                FirstName = attendeeRegisterDto.FirstName,
                LastName = attendeeRegisterDto.LastName,
                Email = attendeeRegisterDto.Email,
                PhoneNumber = formattedPhone,
                QRCode = "i will update this later",
                EventId = eventId
            };
            await _context.Attendees.AddAsync(attendeeModel);
            await _context.SaveChangesAsync();

            // var subject = "Welcome!";
            // var body = "This is a test email sent to the logged-in user.";
            // var email = "salawulucky08071@gmail.com";
            // await _emailService.SendEmailAsync(email, subject, body);

            return ($"Attendee registered successfully for event", attendeeModel);
        }

        public async Task<(string message, BuyTicketCondition ticketCondition)> AttendeeBuyTicketAsync(
            AttendeeBuyTicketDto AttendeeBuyTicketDto, int eventId, HttpRequest request)
        {
            var properties = typeof(AttendeeBuyTicketDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(AttendeeBuyTicketDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", BuyTicketCondition.error);
                    }
                }
            }
            if (!StringHelpers.IsValidEmail(AttendeeBuyTicketDto.Email.Trim()))
            {
                return ($"Invalid email {AttendeeBuyTicketDto.Email}", BuyTicketCondition.error);
            }
            if (!await eventId.IsValidEventAttendee(_context))
                return ($"Event does not exists.", BuyTicketCondition.error);
            if (!await AttendeeBuyTicketDto.Email.IsValidEventRegistered(eventId, _context))
                return ($"You have not registered for this event", BuyTicketCondition.error);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(x=>x.Attendee.Email == AttendeeBuyTicketDto.Email
            && x.IsCheckedIn == false);
            if (ticket != null)
                return ($"sorry you can buy, you till have unused ticket", BuyTicketCondition.error);
            var eventModel = await _context.Events.FindAsync(eventId);
            var attendee = await _context.Attendees.FirstOrDefaultAsync(x => x.Email == AttendeeBuyTicketDto.Email);
            if (eventModel?.TicketType == TicketType.Free)
            {
                var ticketModel = new Ticket{
                    AttendeeId = attendee?.Id,
                    TicketType = eventModel?.TicketType,
                    Price = eventModel.Price,
                };
                return ($"Event Ticket successfully purchased because it is free for event", BuyTicketCondition.create);
            }
            var result = await _flutterRepo.InitializePayment(eventModel.Price, attendee, eventId, request);
            return ($"{result}", BuyTicketCondition.flutter);

        }


        public async Task<(FlutterwaveResponseDto?, string Message, bool IsCompleted)> VerifyTicketPaymentAsync(string transaction_id)
        {
            
            var flutterwaveSecretKey = _secretKey;
            var url = $"https://api.flutterwave.com/v3/transactions/{transaction_id}/verify";
            try
            {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", flutterwaveSecretKey);
            _httpClient.Timeout = TimeSpan.FromSeconds(30); // Set timeout

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Throws if not 200-299

            var content = await response.Content.ReadAsStringAsync();
            

            // Deserialize and return the response
            var flutterwaveResponse = JsonConvert.DeserializeObject<FlutterwaveResponseDto>(content);
            
            if (flutterwaveResponse != null)
            {
                // Return the DTO, success message, and completed status
                return (flutterwaveResponse, "Payment verified successfully.", true);
            }
            else
            {
                return (null, "Failed to verify the payment: Invalid response data.", false);
            }
            }
            catch (HttpRequestException ex)
            {
                return (null, $"Failed to verify payment due to a network error: {ex.Message}", false);
            }
            catch (TaskCanceledException ex)
            {
                return (null, $"The request timed out: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                return (null, $"An unexpected error occurred: {ex.Message}", false);
            }
        }

        public async Task<(string message, bool IsSuccess)> VerifyTicketPaymentSettleAsync(FlutterwaveResponseDto responseDto, int eventId, string transaction_id)
        {
            var email = responseDto?.Data?.Customer?.Email;
            var phone = responseDto?.Data?.Customer?.Phone_number;
            var name = responseDto?.Data?.Customer?.Name;
            var Created_at = responseDto.Data.Customer.Created_at;
            var attendee = await _context.Attendees.FirstOrDefaultAsync(x => x.Email == email
            && x.PhoneNumber == phone && x.FirstName == name);
            var eventModel = await _context.Events.FindAsync(eventId);
            if(attendee == null)
                return ("Payment Verified, but data wasn't fetch correctly. please use the re-confirm feature.", false);
            
            var wallet = await _context.Wallets.FirstOrDefaultAsync(x=>x.UserId==eventModel.OrganizerId);
            if (wallet == null)
                return ("Organizer's wallet not found. please contact the administrator", false);
            wallet.Balance += eventModel.Price;
            await _context.SaveChangesAsync();
            var ticketModel = new Ticket{
                    AttendeeId = attendee?.Id,
                    TicketType = eventModel?.TicketType,
                    Price = eventModel.Price,
                };
            await _context.Tickets.AddAsync(ticketModel);
            await _context.SaveChangesAsync();
            var paymentModel = new Payment{
                Amount = eventModel.Price,
                PaymentDate = Created_at,
                Method = PaymentMethod.BankTransfer,
                Status = PaymentStatus.Completed,
                TicketId = ticketModel.Id,
                EventId = eventId,
                TransactionId = transaction_id

            };
            await _context.Payments.AddAsync(paymentModel);

            var transactionModel = new Transaction{
                UserId = eventModel.OrganizerId,
                Amount = eventModel.Price,
                Description = $"Payment for purchase of Ticket for Event: {eventModel.Name}",
                Ref = transaction_id,
                Date = Created_at
            };
            await _context.Transactions.AddAsync(transactionModel);
            await _context.SaveChangesAsync();

            return ("Payment verified and settled successfully", true);

        }

        public async Task<(string message, bool IsSuccess)> ReConfirmTicketPaymentSettleAsync(ReBuyTicketDto reBuyTicketDto, int eventId)
        {
            var properties = typeof(ReBuyTicketDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(reBuyTicketDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", false);
                    }
                }
            }
            if (!StringHelpers.IsValidEmail(reBuyTicketDto.Email.Trim()))
            {
                return ($"Invalid email {reBuyTicketDto.Email}", false);
            }
            if (!await eventId.IsValidEventAttendee(_context))
                return ($"Event does not exists.", false);
            if (!await reBuyTicketDto.Email.IsValidEventRegistered(eventId, _context))
                return ($"You have not registered for this event yet", false);

            var validPayment = await _context.Payments.FirstOrDefaultAsync(x=>x.EventId==eventId && x.TransactionId == reBuyTicketDto.TransactionId);
            if(validPayment != null)
                return ($"This transaction has been settled", false);
            return ($"Proceeding", true);
            
            
        }

        public async Task<(List<AttendeeTicketListDto>? ticketListDtos, string message)> AttendeeGetEventTicketDetailsAsync(AttendeeEmailDto emailDto, int eventId)
        {
            if(string.IsNullOrEmpty(emailDto.Email.Trim())){
                return (null, "Email cannot be empty");
            }
            if (!StringHelpers.IsValidEmail(emailDto.Email.Trim()))
            {
                return (null, $"Invalid email {emailDto.Email}");
            }
            if (!await eventId.IsValidEventAttendee(_context))
                return (null, $"Event does not exists.");
            if (!await emailDto.Email.IsValidEventRegistered(eventId, _context))
                return (null, $"You have not registered for this event");
            var tickets = await _context.Tickets
            .Where(x=>x.Attendee.Email == emailDto.Email && x.Attendee.EventId == eventId)
            .Include(x=>x.Payments)
            .ToListAsync();
            var ticketsDto = tickets?.Select(x=>x.ToAttendeeTicketListDto()).ToList();
            return (ticketsDto, "Ok");
        
            
        }

        public async Task<List<AttendeeEventsListDto>?> AttendeesEventsListAsync(AttendeeListEventQuery query)
        {
            var eventLists = _context.Events
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
                .Select(x => x.ToAttendeeEventsListDto());

            var SkipNumber = (query.PageNumber - 1) * query.PageSize;
            return await eventListDtos.Skip(SkipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<(AttendeeEventDetailsDto? attendeeEventDetails, bool IsSuccess)> AttendeeGetEventDetailsAsync(int eventId)
        {
            if (!await eventId.IsValidEventAttendee(_context))
                return (null, false);
            var existingEvent = await _context.Events
            .Include(x=>x.Sessions)
            .Include(x=>x.Feedbacks)
            .FirstOrDefaultAsync(x=>x.Id == eventId);

            var eventDetailsDto = existingEvent?.ToAttendeeEventDetailsDto();
            return (eventDetailsDto, true);
            
        }

        public async Task<(string message, bool IsSuccess)> AttendeeCreateFeedbacks (AttendeeCreateFeedback createFeedback, int eventId)
        {
            var properties = typeof(AttendeeCreateFeedback).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(createFeedback);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", false);
                    }
                }
            }
            if (!int.TryParse(createFeedback.Rating.ToString(), out int rating) || rating < 1 || rating > 5)
            {
                return ("Rating must be an integer between 1 and 5", false);
            }
            if (!StringHelpers.IsValidEmail(createFeedback.Email.Trim()))
            {
                return ($"Invalid email {createFeedback.Email}", false);
            }
            if (!await eventId.IsValidEventAttendee(_context))
                return ($"Event does not exists.", false);
            if (!await createFeedback.Email.IsValidEventRegistered(eventId, _context))
                return ($"You have to  registered for this event before you can comment", false);
            int Feedback = await _context.Feedbacks
            .Where(x=>x.AttendeeEmail == createFeedback.Email)
            .CountAsync();
            if (Feedback > 3)
                return ($"you can only give 3 feedbacks", false);
            var feedbackModel = new Feedback{
                AttendeeEmail = createFeedback.Email,
                Comments = createFeedback.Comments,
                Rating = createFeedback.Rating,
                EventId = eventId

            };
            await _context.Feedbacks.AddAsync(feedbackModel);
            await _context.SaveChangesAsync();

            return($"feedback added", true);

        }

        public async Task<(string message, bool IsSuccess)> RefundTicketPayment(AttendeeEmailDto emailDto, int id)
        {
            if(string.IsNullOrEmpty(emailDto.Email.Trim())){
                return ("Email cannot be empty", false);
            }
            if (!StringHelpers.IsValidEmail(emailDto.Email.Trim()))
            {
                return ($"Invalid email {emailDto.Email}", false);
            }
            var ticket = await _context.Tickets.FirstOrDefaultAsync(x=>x.Attendee.Email == emailDto.Email && x.Id == id);
            if(ticket == null)
                return ($"ticket cannot be found", false);
            if (ticket.IsCheckedIn == true)
                return ("ticket has been used", false);
            return("ok", true);

        }
    }
}