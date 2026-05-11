using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.Entites
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public AppUser User { get; set; }

        public int EventID { get; set; }
        public Event Event { get; set; }
    }
}
