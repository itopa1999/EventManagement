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
        public AdminController(
            DBContext context,
            UserManager<User> userManager,
            IAdminInterface adminRepo,
            EmailService emailSender,
            ILogger<AdminController> logger
        )
        {
            _context = context;
            _userManager = userManager;
            _adminRepo = adminRepo;
            _emailSender = emailSender;
            _logger = logger;
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

            return StatusCode((int)HttpStatusCode.OK, message);
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

            return StatusCode((int)HttpStatusCode.OK, message);
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

            return StatusCode((int)HttpStatusCode.OK, message);
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

            return StatusCode((int)HttpStatusCode.OK, message);
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

            return StatusCode((int)HttpStatusCode.OK, message);
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

            return StatusCode((int)HttpStatusCode.OK, message);
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

            return StatusCode((int)HttpStatusCode.OK, message);
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

            return StatusCode((int)HttpStatusCode.OK, message);
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
        public async Task<IActionResult> ListOrganizers(){
            var organizers = await _adminRepo.AdminListOrganizerAsync();
            return StatusCode((int)HttpStatusCode.OK, organizers);
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

            return StatusCode((int)HttpStatusCode.OK, eventDetailsDto);
        }



        [HttpGet("event/{eventId:int}/invitation/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventInvitationListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventIvDetailsAsync([FromRoute] int eventId){
            var (eventIvDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventIvDetailsAsync(eventId);
            if (!IsSuccess)
            {
                _logger.LogError($"Get Event Invitation:  Event with Id: {eventId} ");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventIvDetailsDto);
        }


        [HttpGet("event/{eventId:int}/attendees/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventAttendeeListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventAttendeeDetailsAsync([FromRoute] int eventId){
            var (eventAttendeeDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventAttendeeDetailsAsync(eventId);
            if (!IsSuccess)
            {
                _logger.LogError($"Get Event attendees:  Event with Id: {eventId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventAttendeeDetailsDto);
        }


        [HttpGet("event/{eventId:int}/tickets/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventTicketListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventTicketsDetailsAsync([FromRoute] int eventId){
            var (eventTicketDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventTicketDetailsAsync(eventId);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event tickets:  Event with Id: {eventId} ");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventTicketDetailsDto);
        }


        [HttpGet("event/{eventId:int}/payments/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventPaymentListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventPaymentsDetailsAsync([FromRoute] int eventId){
            var (eventPaymentsDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventPaymentDetailsAsync(eventId);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event payments:  Event with Id: {eventId} ");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventPaymentsDetailsDto);
        }


        [HttpGet("event/{eventId:int}/feedbacks/details")]
        [Authorize]
        [Authorize(Policy = "IsAdmin")]
        [ProducesResponseType(typeof(List<AdminEventFeedbackListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventFeedbackDetailsAsync([FromRoute] int eventId){
            var (eventFeedbacksDetailsDto, IsSuccess) = await _adminRepo.AdminGetEventRatingDetailsAsync(eventId);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event feedbacks:  Event with Id: {eventId} ");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventFeedbacksDetailsDto);
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


        

 
















    }
}