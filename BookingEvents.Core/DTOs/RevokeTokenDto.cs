using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingEvents.Core.DTOs
{
    public class RevokeTokenDto
    {
        public string? token { get; set; }
    }
}
