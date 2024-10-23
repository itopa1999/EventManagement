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
        public OrganizerController(
            DBContext context,
            IOrganizerInterfaces organizerRepo
        )
        {
            _context = context;
            _organizerRepo = organizerRepo;
        }


        [HttpGet("list/event")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<organizerEventsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ListEvents([FromQuery] OrganizerListEventQuery query){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var listEvent = await _organizerRepo.OrganizerEventsAsync(userId, query);
            if (listEvent == null)
            {
                return NoContent();
            }

            return StatusCode((int)HttpStatusCode.OK, listEvent);
            
        }





        [HttpPost("create/event")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, createEvent) = await _organizerRepo.CreateEventAsync(createEventDto,userId);
            if (createEvent == null)
            {
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }

        [HttpGet("event/details/{eventId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<OrganizerEventDetailsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventsDetails([FromRoute] int eventId){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (eventDetailsDto, IsSuccess) = await _organizerRepo.GetEventDetailsAsync(eventId,userId);
            if (!IsSuccess)
            {
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event with Id: {eventId} not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventDetailsDto);
        }


        [HttpPut("update/event/{eventId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDto updateEventDto,[FromRoute] int eventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, updateEvent) = await _organizerRepo.UpdateEventAsync(updateEventDto,eventId,userId);
            if (updateEvent == null)
            {
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }

        [HttpPut("update/event/session/{sessionId:int}/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
            
        }


        [HttpDelete("delete/event/session/{sessionId:int}/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("create/event/session/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }

        
        [HttpPut("update/event/Iv/{invitationId:int}/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
            
        }

        [HttpGet("event/Iv/details/{eventId:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(List<OrganizerInvitationListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventIvDetailsAsync([FromRoute] int eventId){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (eventIvDetailsDto, IsSuccess) = await _organizerRepo.GetEventIvDetailsAsync(eventId,userId);
            if (!IsSuccess)
            {
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event with Id: {eventId} not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventIvDetailsDto);
        }


        [HttpDelete("delete/event/invitation/{invitationId:int}/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }


        [HttpPost("create/event/Iv/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }

        [HttpPost("bulk/upload/event/Iv/{eventId}")]
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
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }
                return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
            }catch (InvalidInputException ex)
            {
                return BadRequest(new { error_description = ex.Message });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An error occurred while creating an admin.");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { ErrorDescription = $"{ex}" });
            }
        }


        [HttpPost("create/event/reminder/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }


        [HttpPut("update/event/reminder/{reminderId:int}/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
            
        }


        [HttpDelete("delete/event/reminder/{reminderId:int}/{eventId:int}")]
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
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }
        






        











    }
}