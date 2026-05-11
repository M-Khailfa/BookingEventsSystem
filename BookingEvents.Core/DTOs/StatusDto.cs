using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.DTOs
{
    public class StatusDto
    {
        public bool Successed { get; set; } = false;
        public string Message { get; set; } = "";
    }
}
