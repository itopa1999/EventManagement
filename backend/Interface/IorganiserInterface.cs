using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Helpers;
using backend.Models;

namespace backend.Interface
{
    public interface IOrganizerInterfaces
    {
        Task<(string message, Event? createEvent)> CreateEventAsync (
            CreateEventDto createEventDto, string userId);
        Task<(string message, Event? UpdateEvent)> UpdateEventAsync (
            UpdateEventDto updateEventDto, 
            int id, string userId);
        Task<(OrganizerEventDetailsDto? eventDetailsDto, bool IsSuccess)> GetEventDetailsAsync(
            int id, string userId, HttpRequest request);
        Task<(string message, bool IsSuccess)> DeleteEventAsync (
            int id, string userId);
        Task<List<organizerEventsDto>?> OrganizerEventsAsync (
            string userId, 
            OrganizerListEventQuery query, HttpRequest request);
        Task<(string message, Session? session)> CreateEventSessionAsync (
            CreateEventSessionDto createEventSessionDto, 
            int id, string userId);
        Task<(string message, Session? session)> UpdateEventSessionAsync (
            UpdateEventSessionDto eventSessionDto,
             int id, int eventId, string userId);

        Task<(string message, bool IsSuccess)> DeleteEventSessionAsync (
            int id,int eventId, string userId);
        Task<(string message, Invitation? invitation)> CreateEventIvAsync (
            CreateEventIvDto createEventIvDto, 
            int id, string userId);
        Task<(string message, Invitation? invitation)> UpdateEventInvitationAsync (
            UpdateEventIvDto updateEventIvDto, 
            int id, int eventId, string userId);
        Task<(List<OrganizerInvitationListDto>? invitationListDto, bool IsSuccess)> GetEventIvDetailsAsync (
            int eventId, string userId);
        Task<(string message, bool IsSuccess)> DeleteEventIvAsync (
           int id, int eventId, string userId);
        Task<(string message, bool IsSuccess)> UploadBulkEventIvAsync (
            IFormFile file, int id, 
            string userId);
        Task<(string message, Reminder? reminder)> CreateReminderAsync (
            CreateEventReminderDto eventReminderDto,
             int id, string userId);
        Task<(string message, Reminder? reminder)> UpdateEventReminderAsync (
            UpdateEventReminderDto eventReminderDto,
             int id, int eventId, string userId);
        Task<(string message, bool IsSuccess)> DeleteEventReminderAsync (
            int id, int eventId, string userId);
        Task<(List<OrganizerAttendeeListDto>? attendeeListDto, bool IsSuccess)> GetEventAttendeeDetailsAsync (
            int eventId, string userId);
        Task<(List<OrganizerTicketListDto>? ticketListDto, bool IsSuccess)> GetEventTicketDetailsAsync (
            int eventId, string userId);
        Task<(List<OrganizerPaymentListDto>? paymentListDto, bool IsSuccess)> GetEventPaymentDetailsAsync (
            int eventId, string userId);
        Task<(List<OrganizerFeedbackListDto>? feedbackListDto, bool IsSuccess)> GetEventRatingDetailsAsync (
            int eventId, string userId);
        Task<(OrganizerWalletTransactionsDto?, bool IsSuccess)> GetWalletTransactionsAsync (string userId);
        Task<(List<OrganizerDevicesDto>?, bool IsSuccess)> GetDevicesListAsync(string userId);
        Task<(string message, bool IsSuccess)> DeleteDeviceAsync(string userId, int id);

            

        
    }
}