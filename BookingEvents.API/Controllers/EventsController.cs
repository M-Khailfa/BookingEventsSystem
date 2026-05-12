using BookingEvents.Core.DTOs.Event;
using BookingEvents.Core.Interfaces;
using BookingEvents.Core.Settings;
using BookingEvents.Infrastructure.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace BookingEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;
        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetAllEvents([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string search = "")
        {
            var response = new ApiResponse();

            if (pageSize <= 0 || pageNumber <= 0)
            {
                response = ApiResponse.BadRequest("pageSize and pageNumber must be greater than 0.");
                return BadRequest(response);
            }

            var events = await _eventsService.GetAllEventsAsync(pageSize, pageNumber, search);
            if (events.Items is null || !events.Items.Any())
            {
                response = ApiResponse.Success(events, "No events found.", HttpStatusCode.OK);
                return Ok(response);
            }

            response = ApiResponse.Success(events, "Events retrieved successfully.", HttpStatusCode.OK);
            return Ok(response);
        }

        [HttpGet("event/{id}")]
        public async Task<IActionResult> GetEventByIdAsync(int id)
        {
            var response = new ApiResponse();
            if (id <= 0)
            {
                response = ApiResponse.BadRequest("Id must be greater than 0.");
                return BadRequest(response);
            }
            var targetevent = await _eventsService.GetEventByIdAsync(id);
            if (targetevent is null)
            {
                response = ApiResponse.NotFound("Event not found.");
                return NotFound(response);
            }
            response = ApiResponse.Success(targetevent, "Event retrieved successfully.", HttpStatusCode.OK);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-event")]
        public async Task<IActionResult> CreateEventAsync([FromForm] EventDto eventDto)
        {
            var response = new ApiResponse();
            
            if (!ModelState.IsValid)
            {
                response = ApiResponse.BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList());
                return BadRequest(response);
            }

            var result = await _eventsService.CreateEventAsync(eventDto);
            if (!result.Succeeded)
            {
                response = ApiResponse.BadRequest(result.Message);
                return BadRequest(response);
            }
            response = ApiResponse.Success(null, result.Message, HttpStatusCode.Created);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-event/{id}")]
        public async Task<IActionResult> DeleteEventAsync(int id)
        {
            var response = new ApiResponse();

            if (id <= 0)
            {
                response = ApiResponse.BadRequest("Id must be greater than 0.");
                return BadRequest(response);
            }

            var result = await _eventsService.DeleteEventByIdAsync(id);
            if (!result.Succeeded)
            {
                response = ApiResponse.NotFound(result.Message);
                return NotFound(response);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-event/{id}")]
        public async Task<IActionResult> UpdateEventAsync(int id, [FromBody] EventDto eventDto)
        {
            var response = new ApiResponse();
            if (id <= 0)
            {
                response = ApiResponse.BadRequest("Id must be greater than 0.");
                return BadRequest(response);
            }
            if (!ModelState.IsValid)
            {
                response = ApiResponse.BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList());
                return BadRequest(response);
            }
            var result = await _eventsService.UpdateEventByIdAsync(id, eventDto);
            if (!result.Succeeded)
            {
                response = ApiResponse.NotFound(result.Message);
                return NotFound(response);
            }
            response = ApiResponse.Success(result, result.Message, HttpStatusCode.OK);
            return Ok(response);
        }

        [HttpGet("event/category/{category}")]
        public async Task<IActionResult> GetEventsByCategory(string category,[FromQuery] int pageSize = 3,[FromQuery] int pageNumber = 1)
        {
            var response = new ApiResponse();
            if (string.IsNullOrWhiteSpace(category))
            {
                response = ApiResponse.BadRequest("Category cannot be empty.");
                return BadRequest(response);
            }

            var result = await _eventsService.GetEventsByCategoryAsync(category, pageSize, pageNumber);

            if (!result.Items.Any())
            {
                response = ApiResponse.NotFound($"No events found for category '{category}'.");
                return NotFound(response);
            }
            response = ApiResponse.Success(result, $"Events for category '{category}' retrieved successfully.", HttpStatusCode.OK);
            return Ok(response);
        }

    }
}
