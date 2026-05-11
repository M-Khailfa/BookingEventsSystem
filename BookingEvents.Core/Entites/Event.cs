using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookingEvents.Core.Entites
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(20)]
        public string Category { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string ImageURL { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
