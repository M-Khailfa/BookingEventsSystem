using BookingEvents.Core.DTOs;
using BookingEvents.Core.DTOs.Booking;
using BookingEvents.Core.DTOs.Event;
using BookingEvents.Core.Entites;
using BookingEvents.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Infrastructure.Repos
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public BookingService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ReturnBookingDto> CreateBookingAsync(BookingDto bookingDto)
        {
            var returnBookingDto = new ReturnBookingDto();

            var eventToBook = await _context.Events.FindAsync(bookingDto.EventId);
            if (eventToBook is null)
            {
                returnBookingDto.Message = "Event not found.";
                return returnBookingDto;
            }

            var user = await _context.Users.FindAsync(bookingDto.UserId);
            if (user is null)
            {
                returnBookingDto.Message = "User not found.";
                return returnBookingDto;
            }

            bool alreadyBooked = await _context.Bookings
                .AnyAsync(b => b.EventID == bookingDto.EventId && b.UserID == bookingDto.UserId);
            if (alreadyBooked)
            {
                returnBookingDto.Message = "User has already booked this event.";
                return returnBookingDto;
            }

            var booking = new Booking
            {
                EventID = bookingDto.EventId,
                UserID = bookingDto.UserId
            };

            _context.Bookings.Add(booking);
            try
            {
                await _context.SaveChangesAsync();
                returnBookingDto.Succeeded = true;
                returnBookingDto.Message = "Booking created successfully.";
                returnBookingDto.Id = booking.Id;
                returnBookingDto.EventId = booking.EventID;
                returnBookingDto.UserId = booking.UserID;
            }
            catch (DbUpdateException)
            {
                returnBookingDto.Message = "Booking could not be completed. Please try again.";
            }

            return returnBookingDto;
        }
              
        public async Task<StatusDto> DeleteBookingAsync(int bookingId)
        {
            var statusDto = new StatusDto();

            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking is null)
            {
                statusDto.Message = "Booking not found.";
                return statusDto;
            }

            _context.Bookings.Remove(booking);

            try
            {
                await _context.SaveChangesAsync();
                statusDto.Succeeded = true;
                statusDto.Message = "Booking deleted successfully.";
            }
            catch (DbUpdateException)
            {
                statusDto.Message = "Booking could not be deleted. Please try again.";
            }

            return statusDto;
        }

        public async Task<ReturnBookingDto> GetBookingByIdAsync(int bookingId)
        {
            var returnBookingDto = new ReturnBookingDto();
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
            
            if (booking is null)
            {
                returnBookingDto.Message = "Booking not found.";
                return returnBookingDto;
            }

            returnBookingDto.Succeeded = true;
            returnBookingDto.Message = "Booking retrieved successfully.";
            returnBookingDto.Id = booking.Id;
            returnBookingDto.EventId = booking.EventID;
            returnBookingDto.UserId = booking.UserID;
            returnBookingDto.FullName = $"{booking.User.FirstName} {booking.User.LastName}";
            returnBookingDto.EventName = booking.Event.Name;
            returnBookingDto.EventDate = booking.Event.Date;
            returnBookingDto.Location = booking.Event.Place;

            return returnBookingDto;
        }

        public async Task<BookingResultDto<IEnumerable<ReturnBookingDto>>> GetUserBookingsAsync(string userId)
        {
            var resultDto = new BookingResultDto<IEnumerable<ReturnBookingDto>>();
            if (string.IsNullOrEmpty(userId))
            {
                resultDto.Message = "User Id is Empty!";
                return resultDto;
            }
            
            if(!await _context.Users.AnyAsync(u => u.Id == userId))
            {
                resultDto.Message = "Invalid User Id!";
                return resultDto;
            }

            var bookings = await _context.Bookings
                .Where(b => b.UserID == userId)
                .Select(b => new ReturnBookingDto
                {
                    Id = b.Id,
                    EventId = b.EventID,
                    UserId = b.UserID,
                    FullName = $"{b.User.FirstName} {b.User.LastName}",
                    EventName = b.Event.Name,
                    EventDate = b.Event.Date,
                    Location = b.Event.Place
                })
                .ToListAsync();

            resultDto.Succeeded = true;
            resultDto.Data = bookings;

            return resultDto;
        }
    }
}
