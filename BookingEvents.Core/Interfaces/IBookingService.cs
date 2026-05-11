using BookingEvents.Core.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<ReturnedEventDto>> GetBookingsAsync(string userId);
        Task<bool> CreateBookingAsync(int eventId, string userId);
        Task<bool> DeleteBookingAsync(int eventId, string userId);
    }
}
