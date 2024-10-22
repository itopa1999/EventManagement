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

        // [HttpGet("event/details")]
        // [Authorize]
        // [Authorize(Policy = "IsOrganizer")]
        // [ProducesResponseType(typeof(List<organizerEventsDto>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.NoContent)]
        // public async Task<IActionResult> GetEventsDetails(){

        // }


        [HttpPut("update/event/{id:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDto updateEventDto,[FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, updateEvent) = await _organizerRepo.UpdateEventAsync(updateEventDto,id,userId);
            if (updateEvent == null)
            {
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
            
        }



        


        [HttpPost("create/event/session/{id:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEventSession([FromBody] CreateEventSessionDto createEventSessionDto,[FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, createEventSession) = await _organizerRepo.CreateEventSessionAsync(createEventSessionDto,id,userId);
            if (createEventSession == null)
            {
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }



        [HttpPost("create/event/Iv/{id:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEventIv([FromBody] CreateEventIvDto createEventIvDto,[FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, createEventIv) = await _organizerRepo.CreateEventIvAsync(createEventIvDto,id,userId);
            if (createEventIv == null)
            {
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }

        [HttpPost("bulk/upload/event/Iv/{id}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadBulkEventIv(IFormFile file,[FromRoute] int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var (message, IsSuccess) =await _organizerRepo.UploadBulkEventIvAsync(file, id, userId);
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


        [HttpPost("create/event/reminder/{id:int}")]
        [Authorize]
        [Authorize(Policy = "IsOrganizer")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEventReminder([FromBody] CreateEventReminderDto eventReminderDto,[FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (message, createEventReminder) = await _organizerRepo.CreateReminderAsync(eventReminderDto,id,userId);
            if (createEventReminder == null)
            {
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
        }



        











    }
}