using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Helpers.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                // Custom handling for known exceptions
                DbUpdateException => (int)HttpStatusCode.Conflict, // For DB unique constraint errors
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError, // Fallback for unknown exceptions
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                error = new
                {
                    message = ex.Message,
                    detail = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                }
            });
            
            return context.Response.WriteAsync(result);
        }
    }

}