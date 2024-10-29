using backend.Dtos;
using backend.Helpers;
using backend.Models;

namespace backend.Interface
{
    public interface IAttendeeInterface
    {
        Task<(string message, Attendee? attendee)> RegisterAttendeeAsync (
            AttendeeRegisterDto attendeeRegisterDto, int eventId);
        Task<(string message, BuyTicketCondition ticketCondition)> AttendeeBuyTicketAsync (
            AttendeeBuyTicketDto AttendeeBuyTicketDto, int eventId, HttpRequest request);
        Task<(FlutterwaveResponseDto?, string Message, bool IsCompleted)> VerifyTicketPaymentAsync(string transaction_id);
        Task<(string message, bool IsSuccess)> VerifyTicketPaymentSettleAsync (
            FlutterwaveResponseDto responseDto, int eventId,string transaction_id);
        Task<(string message, bool IsSuccess)> ReConfirmTicketPaymentSettleAsync (
            ReBuyTicketDto reBuyTicketDto, int eventId);
        Task<(List<AttendeeTicketListDto>? ticketListDtos, string message)> AttendeeGetEventTicketDetailsAsync (
           AttendeeEmailDto emailDto, int eventId);
        Task<List<AttendeeEventsListDto>?> AttendeesEventsListAsync(AttendeeListEventQuery query);
        Task<(AttendeeEventDetailsDto? attendeeEventDetails, bool IsSuccess)> AttendeeGetEventDetailsAsync(int eventId);
        Task<(string message, bool IsSuccess)> AttendeeCreateFeedbacks (
            AttendeeCreateFeedback createFeedback, int eventId);
        Task<(string message, bool IsSuccess)> RefundTicketPayment (
            AttendeeEmailDto emailDto, int id);






















        // public string GenerateUuid()
        // {
        //     Guid guid = Guid.NewGuid();
        //     byte[] guidBytes = guid.ToByteArray();
        //     string base64String = Convert.ToBase64String(guidBytes);
        //     base64String = base64String.Replace("+", "-").Replace("/", "_");
        //     return base64String.Substring(0, 20);
        // }
    }
}