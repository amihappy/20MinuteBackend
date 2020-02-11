using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using _20MinuteBackend.API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _20MinuteBackend.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch(Exception ex)
            {
                await this.HandleApiExceptionAsync(ex, context);
            }
        }

        public Task HandleApiExceptionAsync(Exception exception, HttpContext context)
        {
            string errorMessage = exception.Message;
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (exception is ApiException ex)
            {
                statusCode = ex.HttpStatusCode;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse =
                new ErrorResponse(statusCode, errorMessage);

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }

        private class ErrorResponse
        {
            public HttpStatusCode StatusCode { get; }

            public string Message { get; }

            public ErrorResponse(HttpStatusCode statusCode, string message)
            {
                this.StatusCode = statusCode;
                this.Message = message;
            }
        }
    }
}
