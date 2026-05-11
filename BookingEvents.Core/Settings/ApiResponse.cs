using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BookingEvents.Core.Settings
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public object Data { get; set; }

        public static ApiResponse Success(object data, string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
            => new() { Succeeded = true, Data = data, Message = message, StatusCode = statusCode };

        public static ApiResponse BadRequest(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new() { Succeeded = false, Errors = errors, StatusCode = statusCode };

        public static ApiResponse Unauthorized(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
            => new() { Succeeded = false, Message = "", Errors = errors, StatusCode = statusCode };

        public static ApiResponse Forbidden(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.Forbidden)
            => new() { Succeeded = false, Errors = errors, StatusCode = statusCode };

        public static ApiResponse Deleted(string message, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new() { Succeeded = true, Message = message, StatusCode = statusCode };

        public static ApiResponse Updated(string message, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new() { Succeeded = true, Message = message, StatusCode = statusCode };

        public static ApiResponse NotFound(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound)
            => new() { Succeeded = false, Message = message, StatusCode = statusCode };

    }
}
