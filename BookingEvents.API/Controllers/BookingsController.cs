using BookingEvents.Core.DTOs.Booking;
using BookingEvents.Core.Entites;
using BookingEvents.Core.Interfaces;
using BookingEvents.Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BookingEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserBookings(string userId)
        {
            var response = new ApiResponse();
            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (requestingUserId != userId)
                return Forbid();

            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            if (!bookings.Succeeded)
            {
                response = ApiResponse.BadRequest(bookings.Message);
                return BadRequest(response);
            }
            response = ApiResponse.Success(bookings, $"Bookings for user '{userId}' retrieved successfully.", HttpStatusCode.OK);
            return Ok(response);
        }

        [HttpPost("book")]
        [Authorize]
        public async Task<IActionResult> CreateBookingAsync(BookingDto bookingDto)
        {
            var response = new ApiResponse();
            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _bookingService.CreateBookingAsync(bookingDto, requestingUserId);

            if(!result.Succeeded)
            {
                response = ApiResponse.BadRequest(result.Message);
                return BadRequest(response);
            }

            response = ApiResponse.Success(result, "Booking Done Successfully", HttpStatusCode.OK);
            return Ok(response);
        }

        [HttpDelete("Cancel/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> DeleteBookingAsync(int bookingId)
        {
            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var booking = await _bookingService.GetBookingByIdAsync(bookingId);

            if(booking is null)
            {
                var response = ApiResponse.NotFound("Booking not found.");
                return NotFound(response);
            }

            if (!isAdmin && booking.UserId != requestingUserId)
                return Forbid();

            await _bookingService.DeleteBookingAsync(bookingId);

            return NoContent();
        }
    }
}
