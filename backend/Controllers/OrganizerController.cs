using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("organizer/api/")]
    [ApiController]
    public class OrganizerController : ControllerBase
    {
        public readonly DBContext _context;
        public readonly IOrganizerInterfaces _organizerRepo;
        private readonly ILogger<OrganizerController> _logger;
        public OrganizerController(
            DBContext context,
            IOrganizerInterfaces organizerRepo,
            ILogger<OrganizerController> logger
        )
        {
            _context = context;
            _organizerRepo = organizerRepo;
            _logger = logger;
        }


        [HttpGet("list/event")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<organizerEventsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ListEvents([FromQuery] OrganizerListEventQuery query){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var listEvent = await _organizerRepo.OrganizerEventsAsync(userId, query, Request);
            if (listEvent == null)
            {
                _logger.LogInformation($"List Event: NoContent for userId: {userId}");
                return NoContent();
            }
            
            return StatusCode((int)HttpStatusCode.OK, listEvent);
            
        }





        [HttpPost("create/event")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto createEventDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, createEvent) = await _organizerRepo.CreateEventAsync(createEventDto,userId);
            if (createEvent == null)
            {
                _logger.LogError($"Create Event: An error occurred: {message} for userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }
            _logger.LogInformation($"Create Event: {message}: for userId: {userId}");
            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }

        [HttpGet("event/{eventId:int}/details")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<OrganizerEventDetailsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventsDetails([FromRoute] int eventId){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (eventDetailsDto, IsSuccess) = await _organizerRepo.GetEventDetailsAsync(eventId,userId, Request);
            if (!IsSuccess)
            {
                _logger.LogError($"Event Details: Event with Id: {eventId} not found or userId : {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventDetailsDto);
        }


        [HttpPut("update/event/{eventId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventDto updateEventDto,[FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, updateEvent) = await _organizerRepo.UpdateEventAsync(updateEventDto,eventId,userId);
            if (updateEvent == null)
            {
                _logger.LogError($"Update Event: Event with Id: {eventId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }
            _logger.LogInformation($"Update Event: Event with Id: {eventId}_{message} : userId: {userId}");
            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
            
        }


        [HttpDelete("delete/event/{eventId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteEvent([FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, IsSuccess) = await _organizerRepo.DeleteEventAsync(eventId,userId);
            if (!IsSuccess)
            {
                _logger.LogError($"Delete Event: Event with Id: {eventId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }

        [HttpPut("update/event/{eventId:int}/session/{sessionId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateEventSession([FromBody] UpdateEventSessionDto eventSessionDto,[FromRoute] int sessionId, [FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, updateEventSession) = await _organizerRepo.UpdateEventSessionAsync(eventSessionDto,sessionId,eventId,userId??"dfdfdff");
            if (updateEventSession == null)
            {
                _logger.LogError($"Update Event Session:  Event with Id: {eventId}, SessionId: {sessionId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }
            _logger.LogInformation($"Update Event Session:  Event with Id: {eventId}, SessionId: {sessionId}_{message} : userId: {userId}");
            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
            
        }


        [HttpDelete("delete/event/{eventId:int}/session/{sessionId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteEventSession([FromRoute] int sessionId, [FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, IsSuccess) = await _organizerRepo.DeleteEventSessionAsync(sessionId,eventId,userId);
            if (!IsSuccess)
            {
                _logger.LogError($"Delete Event Session:  Event with Id: {eventId}, SessionId: {sessionId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("create/event/{eventId:int}/session")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEventSession([FromBody] CreateEventSessionDto createEventSessionDto,[FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, createEventSession) = await _organizerRepo.CreateEventSessionAsync(createEventSessionDto,eventId,userId);
            if (createEventSession == null)
            {
                _logger.LogError($"Create Event Session:  Event with Id: {eventId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }
             _logger.LogInformation($"Create Event Session:  Event with Id: {eventId}, SessionId: {createEventSession?.Id}_{message} : userId: {userId}");
            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }

        
        [HttpPut("update/event/{eventId:int}/invitation/{invitationId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateEventInvitation([FromBody] UpdateEventIvDto updateEventIvDto,[FromRoute] int invitationId, [FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, updateEventIv) = await _organizerRepo.UpdateEventInvitationAsync(updateEventIvDto,invitationId,eventId,userId);
            if (updateEventIv == null)
            {
                _logger.LogError($"Update Event Invitation:  Event with Id: {eventId}, invitationId: {invitationId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }   
            _logger.LogInformation($"Update Event Invitation:  Event with Id: {eventId}, invitationId: {invitationId}_{message} : userId: {userId}");
            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
            
        }

        [HttpGet("event/{eventId:int}/invitation/details")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<OrganizerInvitationListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventIvDetailsAsync([FromRoute] int eventId){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (eventIvDetailsDto, IsSuccess) = await _organizerRepo.GetEventIvDetailsAsync(eventId,userId);
            if (!IsSuccess)
            {
                _logger.LogError($"Get Event Invitation:  Event with Id: {eventId} not found : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventIvDetailsDto);
        }


        [HttpDelete("delete/event/{eventId:int}/invitation/{invitationId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteEventIv([FromRoute] int invitationId, [FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, IsSuccess) = await _organizerRepo.DeleteEventIvAsync(invitationId,eventId,userId);
            if (!IsSuccess)
            {
                _logger.LogError($"Delete Event invitation:  Event with Id: {eventId}, invitationId: {invitationId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("create/event/{eventId:int}/invitation")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEventIv([FromBody] CreateEventIvDto createEventIvDto,[FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, createEventIv) = await _organizerRepo.CreateEventIvAsync(createEventIvDto,eventId,userId);
            if (createEventIv == null)
            {
                _logger.LogError($"Create Event invitation:  Event with Id: {eventId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }
            _logger.LogInformation($"Create Event invitation:  Event with Id: {eventId}, invitation: {createEventIv.Id}_{message} : userId: {userId}");
            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }

        [HttpPost("bulk/upload/event/{eventId:int}/invitation")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadBulkEventIv(IFormFile file,[FromRoute] int eventId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var (message, IsSuccess) =await _organizerRepo.UploadBulkEventIvAsync(file, eventId, userId);
                if (!IsSuccess)
                {
                    _logger.LogError($"Bulk Create Event invitation:  Event with Id: {eventId}_{message} : userId: {userId}");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }
                _logger.LogInformation($"Bulk Create Event invitation:  Event with Id: {eventId}_{message} : userId: {userId}");
                return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
            }catch (InvalidInputException ex)
            {
                 _logger.LogCritical($"Bulk Create Event invitation:  Event with Id: {eventId}_{ex.Message}");
                return BadRequest(new { error_description = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Bulk Create Event invitation:  Event with Id: {eventId}_{ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { ErrorDescription = $"{ex}" });
            }
        }


        [HttpPost("create/event/{eventId:int}/reminder")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEventReminder([FromBody] CreateEventReminderDto eventReminderDto,[FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, createEventReminder) = await _organizerRepo.CreateReminderAsync(eventReminderDto,eventId,userId);
            if (createEventReminder == null)
            {
                _logger.LogError($"Create Event reminder:  Event with Id: {eventId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }
            _logger.LogInformation($"Create Event reminder:  Event with Id: {eventId}, reminderId: {createEventReminder.Id}_{message} : userId: {userId}");
            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }


        [HttpPut("update/event/{eventId:int}/reminder/{reminderId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateEventReminder([FromBody] UpdateEventReminderDto eventReminderDto,[FromRoute] int reminderId, [FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, updateEventReminder) = await _organizerRepo.UpdateEventReminderAsync(eventReminderDto,reminderId,eventId,userId);
            if (updateEventReminder == null)
            {
                _logger.LogError($"Update Event reminder:  Event with Id: {eventId}, reminderId: {reminderId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }
            _logger.LogInformation($"Update Event reminder:  Event with Id: {eventId}, reminderId: {reminderId}_{message} : userId: {userId}");
            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
            
        }


        [HttpDelete("delete/event/{eventId:int}/reminder/{reminderId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteEventReminder([FromRoute] int reminderId, [FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, IsSuccess) = await _organizerRepo.DeleteEventReminderAsync(reminderId,eventId,userId);
            if (!IsSuccess)
            {
                _logger.LogError($"Delete Event reminder:  Event with Id: {eventId}, reminderId: {reminderId}_{message} : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpGet("event/{eventId:int}/attendees/details")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<OrganizerAttendeeListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventAttendeeDetailsAsync([FromRoute] int eventId){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (eventAttendeeDetailsDto, IsSuccess) = await _organizerRepo.GetEventAttendeeDetailsAsync(eventId,userId);
            if (!IsSuccess)
            {
                _logger.LogError($"Get Event attendees:  Event with Id: {eventId} not found : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventAttendeeDetailsDto);
        }


        [HttpGet("event/{eventId:int}/tickets/details")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<OrganizerTicketListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventTicketsDetailsAsync([FromRoute] int eventId){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (eventTicketDetailsDto, IsSuccess) = await _organizerRepo.GetEventTicketDetailsAsync(eventId,userId);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event tickets:  Event with Id: {eventId} not found : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventTicketDetailsDto);
        }


        [HttpGet("event/{eventId:int}/payments/details")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<OrganizerPaymentListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventPaymentsDetailsAsync([FromRoute] int eventId){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (eventPaymentsDetailsDto, IsSuccess) = await _organizerRepo.GetEventPaymentDetailsAsync(eventId,userId);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event payments:  Event with Id: {eventId} not found : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventPaymentsDetailsDto);
        }


        [HttpGet("event/{eventId:int}/feedbacks/details")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<OrganizerFeedbackListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventFeedbackDetailsAsync([FromRoute] int eventId){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (eventFeedbacksDetailsDto, IsSuccess) = await _organizerRepo.GetEventRatingDetailsAsync(eventId,userId);
            if (!IsSuccess)
            {
                 _logger.LogError($"Get Event feedbacks:  Event with Id: {eventId} not found : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventFeedbacksDetailsDto);
        }


        [HttpGet("wallet/balance/transactions")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(OrganizerWalletTransactionsDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetWalletBalanceTransactionsAsync(){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (walletTransactionsDto, IsSuccess) = await _organizerRepo.GetWalletTransactionsAsync(userId);
            if (!IsSuccess)
            {
                _logger.LogError($"Get Balance: cannot get balance not found or created : userId: {userId}");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Wallet not found for user, please contact administrator" });
            }

            return StatusCode((int)HttpStatusCode.OK, walletTransactionsDto);
        }

        






        











    }
}