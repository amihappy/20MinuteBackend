using System;
using System.Net;

namespace _20MinuteBackend.API.Exceptions
{
    public abstract class ApiException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Message { get; }

        public ApiException()
        {
        }

        public ApiException(string message) : this(message, HttpStatusCode.BadRequest)
        {
        }

        public ApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            this.Message = message;
            this.HttpStatusCode = statusCode;
        }
    }
}
