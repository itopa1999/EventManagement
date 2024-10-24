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
                var (message, ticketCondition) = await _attendeeRepo.AttendeeBuyTicketAsync(buyTicketDto,eventId);
                if (ticketCondition == BuyTicketCondition.error)
                {
                    _logger.LogError($"Buy Event Ticket: Event with Id: {eventId} An error occurred: {message} for {buyTicketDto.Email}");
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }else if(ticketCondition == BuyTicketCondition.create){
                _logger.LogInformation($"Buy Event Ticket: Event with Id: {eventId}_{message} for {buyTicketDto.Email}");
                return StatusCode((int)HttpStatusCode.Created, new MessageResponse(message));
                }else{
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
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }
                if (verifiedDto?.Status != null && verifiedDto.Status != "success" &&
                    verifiedDto?.Message == "Transaction fetched successfully")
                {
                    return BadRequest(new ErrorResponse { ErrorDescription = $"Payment verification failed {transaction_id}" });
                }

                var (result, IsSuccess) = await _attendeeRepo.VerifyTicketPaymentSettleAsync(verifiedDto, eventId,transaction_id);
                if (!IsSuccess)
                {
                    return BadRequest(new ErrorResponse { ErrorDescription = result });
                }
                
                await transaction.CommitAsync();
                
                return StatusCode((int)HttpStatusCode.OK, new MessageResponse(result));
            }catch(Exception ex){
                await transaction.RollbackAsync();
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
                    return BadRequest(new ErrorResponse { ErrorDescription = result });
                }
                var (verifiedDto,message, IsCompleted) = await _attendeeRepo.VerifyTicketPaymentAsync(reBuyTicketDto.TransactionId);
                if (!IsCompleted)
                {
                    return BadRequest(new ErrorResponse { ErrorDescription = message });
                }

                if (verifiedDto?.Status != null && verifiedDto.Status != "success" &&
                    verifiedDto?.Message == "Transaction fetched successfully")
                {
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
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { ErrorDescription = $"{ex.Message}" });
            
            }
        }



    





















    
        
    }
}