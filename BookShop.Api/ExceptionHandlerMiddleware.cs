﻿using BookShop.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BookShop.Api
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            string result = string.Empty;
            Exception exception = null;
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
                        httpStatusCode = HttpStatusCode.NotFound;
                        result = nf.ToString ();
                        break;
                    case ValidationException v:
                        httpStatusCode = HttpStatusCode.BadRequest;
                        result = JsonConvert.SerializeObject(v.Errors);
                        break;
                    default:
                        exception = ex;
                        break;
                }
            }

            if (exception != null)
                throw exception;

            context.Response.StatusCode = (int)httpStatusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
