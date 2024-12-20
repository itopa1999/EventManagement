using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Helpers;
using backend.Models;

namespace backend.Interface
{
    public interface IAdminInterface
    {
        Task<BlockAccessDto> BlockAccessDtoAsync (HttpContext context);
        Task<(string message, bool IsSuccess)> BlockUnblockEventAccessDtoAsync (int eventId, string status);
        Task<(string message, bool IsSuccess)> BlockUnblockUserAccessDtoAsync (string Id, string status);
        Task<(string message, bool IsSuccess)> BlockUnblockEmailAccessDtoAsync (string Email, string status);
        Task<(string message, bool IsSuccess)> BlockUnblockWalletAccessDtoAsync (string Id, string status);
        Task<(string message, bool IsSuccess)> AllBlockUnblockEventAccessDtoAsync (string status);
        Task<(string message, bool IsSuccess)> AllBlockUnblockUserAccessDtoAsync (string status);
        Task<(string message, bool IsSuccess)> AllBlockUnblockEmailAccessDtoAsync (string status);
        Task<(string message, bool IsSuccess)> AllBlockUnblockWalletAccessDtoAsync (string status);
        Task<(string message, User? user)> CreateOrganizerAsync(CreateOrganizerDto organizerDto);
        Task<List<AdminListOrganizerDto>?> AdminListOrganizerAsync(AdminSearchQuery query);
        Task<List<AdminListAttendeeDto>?> AdminListEventsAttendeesAsync(AdminSearchQuery query);
        Task<List<AdminListOrganizersWalletDto>?> AdminListOrganizersWalletAsync(AdminSearchQuery query);
        Task<List<AdminListTransactionDto>?> AdminListTransactionAsync(AdminTransactionQuery query);
        Task<AdminOrganizerDetailsDto?> AdminOrganizerDetailsAsync(string Id, HttpRequest request);
        Task<List<AdminOrganizerEventListDto>?> AdminListEventAsync(AdminListEventQuery query,HttpRequest request);
        Task<(AdminEventDetailsDto? eventDetailsDto, bool IsSuccess)> AdminGetEventDetailsAsync(int id, HttpRequest request);
        Task<(List<AdminEventAttendeeListDto>? attendeeListDto, bool IsSuccess)> AdminGetEventAttendeeDetailsAsync (
            int eventId, AdminSearchQuery query);
        Task<(List<AdminEventTicketListDto>? ticketListDto, bool IsSuccess)> AdminGetEventTicketDetailsAsync (
            int eventId, AdminSearchQuery query);
        Task<(List<AdminEventPaymentListDto>? paymentListDto, bool IsSuccess)> AdminGetEventPaymentDetailsAsync (
            int eventId, AdminSearchQuery query);
        Task<(List<AdminEventFeedbackListDto>? feedbackListDto, bool IsSuccess)> AdminGetEventRatingDetailsAsync (
            int eventId, AdminSearchQuery query);
        
        Task<(List<AdminEventInvitationListDto>? invitationListDto, bool IsSuccess)> AdminGetEventIvDetailsAsync (
            int eventId, AdminSearchQuery query);
        
    }
}