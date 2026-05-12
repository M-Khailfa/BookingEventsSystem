using BookingEvents.Core.DTOs;
using BookingEvents.Core.DTOs.Booking;
using BookingEvents.Core.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResultDto<IEnumerable<ReturnBookingDto>>> GetUserBookingsAsync(string userId);
        Task<ReturnBookingDto> CreateBookingAsync(BookingDto bookingDto, string userId);
        Task<StatusDto> DeleteBookingAsync(int bookingId);
        Task<ReturnBookingDto> GetBookingByIdAsync(int bookingId);
    }
}
