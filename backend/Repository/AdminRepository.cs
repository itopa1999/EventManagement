using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Mappers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace backend.AdminRepository
{
    public class AdminRepository : IAdminInterface
    {
        public readonly DBContext _context;
        public readonly UserManager<User> _userManager;
        public readonly IJWTService _token;
        public AdminRepository(
            DBContext context,
            UserManager<User> userManager,
            IJWTService token
        )
        {
            _context = context;
            _userManager = userManager;
            _token=token;
        }

        public async Task<BlockAccessDto> BlockAccessDtoAsync(HttpContext context)
        {
            var blockAccessDto = await BlockAccessFunctionMiddleware.BlockAccessDtoAsync(context);
            return blockAccessDto;
        }

        public async Task<(string message, bool IsSuccess)> BlockUnblockEmailAccessDtoAsync(string email, string status)
        {
            if(string.IsNullOrEmpty(email.Trim())){
                return ("Email cannot be empty", false);
            }
            if(string.IsNullOrEmpty(status.Trim())){
                return ("status cannot be empty", false);
            }
            if (!StringHelpers.IsValidBlockStatus(status.Trim()))
            {
                return ($"Status can be either 'Activate' or 'Deactivate'", false);
            }
            if (!StringHelpers.IsValidEmail(email.Trim()))
            {
                return ($"Invalid email {email}", false);
            }
            if (!await email.IsValidEmailAdmin(_context))
                return ($"Email does not exists.", false);
            var existingAttendee = await _context.Attendees.FirstOrDefaultAsync(x => x.Email == email);
            if (status == "Deactivate")
            {
                existingAttendee.IsBlock = false;
                await _context.SaveChangesAsync();
                return ("Email has been unblocked successfully", true);
            }else if (status == "Activate"){
                existingAttendee.IsBlock = true;
                await _context.SaveChangesAsync();
                return ("Email has been blocked successfully", true);
            }
            else{
                return ("Cannot complete the action", false);
            }
            
            
            
        }

        public async Task<(string message, bool IsSuccess)> BlockUnblockEventAccessDtoAsync(int eventId, string status)
        {
            if (eventId == null)
                return ("Event ID is empty", false);
            if(string.IsNullOrEmpty(status.Trim())){
                return ("status cannot be empty", false);
            }
            if (!StringHelpers.IsValidBlockStatus(status.Trim()))
            {
                return ($"Status can be either 'Activate' or 'Deactivate'", false);
            }
            if (!await eventId.IsValidEventAttendee(_context))
                return ($"Event does not exists.", false);
            var existingEvent = await _context.Events.FirstOrDefaultAsync(x => x.Id == eventId);
            if (status == "Deactivate")
            {
                existingEvent.IsBlock = false;
                await _context.SaveChangesAsync();
                return ("Event has been unblocked successfully", true);
            }else if (status == "Activate"){
                existingEvent.IsBlock = true;
                await _context.SaveChangesAsync();
                return ("Event has been blocked successfully", true);
            }
            else{
                return ("Cannot complete the action", false);
            }

        }

        public async Task<(string message, bool IsSuccess)> BlockUnblockUserAccessDtoAsync(string Id, string status)
        {
            if(string.IsNullOrEmpty(Id.Trim())){
                return ("Id cannot be empty", false);
            };
            if(string.IsNullOrEmpty(status.Trim())){
                return ("status cannot be empty", false);
            }
            if (!StringHelpers.IsValidBlockStatus(status.Trim()))
            {
                return ($"Status can be either 'Activate' or 'Deactivate'", false);
            }
            var userModel = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id==Id && x.UserType != UserType.Admin);
            if (userModel == null)
                return ("User does not exist", false);
            if (status == "Deactivate")
            {
                userModel.IsBlock = false;
                await _context.SaveChangesAsync();
                return ("User has been unblocked successfully", true);
            }else if (status == "Activate"){
                userModel.IsBlock = true;
                await _context.SaveChangesAsync();
                return ("User has been blocked successfully", true);
            }
            else{
                return ("Cannot complete the action", false);
            }

        }
        public async Task<(string message, bool IsSuccess)> BlockUnblockWalletAccessDtoAsync(string Id, string status)
        {
            if(string.IsNullOrEmpty(Id.Trim())){
                return ("Id cannot be empty", false);
            };
            if(string.IsNullOrEmpty(status.Trim())){
                return ("status cannot be empty", false);
            }
            if (!StringHelpers.IsValidBlockStatus(status.Trim()))
            {
                return ($"Status can be either 'Activate' or 'Deactivate'", false);
            }
            var userModel = await _userManager.FindByIdAsync(Id);
            if (userModel == null)
                return ("User does not exist", false);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(x=>x.UserId == Id);
            if (wallet == null)
                return ("Wallet not found for account", false);
            if (status == "Deactivate")
            {
                wallet.IsBlock = false;
                await _context.SaveChangesAsync();
                return ("User wallet has been unblocked successfully", true);
            }else if (status == "Activate"){
                wallet.IsBlock = true;
                await _context.SaveChangesAsync();
                return ("User wallet has been blocked successfully", true);
            }
            else{
                return ("Cannot complete the action", false);
            }
        }
        public async Task<(string message, bool IsSuccess)> AllBlockUnblockEmailAccessDtoAsync(string status)
        {
            if(string.IsNullOrEmpty(status.Trim())){
                return ("status cannot be empty", false);
            }
            if (!StringHelpers.IsValidBlockStatus(status.Trim()))
            {
                return ($"Status can be either 'Activate' or 'Deactivate'", false);
            }
            if (status == "Deactivate")
            {
                var allAttendees = await _context.Attendees.ToListAsync();
                allAttendees.ForEach(a => a.IsBlock = false);
                await _context.SaveChangesAsync();
                return ("All emails unblocked successfully", true);
            }else if (status == "Activate"){
                var allAttendees = await _context.Attendees.ToListAsync();
                allAttendees.ForEach(a => a.IsBlock = true);
                await _context.SaveChangesAsync();
                return ("All emails blocked successfully", true);
            }
            else{
                return ("Cannot complete the action", false);
            }
            
        }

        public async Task<(string message, bool IsSuccess)> AllBlockUnblockEventAccessDtoAsync(string status)
        {
            if(string.IsNullOrEmpty(status.Trim())){
                return ("status cannot be empty", false);
            }
            if (!StringHelpers.IsValidBlockStatus(status.Trim()))
            {
                return ($"Status can be either 'Activate' or 'Deactivate'", false);
            }
            if (status == "Deactivate")
            {
                var allEvent = await _context.Events.ToListAsync();
                allEvent.ForEach(a => a.IsBlock = false);
                await _context.SaveChangesAsync();
                return ("All Events unblocked successfully", true);
            }else if (status == "Activate"){
                var allEvent = await _context.Events.ToListAsync();
                allEvent.ForEach(a => a.IsBlock = true);
                await _context.SaveChangesAsync();
                return ("All Events blocked successfully", true);
            }
            else{
                return ("Cannot complete the action", false);
            }
        }

        public async Task<(string message, bool IsSuccess)> AllBlockUnblockUserAccessDtoAsync(string status)
        {
            if(string.IsNullOrEmpty(status.Trim())){
                return ("status cannot be empty", false);
            }
            if (!StringHelpers.IsValidBlockStatus(status.Trim()))
            {
                return ($"Status can be either 'Activate' or 'Deactivate'", false);
            }
            if (status == "Deactivate")
            {
                var allUsers = await _userManager.Users.Where(x=>x.UserType != UserType.Admin).ToListAsync();
                allUsers.ForEach(a => a.IsBlock = false);
                await _context.SaveChangesAsync();
                return ("All Users unblocked successfully", true);
            }else if (status == "Activate"){
                var allUsers = await _userManager.Users.ToListAsync();
                allUsers.ForEach(a => a.IsBlock = true);
                await _context.SaveChangesAsync();
                return ("All Users blocked successfully", true);
            }
            else{
                return ("Cannot complete the action", false);
            }
        }

        public async Task<(string message, bool IsSuccess)> AllBlockUnblockWalletAccessDtoAsync(string status)
        {
            if(string.IsNullOrEmpty(status.Trim())){
                return ("status cannot be empty", false);
            }
            if (!StringHelpers.IsValidBlockStatus(status.Trim()))
            {
                return ($"Status can be either 'Activate' or 'Deactivate'", false);
            }
            if (status == "Deactivate")
            {
                var allWallets = await _context.Wallets.ToListAsync();
                allWallets.ForEach(a => a.IsBlock = false);
                await _context.SaveChangesAsync();
                return ("All Wallets unblocked successfully", true);
            }else if (status == "Activate"){
                var allWallets = await _context.Wallets.ToListAsync();
                allWallets.ForEach(a => a.IsBlock = true);
                await _context.SaveChangesAsync();
                return ("All Wallets blocked successfully", true);
            }
            else{
                return ("Cannot complete the action", false);
            }
        }


        public async Task<(string message, User? user)> CreateOrganizerAsync(CreateOrganizerDto organizerDto)
        {
            var properties = typeof(CreateOrganizerDto).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(organizerDto);
                    if (string.IsNullOrEmpty(value))
                    {
                        return ($"{property.Name} cannot be empty.", null);
                    }
                }
            }
            if (!StringHelpers.IsValidGender(organizerDto.Gender.Trim()))
                return ("Gender must be either 'Male' or 'Female'.", null);
            if (!Enum.IsDefined(typeof(UserType), organizerDto.UserType) || organizerDto.UserType != UserType.Organizer)
            {
                return ("Invalid user type or userType not organizer", null);
            }
            string formattedPhone = StringHelpers.FormatPhoneNumber(organizerDto.Phone.Trim());
            bool isValidPhone = StringHelpers.IsValidPhoneNumber(formattedPhone);
            if (!isValidPhone)
            {
                return (formattedPhone, null);
            }
            if (!StringHelpers.IsValidEmail(organizerDto.Email))
            {
                return ($"Invalid email {organizerDto.Email}", null);
            }
            var existingEmail = await _userManager.Users.FirstOrDefaultAsync(x=>x.Email == organizerDto.Email.ToString());
            if (existingEmail != null)
            {
                return ("Email is already taken.", null);
            }
            var user = new User 
            {
                UserName = organizerDto.UserName,
                Email = organizerDto.Email,
                PhoneNumber = formattedPhone,
                FirstName = organizerDto.FirstName,
                LastName = organizerDto.LastName,
                OtherName = organizerDto.OtherName,
                State = organizerDto.State,
                LGA = organizerDto.LGA,
                Address = organizerDto.Address,
                Gender = organizerDto.Gender,
                UserType = organizerDto.UserType
            };

            var userModel = await _userManager.CreateAsync(user, organizerDto.Password);
            if (!userModel.Succeeded)
            {
                var errors = string.Join(", ", userModel.Errors.Select(e => e.Description));
                return ($"Failed to create user: {errors}", null);
            }

            // Add role to the user
            var role = await _userManager.AddToRoleAsync(user, "ORGANIZER");
            if (!role.Succeeded)
            {
                var roleErrors = string.Join(", ", role.Errors.Select(e => e.Description));
                return ($"Failed to add Admin role: {roleErrors}", null);
            }
            var otp = new Otp
            {
                UserId = user.Id,
                Question = organizerDto.SecurityQuestion,
                Answer = organizerDto.SecurityAnswer,
                Token = _token.GenerateToken()
            };
            await _context.Otps.AddAsync(otp);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Your otp code is {otp.Token}");

            return ("User created successfully.", user);

        }

        public async Task<List<AdminListOrganizerDto>?> AdminListOrganizerAsync(AdminSearchQuery query)
        {
            var organizers =  _userManager.Users
            .Where(x=>x.UserType != UserType.Admin)
            .AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                organizers = organizers.Where(x=>x.FirstName.Contains(query.Search)
                || x.Email.Contains(query.Search)
                || x.UserName.Contains(query.Search)
                || x.Id.ToString().Contains(query.Search));
            };

            var organizersDto = await organizers.Select(x=>x.ToAdminListOrganizerDto()).ToListAsync();

            return organizersDto;

        }


        public async Task<AdminOrganizerDetailsDto?> AdminOrganizerDetailsAsync(string Id, HttpRequest request)
        {
            var organizer = await _userManager.Users
            .Include(x=>x.Events)
            .FirstOrDefaultAsync(x=>x.UserType != UserType.Admin && x.Id==Id);
            if (organizer == null)
                return null;
            var organizerDto = organizer.ToAdminOrganizerDetailsDto(request);

            return organizerDto;

        }

        public async Task<List<AdminOrganizerEventListDto>?> AdminListEventAsync(AdminListEventQuery query, HttpRequest request)
        {
            var events = _context.Events
            .OrderByDescending(x=>x.CreatedAt)
            .AsQueryable();
            
            if(!string.IsNullOrWhiteSpace(query.Name))
            {
                events = events.Where(x=>x.Name.Contains(query.Name)
                || x.Id.ToString().Contains(query.Name));
            };
            if(!string.IsNullOrWhiteSpace(query.EventType))
            {
               events = events.Where(x=>x.EventType.ToString().Contains(query.EventType));
                
            };
            if (query.IsInvitationOnly)
            {
                events = events.Where(x => x.IsInvitationOnly);
            }

            if (query.HasPayment)
            {
                events = events.Where(x => x.HasPayment);
            }
            
            if(!string.IsNullOrWhiteSpace(query.Location))
            {
                events = events.Where(x=>x.Location.Contains(query.Location));
            };

            
            if (query.StartDate > DateTime.MinValue)
            {
                Console.WriteLine($"StartDate Filter: {query.StartDate}");
                events = events.Where(x => x.StartDate >= query.StartDate);
            }

            if (query.EndDate > DateTime.MinValue)
            {
                Console.WriteLine($"EndDate Filter: {query.EndDate}");
                events = events.Where(x => x.EndDate <= query.EndDate); // Use <= for end date comparison.
            }


            var eventDtos = events
                .Select(x => x.ToAdminOrganizerEventListDto(request));
            

            return await eventDtos.ToListAsync();

            
            
        }

        public async Task<(AdminEventDetailsDto? eventDetailsDto, bool IsSuccess)> AdminGetEventDetailsAsync(int id, HttpRequest request)
        {
            if (!await id.IsValidEventAdmin(_context))
                return (null, false);
            var existingEvent = await _context.Events
            .Include(x=>x.Sessions)
            .Include(x=>x.Reminders)
            .Include(x=>x.Organizer)
            .FirstOrDefaultAsync(x=>x.Id == id);

            var eventDetailsDto = existingEvent?.ToAdminEventDetailsDto(request);
            return (eventDetailsDto, true);

        }


        public async Task<(List<AdminEventAttendeeListDto>? attendeeListDto, bool IsSuccess)> AdminGetEventAttendeeDetailsAsync(int id,AdminSearchQuery query)
        {
            if (!await id.IsValidEventAdmin(_context))
                return (null, false);
            var attendees = _context.Attendees
            .Where(x=>x.EventId == id)
            .AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                attendees = attendees.Where(x=>x.FirstName.Contains(query.Search)
                || x.Email.Contains(query.Search)
                || x.PhoneNumber.Contains(query.Search)
                || x.RegisteredAt.ToString().Contains(query.Search));
            };

            var attendeesDto = attendees?.Select(x=>x.ToAdminEventAttendeeListDto()).ToList();

            return (attendeesDto, true);
        }

        public async Task<(List<AdminEventTicketListDto>? ticketListDto, bool IsSuccess)> AdminGetEventTicketDetailsAsync(int id,AdminSearchQuery query)
        {
            if (!await id.IsValidEventAdmin(_context))
                return (null, false);
            var tickets = _context.Tickets
            .Include(x=>x.Attendee)
            .Where(x=>x.Attendee.EventId == id)
            .AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                tickets = tickets.Where(x=>x.Attendee.Email.Contains(query.Search)
                || x.Attendee.FirstName.Contains(query.Search)
                // || x.createdAt.Contains(query.Search)
                );
            };

            var ticketsDto = tickets?.Select(x=>x.ToAdminEventTicketListDto()).ToList();

            return (ticketsDto, true);
            
        }

        public async Task<(List<AdminEventPaymentListDto>? paymentListDto, bool IsSuccess)> AdminGetEventPaymentDetailsAsync(int id,AdminSearchQuery query)
        {
            if (!await id.IsValidEventAdmin(_context))
                return (null, false);
            var payments = _context.Payments
            
            .Where(x=>x.EventId == id)
            .Include(x=>x.Ticket)
                .ThenInclude(x=>x.Attendee)
            .AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                payments = payments.Where(x=>x.Ticket.Attendee.Email.Contains(query.Search)
                || x.Amount.ToString().Contains(query.Search)
                || x.TransactionId.ToString().Contains(query.Search)
                || x.PaymentDate.ToString().Contains(query.Search));
            };

            var paymentsDto = payments?.Select(x=>x.ToAdminEventPaymentListDto()).ToList();

            return (paymentsDto, true);
        }

        public async Task<(List<AdminEventFeedbackListDto>? feedbackListDto, bool IsSuccess)> AdminGetEventRatingDetailsAsync(int id,AdminSearchQuery query)
        {
            if (!await id.IsValidEventAdmin(_context))
                return (null, false);
            var feedbacks = _context.Feedbacks
            .Where(x=>x.EventId == id)
            .AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                feedbacks = feedbacks.Where(x=>x.AttendeeEmail.Contains(query.Search)
                || x.Comments.Contains(query.Search)
                || x.SubmittedAt.ToString().Contains(query.Search));
            };

            var feedbacksDto = feedbacks?.Select(x=>x.ToAdminEventFeedbackListDto()).ToList();

            return (feedbacksDto, true);
        }


        public async Task<(List<AdminEventInvitationListDto>? invitationListDto, bool IsSuccess)> AdminGetEventIvDetailsAsync(int id,AdminSearchQuery query)
        {
            if (!await id.IsValidEventAdmin(_context))
                return (null, false);
            var invitations = _context.Invitations
            .Where(x=>x.EventId == id)
            .AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                invitations = invitations.Where(x =>
                    x.AttendeeEmail.Contains(query.Search) || 
                    // x.Status.Equals(query.Search) || 
                    x.SentAt.ToString().Contains(query.Search)
                );
            }


            var invitationDto = invitations?.Select(x=>x.ToAdminEventInvitationListDto()).ToList();

            return (invitationDto, true);

            
        }

        public async Task<List<AdminListAttendeeDto>?> AdminListEventsAttendeesAsync(AdminSearchQuery query)
        {
            var attendees =  _context.Attendees
            .ToList()
            .GroupBy(x => x.Email)
            .Select(g => g.First());
            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                attendees = attendees.Where(x=>x.FirstName.Contains(query.Search)
                || x.Email.Contains(query.Search)
                || x.LastName.Contains(query.Search)
                || x.Id.ToString().Contains(query.Search));
            };

            var attendeesDto = attendees.Select(x=>x.ToAdminListAttendeeDtoDto()).ToList();

            return attendeesDto;
        }

        public async Task<List<AdminListOrganizersWalletDto>?> AdminListOrganizersWalletAsync(AdminSearchQuery query)
        {
            var wallets =  _context.Wallets
            .Include(x=>x.User)
            .AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                wallets = wallets.Where(x=>x.User.FirstName.Contains(query.Search)
                || x.User.Email.Contains(query.Search)
                || x.User.LastName.Contains(query.Search)
                || x.Balance.ToString().Contains(query.Search));
            };

            var walletsDto = await wallets.Select(x=>x.ToAdminListOrganizersWalletDto()).ToListAsync();

            return walletsDto;
        }


        public async Task<List<AdminListTransactionDto>?> AdminListTransactionAsync(AdminTransactionQuery query)
        {
            var transactions = _context.Transactions
            .Include(x=>x.User)
            .OrderByDescending(x=>x.Date)
            .AsQueryable();
            
            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                transactions = transactions.Where(x=>x.User.UserName.Contains(query.Search)
                || x.User.Email.Contains(query.Search)
                || x.User.FirstName.Contains(query.Search)
                || x.User.LastName.Contains(query.Search)
                || x.Id.ToString().Contains(query.Search));
            };

            if (query.StartDate > DateTime.MinValue)
            {
                Console.WriteLine($"StartDate Filter: {query.StartDate}");
                transactions = transactions.Where(x => x.Date >= query.StartDate);
            }

            if (query.EndDate > DateTime.MinValue)
            {
                Console.WriteLine($"EndDate Filter: {query.EndDate}");
                transactions = transactions.Where(x => x.Date <= query.EndDate); // Use <= for end date comparison.
            }

            var transactionsDtos = transactions
                .Select(x => x.ToAdminListTransactionDto());
            

            return await transactionsDtos.ToListAsync();
        }



    
    }
}
