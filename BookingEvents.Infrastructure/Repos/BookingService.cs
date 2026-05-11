using BookingEvents.Core.DTOs.Event;
using BookingEvents.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Infrastructure.Repos
{
    public class BookingService : IBookingService
    {
        public Task<bool> CreateBookingAsync(int eventId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBookingAsync(int eventId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReturnedEventDto>> GetBookingsAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
