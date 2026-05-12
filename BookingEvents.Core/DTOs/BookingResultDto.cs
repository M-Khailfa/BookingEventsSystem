using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.DTOs
{
    public class BookingResultDto<T>
    {
        public bool Succeeded { get; set; } = false;
        public string Message { get; set; } = "";
        public T Data { get; set; }
    }
}
