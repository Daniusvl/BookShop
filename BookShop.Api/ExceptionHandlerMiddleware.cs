using BookShop.Core.Exceptions;
using BookShop.Core.Models.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BookShop.Api
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                switch (ex)
                {
                    case NotFoundException nf:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        await context.Response.WriteAsync(
                            new ErrorResponse(nameof(NotFoundException), $"{nf.ParamName} with id - {nf.Id} not found").ToJson());
                        break;
                    case ValidationException v:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(
                            new ErrorResponse(nameof(ValidationException), JsonConvert.SerializeObject(v.Errors)).ToJson());
                        break;
                    case UnknownException u:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(
                            new ErrorResponse(nameof(UnknownException), u.Message).ToJson());
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        logger.LogError(ex.Message);
                        break;
                }
            }
        }
    }
}
