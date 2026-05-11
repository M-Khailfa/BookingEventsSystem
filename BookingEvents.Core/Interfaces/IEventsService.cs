using BookingEvents.Core.DTOs;
using BookingEvents.Core.DTOs.Event;
using BookingEvents.Core.Entites;
using BookingEvents.Core.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.Interfaces
{
    public interface IEventsService
    {
        Task<PagedResult<ReturnedEventDto>> GetAllEventsAsync(int pageSize = 3, int pageNumber = 1,string search = "");
        Task<PagedResult<ReturnedEventDto>> GetEventsByCategoryAsync(string category, int pageSize = 3, int pageNumber = 1);
        Task<ReturnedEventDto> GetEventByIdAsync(int id);
        Task<StatusDto> CreateEventAsync(EventDto eventDto);
        Task<StatusDto> DeleteEventByIdAsync(int id);
        Task<ReturnedEventDto> UpdateEventByIdAsync(int id, EventDto eventDto);
    }
}
