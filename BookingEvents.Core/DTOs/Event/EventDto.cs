using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.DTOs.Event
{
    public class EventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string Place { get; set; }
        public DateTime Date { get; set; }
        public IFormFile Image { get; set; }
    }
}
