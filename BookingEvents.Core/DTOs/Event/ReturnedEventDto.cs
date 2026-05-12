using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BookingEvents.Core.DTOs.Event
{
    public class ReturnedEventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string Place { get; set; }
        public DateTime Date { get; set; }
        public string ImageURL { get; set; }
        [JsonIgnore]
        public bool Succeeded { get; set; } = false;
        [JsonIgnore]
        public string Message { get; set; } = "";
    }
}
