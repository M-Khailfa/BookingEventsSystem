using BookingEvents.Core.DTOs;
using BookingEvents.Core.DTOs.Event;
using BookingEvents.Core.Entites;
using BookingEvents.Core.Interfaces;
using BookingEvents.Core.Settings;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookingEvents.Infrastructure.Repos
{
    public class EventsService : IEventsService
    {
        private readonly AppDbContext _context;
        private readonly IImageService _imageService;
        public EventsService(AppDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }
        public async Task<StatusDto> CreateEventAsync(EventDto eventDto)
        {
            if(eventDto is null)
            {
                return new StatusDto { Succeeded = false, Message = "Event data is required." };
            }
            
            string imageUrl = null;

            try
            {
                imageUrl = await _imageService.UploadImageAsync(eventDto.Image);
                var newEvent = new Event
                {
                    Name = eventDto.Name,
                    Description = eventDto.Description,
                    Category = eventDto.Category,
                    Price = eventDto.Price,
                    Place = eventDto.Place,
                    Date = eventDto.Date,
                    ImageURL = imageUrl
                };
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();
                return new StatusDto { Succeeded = true, Message = "Event created successfully." };
            }
            catch (Exception ex)
            {
                if(imageUrl is not null)
                    await _imageService.DeleteImageAsync(imageUrl);

                return new StatusDto { Succeeded = false, Message = $"Failed to create event. {ex.Message}" };
            }
        }

        public async Task<StatusDto> DeleteEventByIdAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete is null)
            {
                return new StatusDto { Succeeded = false, Message = "Wrong Provided Id." };
            }

            try
            {
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new StatusDto { Succeeded = false, Message = $"Failed to delete event. {ex.Message}" };
            }

            try
            {
                await _imageService.DeleteImageAsync(eventToDelete.ImageURL);
            }
            catch
            {
                return new StatusDto { Succeeded = true, Message = "Event deleted successfully. But Image Didn't Deleted" };
            }
            return new StatusDto { Succeeded = true, Message = "Event deleted successfully." };
        }

        public async Task<PagedResult<ReturnedEventDto>> GetAllEventsAsync(int pageSize = 3, int pageNumber = 1, string search = "")
        {
            if (pageSize <= 0 || pageSize > 10) pageSize = 3;
            if (pageNumber <= 0) pageNumber = 1;

            var query = _context.Events.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(e =>
                    e.Name.ToLower().Contains(term) ||
                    e.Description.ToLower().Contains(term) ||
                    e.Category.ToLower().Contains(term) ||
                    e.Place.ToLower().Contains(term));
            }

            var totalCount = await query.CountAsync();

            var returnedEvents = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ReturnedEventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Category = e.Category,
                    Price = e.Price,
                    Place = e.Place,
                    Date = e.Date,
                    ImageURL = e.ImageURL,
                })
                .ToListAsync();

            var pagedResult = new PagedResult<ReturnedEventDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = returnedEvents
            };

            return pagedResult;
        }

        public async Task<PagedResult<ReturnedEventDto>> GetEventsByCategoryAsync(string category, int pageSize = 3, int pageNumber = 1)
        {
            if (pageSize <= 0 || pageSize > 10) pageSize = 3;
            if (pageNumber <= 0) pageNumber = 1;

            var query = _context.Events.AsNoTracking();

            if (!string.IsNullOrEmpty(category))
            {
                var term = category.Trim().ToLower();
                query = query.Where(e => e.Category.ToLower() == term);
            }

            var totalCount = await query.CountAsync();

            var returnedEvents = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ReturnedEventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Category = e.Category,
                    Price = e.Price,
                    Place = e.Place,
                    Date = e.Date,
                    ImageURL = e.ImageURL,
                })
                .ToListAsync();

            return new PagedResult<ReturnedEventDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = returnedEvents
            };
        }

        public async Task<ReturnedEventDto> GetEventByIdAsync(int id)
        {
            var result = await _context.Events.FindAsync(id);

            if (result is null)
            {
                return new ReturnedEventDto();
            }

            return new ReturnedEventDto
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Category = result.Category,
                Price = result.Price,
                Place = result.Place,
                Date = result.Date,
                ImageURL = result.ImageURL,
                Successed = true,
                Message = "Event retrieved successfully."
            };
        }

        public async Task<ReturnedEventDto> UpdateEventByIdAsync(int id, EventDto eventDto)
        {
            if(eventDto is null)
            {
                return new ReturnedEventDto { Successed = false, Message = "Event data is required." };
            }

            var eventToUpdate = await _context.Events.FindAsync(id);
            if (eventToUpdate is null)
            {
                return new ReturnedEventDto { Successed = false, Message = "Wrong Provided Id." };
            }
            try
            {
                eventToUpdate.Name = eventDto.Name;
                eventToUpdate.Description = eventDto.Description;
                eventToUpdate.Category = eventDto.Category;
                eventToUpdate.Price = eventDto.Price;
                eventToUpdate.Place = eventDto.Place;
                eventToUpdate.Date = eventDto.Date;
                if (eventDto.Image is not null)
                {
                    string newImageUrl = null;
                    try
                    {
                        newImageUrl = await _imageService.UploadImageAsync(eventDto.Image);
                        if (eventToUpdate.ImageURL is not null)
                        {
                            await _imageService.DeleteImageAsync(eventToUpdate.ImageURL);
                        }
                        eventToUpdate.ImageURL = newImageUrl;
                    }
                    catch
                    {
                        return new ReturnedEventDto { Successed = false, Message = "Failed to update event image." };
                    }
                }
                await _context.SaveChangesAsync();
                return new ReturnedEventDto { Id = eventToUpdate.Id, Successed = true, Message = "Event updated successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnedEventDto { Successed = false, Message = $"Failed to update event. {ex.Message}" };
            }
        }
    }
}
