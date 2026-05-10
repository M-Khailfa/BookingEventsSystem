using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.Settings
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public bool succeeded { get; set; }
        public string message { get; set; }
        public string errors { get; set; }
        public object data { get; set; }
    }
}
