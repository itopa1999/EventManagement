using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Models;
using backend.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("admin/api/")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly DBContext _context;
        public readonly UserManager<User> _userManager;
        public readonly IAdminInterface _adminRepo;
        private readonly EmailService _emailSender;
        private readonly ILogger<AdminController> _logger;
        private readonly SpecialHelpers _specialHelpers;
        public AdminController(
            DBContext context,
            UserManager<User> userManager,
            IAdminInterface adminRepo,
            EmailService emailSender,
            ILogger<AdminController> logger,
            SpecialHelpers specialHelpers
        )
        {
            _context = context;
            _userManager = userManager;
            _adminRepo = adminRepo;
            _emailSender = emailSender;
            _logger = logger;
            _specialHelpers = specialHelpers;
        }

        
        [HttpPost("unblock/{email}/{status}/block/email/access")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EmailUnblockBlockAccess([FromRoute] string email, [FromRoute] string status){
            var (message, IsSuccess) = await _adminRepo.BlockUnblockEmailAccessDtoAsync(email,status);
            if (!IsSuccess)
            {
                _logger.LogError($"Email Unblock Block Access: Error: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("unblock/{eventId:int}/{status}/block/event/access")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EventUnblockBlockAccess([FromRoute] int eventId, [FromRoute] string status){
            var (message, IsSuccess) = await _adminRepo.BlockUnblockEventAccessDtoAsync(eventId,status);
            if (!IsSuccess)
            {
                _logger.LogError($"Event Unblock Block Access: Error: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("unblock/{id}/{status}/block/Organizer/access")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UserUnblockBlockAccess([FromRoute] string id, [FromRoute] string status){
            var (message, IsSuccess) = await _adminRepo.BlockUnblockUserAccessDtoAsync(id,status);
            if (!IsSuccess)
            {
                _logger.LogError($"Organizer Unblock Block Access: Error: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }



        [HttpPost("unblock/{id}/{status}/block/wallet/access")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> WalletUnblockBlockAccess([FromRoute] string id, [FromRoute] string status){
            var (message, IsSuccess) = await _adminRepo.BlockUnblockWalletAccessDtoAsync(id,status);
            if (!IsSuccess)
            {
                _logger.LogError($"Wallet Unblock Block Access: Error: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("unblock/{status}/block/all-email/access")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AllEmailUnblockBlockAccess([FromRoute] string status){
            var (message, IsSuccess) = await _adminRepo.AllBlockUnblockEmailAccessDtoAsync(status);
            if (!IsSuccess)
            {
                _logger.LogError($"All Email Unblock Block Access: Error: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("unblock/{status}/block/all-event/access")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AllEventUnblockBlockAccess([FromRoute] string status){
            var (message, IsSuccess) = await _adminRepo.AllBlockUnblockEventAccessDtoAsync(status);
            if (!IsSuccess)
            {
                _logger.LogError($"All Event Unblock Block Access: Error: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("unblock/{status}/block/all-wallet/access")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AllWalletUnblockBlockAccess([FromRoute] string status){
            var (message, IsSuccess) = await _adminRepo.AllBlockUnblockWalletAccessDtoAsync(status);
            if (!IsSuccess)
            {
                _logger.LogError($"All Wallet Unblock Block Access: Error: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("unblock/{status}/block/all-organizer/access")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AllOrganizerUnblockBlockAccess([FromRoute] string status){
            var (message, IsSuccess) = await _adminRepo.AllBlockUnblockUserAccessDtoAsync(status);
            if (!IsSuccess)
            {
                _logger.LogError($"All Organizer Unblock Block Access: Error: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("create/organizer")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateOrganizer([FromBody] CreateOrganizerDto organizerDto)
        {
            try
            {
                var (message, user) = await _adminRepo.CreateOrganizerAsync(organizerDto);
                if (user == null)
                {
                    _logger.LogError($"Create Organizer: An error occurred: {message}");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }

                return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
            }catch (InvalidInputException ex)
            {
                _logger.LogError($"Create Organizer: An error occurred: {ex.Message}");
                return BadRequest(new { error_description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create Organizer: An error occurred: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { ErrorDescription = $"{ex}" });
            }
        }

        [HttpGet("list/organizers")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminListOrganizerDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListOrganizers([FromQuery] AdminSearchQuery query){
            var organizers = await _adminRepo.AdminListOrganizerAsync(query);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var paginatedOrganizers = organizers.Skip(skipNumber).Take(query.PageSize).ToList();
            return StatusCode((int)HttpStatusCode.OK, new {
                organizers = paginatedOrganizers,
                organizers_count = paginatedOrganizers.Count(),
                pagesize = query.PageSize

            } );
        }


        [HttpGet("organizer/{organizerId}/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(AdminOrganizerDetailsDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> OrganizerDetails([FromRoute] string organizerId){
            var organizer = await _adminRepo.AdminOrganizerDetailsAsync(organizerId, Request);
            if (organizer == null)
                return StatusCode((int)HttpStatusCode.NoContent, new { error_description = "No Content" });

            return StatusCode((int)HttpStatusCode.OK, organizer);
        }


        [HttpGet("list/events")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminOrganizerEventListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AdminListEvent([FromQuery] AdminListEventQuery query){
            var events = await _adminRepo.AdminListEventAsync(query, Request);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var paginatedEvents = events.Skip(skipNumber).Take(query.PageSize).ToList();
            return StatusCode((int)HttpStatusCode.OK, new {
                events = paginatedEvents,
                events_count = paginatedEvents.Count(),
                pagesize = query.PageSize

            } );

        }





        [HttpGet("event/{eventId:int}/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(AdminEventDetailsDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AdminGetEventsDetails([FromRoute] int eventId){
            var (eventDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventDetailsAsync(eventId, Request);
            if (!IsSuccess)
            {
                _logger.LogError($"Event Details: Event with Id: {eventId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            int payments_count = await _context.Payments.Where(x=>x.EventId==eventId).CountAsync();
            int reminders_count = await _context.Reminders.Where(x=>x.EventId==eventId).CountAsync();
            int sessions_count = await _context.Sessions.Where(x=>x.EventId==eventId).CountAsync();
            int attendees_count = await _context.Attendees.Where(x=>x.EventId==eventId).CountAsync();
            int invitations_count = await _context.Invitations.Where(x=>x.EventId==eventId).CountAsync();
            int tickets_count = await _context.Tickets.Where(x=>x.Attendee.EventId==eventId).CountAsync();
            int feedbacks_count = await _context.Feedbacks.Where(x=>x.EventId==eventId).CountAsync();

            return StatusCode((int)HttpStatusCode.OK, new {
                event_details = eventDetailsDto,
                payments_count = payments_count,
                reminders_count= reminders_count,
                sessions_count = sessions_count,
                attendees_count = attendees_count,
                invitations_count = invitations_count,
                tickets_count = tickets_count,
                feedbacks_count = feedbacks_count
                });
                
        }


        [HttpGet("list/attendees")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminListAttendeeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AttendeesList([FromQuery] AdminSearchQuery query){
            var attendees = await _adminRepo.AdminListEventsAttendeesAsync(query);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var paginatedAttendees = attendees.Skip(skipNumber).Take(query.PageSize).ToList();
            return StatusCode((int)HttpStatusCode.OK, new {
                attendees = paginatedAttendees,
                attendees_count = paginatedAttendees.Count(),
                pagesize = query.PageSize

            } );
        }


        [HttpGet("list/organizers/wallet")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminListAttendeeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ListOrganizersWallet([FromQuery] AdminSearchQuery query){
            var wallets = await _adminRepo.AdminListOrganizersWalletAsync(query);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var paginatedWallets = wallets.Skip(skipNumber).Take(query.PageSize).ToList();
            return StatusCode((int)HttpStatusCode.OK, new {
                wallets = paginatedWallets,
                wallets_count = paginatedWallets.Count(),
                pagesize = query.PageSize

            } );
        }



        [HttpGet("event/{eventId:int}/invitation/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventInvitationListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventIvDetailsAsync([FromRoute] int eventId,[FromQuery] AdminSearchQuery query){
            var (eventIvDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventIvDetailsAsync(eventId, query);
            if (!IsSuccess)
            {
                _logger.LogError($"Get Event Invitation:  Event with Id: {eventId} ");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var invitations = eventIvDetailsDto.Skip(skipNumber).Take(query.PageSize).ToList();

            return StatusCode((int)HttpStatusCode.OK, new {
                invitations = invitations,
                invitations_count = invitations.Count(),
                pagesize = query.PageSize

            } );
        }


        [HttpGet("event/{eventId:int}/attendees/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventAttendeeListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventAttendeeDetailsAsync([FromRoute] int eventId,[FromQuery] AdminSearchQuery query){
            var (eventAttendeeDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventAttendeeDetailsAsync(eventId,query);
            if (!IsSuccess)
            {
                _logger.LogError($"Get Event attendees:  Event with Id: {eventId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var attendees = eventAttendeeDetailsDto.Skip(skipNumber).Take(query.PageSize).ToList();

            return StatusCode((int)HttpStatusCode.OK, new {
                attendees = attendees,
                attendees_count = eventAttendeeDetailsDto.Count(),
                pagesize = query.PageSize

            } );
        }


        [HttpGet("event/{eventId:int}/tickets/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventTicketListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventTicketsDetailsAsync([FromRoute] int eventId,[FromQuery] AdminSearchQuery query){
            var (eventTicketDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventTicketDetailsAsync(eventId, query);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event tickets:  Event with Id: {eventId} ");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var tickets = eventTicketDetailsDto.Skip(skipNumber).Take(query.PageSize).ToList();

            return StatusCode((int)HttpStatusCode.OK, new {
                tickets = tickets,
                tickets_count = tickets.Count(),
                pagesize = query.PageSize

            } );
        }


        [HttpGet("event/{eventId:int}/payments/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventPaymentListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventPaymentsDetailsAsync([FromRoute] int eventId,[FromQuery] AdminSearchQuery query){
            var (eventPaymentsDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventPaymentDetailsAsync(eventId, query);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event payments:  Event with Id: {eventId} ");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var payments = eventPaymentsDetailsDto.Skip(skipNumber).Take(query.PageSize).ToList();

            return StatusCode((int)HttpStatusCode.OK, new {
                payments = payments,
                payments_count = payments.Count(),
                pagesize = query.PageSize

            } );
        }


        [HttpGet("event/{eventId:int}/feedbacks/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventFeedbackListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventFeedbackDetailsAsync([FromRoute] int eventId,[FromQuery] AdminSearchQuery query){
            var (eventFeedbacksDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventRatingDetailsAsync(eventId, query);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event feedbacks:  Event with Id: {eventId} ");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var feedbacks = eventFeedbacksDetailsDto.Skip(skipNumber).Take(query.PageSize).ToList();

            return StatusCode((int)HttpStatusCode.OK, new {
                feedbacks = feedbacks,
                feedbacks_count = feedbacks.Count(),
                pagesize = query.PageSize

            } );
        }


        [HttpGet("wallet/balance")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWalletBalance(){
            var balance = await _context.Wallets
            .Where(x=>x.User.UserType != UserType.Admin)
            .SumAsync(x=>x.Balance);
            return StatusCode((int)HttpStatusCode.OK, new {balance = balance}); 
        }


        [HttpGet("dashboard")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminDashboardDataDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdminDashboard([FromQuery] MonthlyDashboardQuery query){
            
            var currentTime = TimeHelper.GetNigeriaTime();
            int tEvent = await _context.Events.CountAsync();
            int tSession = await _context.Sessions.CountAsync();
            int tActiveEvent =  _context.Events
                                .Where(e => e.StartDate.Date >= currentTime.Date)
                                .Count();
            int tActiveSession =  _context.Sessions
                                .Where(e => e.StartTime.Date >= currentTime.Date)
                                .Count();
            int tUser = await _userManager.Users.Where(x=>x.UserType == UserType.Organizer).CountAsync();
            int tAttendee = await _context.Attendees.CountAsync();
            decimal tTicketSold = 30000;
            var userRegistrations = await _specialHelpers.GetMonthlyUserRegistrationsAsync(query.UserYear);
            var EventRegistrations = await _specialHelpers.GetMonthlyEventRegistrationsAsync(query.EventYear);
            var SessionRegistrations = await _specialHelpers.GetMonthlySessionRegistrationsAsync(query.SessionYear);
            var dashboardData = new AdminDashboardDataDto
            {
                TotalEvent = tEvent,
                TotalSession = tSession,
                TotalActiveEvent = tActiveEvent,
                TotalActiveSession = tActiveSession,
                TotalOrganizer = tUser,
                TotalAttendee = tAttendee,
                TotalTicketSold = tTicketSold,
                MonthlyUserRegistrations = userRegistrations,
                MonthlyEventRegistrations = EventRegistrations,
                MonthlySessionRegistrations = SessionRegistrations
            };

            return StatusCode((int)HttpStatusCode.OK, dashboardData); 
            
        }


        [HttpGet("list/transactions")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListTransactions([FromQuery] AdminTransactionQuery query){
           var transactions = await _adminRepo.AdminListTransactionAsync(query);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var paginatedTransactions = transactions.Skip(skipNumber).Take(query.PageSize).ToList();
            return StatusCode((int)HttpStatusCode.OK, new {
                transactions = paginatedTransactions,
                transactions_count = paginatedTransactions.Count(),
                pagesize = query.PageSize

            } );
        }



        

 
















    }
}