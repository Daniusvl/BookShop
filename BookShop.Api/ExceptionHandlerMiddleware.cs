using BookShop.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                next.Invoke(context);
            }
            catch (Exception ex)
            {
                HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
                string result = string.Empty;

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
                    case ServiceNullException sn:
                        httpStatusCode = HttpStatusCode.InternalServerError;
                        
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
