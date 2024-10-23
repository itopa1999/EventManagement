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
            int id, string userId);
        Task<(string message, bool IsSuccess)> DeleteEventAsync (
            int id, string userId);
        Task<List<organizerEventsDto>?> OrganizerEventsAsync (
            string userId, 
            OrganizerListEventQuery query);
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
        Task<(List<OrganizerInvitationListDto>? invitationListDto, bool IsSuccess)> GetEventIvDetailsAsync (
            int eventId, string userId);
        Task<(List<OrganizerInvitationListDto>? invitationListDto, bool IsSuccess)> GetEventIvDetailsAsync (
            int eventId, string userId);
        Task<(List<OrganizerInvitationListDto>? invitationListDto, bool IsSuccess)> GetEventIvDetailsAsync (
            int eventId, string userId);
        Task<(List<OrganizerInvitationListDto>? invitationListDto, bool IsSuccess)> GetEventIvDetailsAsync (
            int eventId, string userId);

        
    }
}