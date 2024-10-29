using System.ComponentModel.DataAnnotations;
using System.Net;
using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Models;
using backend.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace backend.Controllers
{
    [Route("attendee/api/")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        public readonly DBContext _context;
        public readonly IAttendeeInterface  _attendeeRepo;
        private readonly ILogger<AttendeeController> _logger;
        public AttendeeController(
            DBContext context,
            IAttendeeInterface attendeeRepo,
            ILogger<AttendeeController> logger
        )
        {
            _context = context;
            _attendeeRepo = attendeeRepo;
            _logger = logger;
        }


        [HttpPost("register/event/{eventId:int}/attendee")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateEvent([FromBody] AttendeeRegisterDto registerDto, [FromRoute] int eventId)
        {
            try
            {
                var (message, registerAttendee) = await _attendeeRepo.RegisterAttendeeAsync(registerDto,eventId);
                if (registerAttendee == null)
                {
                    _logger.LogError($"Register Event Attendee: An error occurred: {message} for {registerDto.Email}");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }
                _logger.LogInformation($"Register Event Attendee: {message} for {registerDto.Email}");
                return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
            }
            catch (InvalidInputException ex)
            {
                _logger.LogError($"Register Event Attendee: An error occurred: {ex.Message} for {registerDto.Email}");
                return BadRequest(new ErrorResponse { ErrorDescription = ex.Message });
            }
        }



        [HttpPost("buy/event/{eventId:int}/ticket")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BuyEventTicket([FromBody] AttendeeBuyTicketDto buyTicketDto, [FromRoute] int eventId)
        {
            try
            {
                var (message, ticketCondition) = await _attendeeRepo.AttendeeBuyTicketAsync(buyTicketDto,eventId,Request);
                if (ticketCondition == BuyTicketCondition.error)
                {
                    _logger.LogError($"Buy Event Ticket: Event with Id: {eventId} An error occurred: {message} for {buyTicketDto.Email}");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }else if(ticketCondition == BuyTicketCondition.create){
                _logger.LogInformation($"Buy Event Ticket: Event with Id: {eventId}_{message} for {buyTicketDto.Email}");
                return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
                }else{
                _logger.LogInformation($"Buy Event Ticket: Event with Id: {eventId}_{message} for {buyTicketDto.Email}");
                return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Buy Event Ticket: Event with Id: {eventId}_{ex.Message} for {buyTicketDto.Email}");
                return BadRequest(new ErrorResponse { ErrorDescription = ex.Message });
            }
        }



        [HttpGet("confirm/ticket/payment/{eventId:int}")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public  async Task<IActionResult> ConfirmTicketPaymentAsync([FromRoute] int eventId,[FromQuery] string transaction_id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try{
                var (verifiedDto,message, IsCompleted) = await _attendeeRepo.VerifyTicketPaymentAsync(transaction_id);
                if (!IsCompleted)
                {
                    _logger.LogError($"Confirm Buy Event Ticket: Event with Id: {eventId} An error occurred: {message}.");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }
                if (verifiedDto?.Status != null && verifiedDto.Status != "success" &&
                    verifiedDto?.Message == "Transaction fetched successfully")
                {
                    _logger.LogError($"Confirm Buy Event Ticket: Event with Id: {eventId} An error occurred: Payment verification failed : {transaction_id}.");
                    return BadRequest(new ErrorResponse { ErrorDescription = $"Payment verification failed {transaction_id}" });
                }

                var (result, IsSuccess) = await _attendeeRepo.VerifyTicketPaymentSettleAsync(verifiedDto, eventId,transaction_id);
                if (!IsSuccess)
                {
                    _logger.LogError($"Confirm Buy Event Ticket: Event with Id: {eventId} An error occurred: {result}");
                    return BadRequest(new ErrorResponse { ErrorDescription = result });
                }
                
                await transaction.CommitAsync();
                
                return StatusCode((int)HttpStatusCode.OK, new MessageResponse(result));
            }catch(Exception ex){
                await transaction.RollbackAsync();
                _logger.LogCritical($"Confirm Buy Event Ticket: Event with Id: {eventId} An error occurred: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { ErrorDescription = $"{ex.Message}" });
            
            }
        }


        [HttpPost("confirm/ticket/payment/{eventId:int}/settled")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public  async Task<IActionResult> ConfirmTicketPaymentSettledAsync([FromRoute] int eventId,[FromBody] ReBuyTicketDto reBuyTicketDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try{
                var (result, IsValid) = await _attendeeRepo.ReConfirmTicketPaymentSettleAsync(reBuyTicketDto, eventId);
                if (!IsValid)
                {
                    _logger.LogError($"Confirm Buy Event Ticket Settled: Event with Id: {eventId} An error occurred: {result}");
                    return BadRequest(new ErrorResponse { ErrorDescription = result });
                }
                var (verifiedDto,message, IsCompleted) = await _attendeeRepo.VerifyTicketPaymentAsync(reBuyTicketDto.TransactionId);
                if (!IsCompleted)
                {
                    _logger.LogError($"Confirm Buy Event Ticket Settled: Event with Id: {eventId} An error occurred: {message}");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }

                if (verifiedDto?.Status != null && verifiedDto.Status != "success" &&
                    verifiedDto?.Message == "Transaction fetched successfully")
                {
                    _logger.LogError($"Confirm Buy Event Ticket Settled: Event with Id: {eventId} An error occurred: Payment is not found for {reBuyTicketDto.TransactionId}");
                    return BadRequest(new ErrorResponse { ErrorDescription = $"Payment is not found for {reBuyTicketDto.TransactionId}" });
                }

                var (response, IsSuccess) = await _attendeeRepo.VerifyTicketPaymentSettleAsync(verifiedDto, eventId,reBuyTicketDto.TransactionId);
                if (!IsSuccess)
                {
                    return BadRequest(new ErrorResponse { ErrorDescription = response });
                }
                
                await transaction.CommitAsync();
                
                return StatusCode((int)HttpStatusCode.OK, new MessageResponse(response));
            }catch(Exception ex){
                await transaction.RollbackAsync();
                _logger.LogCritical($"Confirm Buy Event Ticket Settled: Event with Id: {eventId} An error occurred: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { ErrorDescription = $"{ex.Message}" });
            
            }
        }

        [HttpPost("attendee/event/{eventId:int}/tickets/details")]
        [ProducesResponseType(typeof(List<AttendeeTicketListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEventFeedbackDetailsAsync([FromRoute] int eventId, [FromBody] AttendeeEmailDto emailDto){
            var (eventFeedbacksDetailsDto, message) = await _attendeeRepo.AttendeeGetEventTicketDetailsAsync(emailDto,eventId);
            if (eventFeedbacksDetailsDto == null)
            {
                 _logger.LogError($"Attendee Get Event Ticket Details List:  Event with Id: {eventId}_{message} ");
                return BadRequest(new ErrorResponse { ErrorDescription = message});
            }

            return StatusCode((int)HttpStatusCode.OK, eventFeedbacksDetailsDto);
        }

        [HttpGet("event/{eventId:int}/details")]
        [ProducesResponseType(typeof(List<AttendeeEventDetailsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AttendeeGetEventsDetails([FromRoute] int eventId){
            var (eventDetailsDto, IsSuccess) = await _attendeeRepo.AttendeeGetEventDetailsAsync(eventId);
            if (!IsSuccess)
            {
                _logger.LogError($"Event Details: Event with Id: {eventId} not found");
                return BadRequest(new ErrorResponse { ErrorDescription = $"Event not found" });
            }

            return StatusCode((int)HttpStatusCode.OK, eventDetailsDto);
        }

        [HttpGet("list/event")]
        [ProducesResponseType(typeof(List<AttendeeEventsListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AttendeeListEvents([FromQuery] AttendeeListEventQuery query){
            var listEvent = await _attendeeRepo.AttendeesEventsListAsync(query);
            if (listEvent == null)
            {
                _logger.LogInformation($"List Event: NoContent");
                return NoContent();
            }
            
            return StatusCode((int)HttpStatusCode.OK, listEvent);
            
        }

        [HttpPost("create/event/{eventId:int}/feedback")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEventFeedback([FromBody] AttendeeCreateFeedback createFeedback, [FromRoute] int eventId)
        {
            try
            {
                var (message, createFeedbacks) = await _attendeeRepo.AttendeeCreateFeedbacks(createFeedback,eventId);
                if (!createFeedbacks)
                {
                    _logger.LogError($"create Event Feedbacks: An error occurred: {message} for {createFeedback.Email}");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }
                _logger.LogInformation($"create Event Feedbacks: {message} for {createFeedback.Email}");
                return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
            }
            catch (InvalidInputException ex)
            {
                _logger.LogCritical($"create Event Feedbacks: An error occurred: {ex.Message} for {createFeedback.Email}");
                return BadRequest(new ErrorResponse { ErrorDescription = ex.Message });
            }
        }

        [HttpPost("refund/ticket/{ticketId:int}/ticket")]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEventFeedback([FromBody] AttendeeEmailDto emailDto, [FromRoute] int ticketId)
        {
            var (message, IsSuccess) = await _attendeeRepo.RefundTicketPayment(emailDto, ticketId);
            if (!IsSuccess)
            {
                _logger.LogError($"Refund ticket payment: {message}");
                return BadRequest(new ErrorResponse { ErrorDescription = message });
            }

            return StatusCode((int)HttpStatusCode.OK, new MessageResponse(message));
        }





    





















    
        
    }
}