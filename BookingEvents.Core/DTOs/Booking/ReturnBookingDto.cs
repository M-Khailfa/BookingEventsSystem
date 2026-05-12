using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BookingEvents.Core.DTOs.Booking
{
    public class ReturnBookingDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        [JsonIgnore]
        public bool Succeeded { get; set; } = false;
        [JsonIgnore]
        public string Message { get; set; }
    }
}
