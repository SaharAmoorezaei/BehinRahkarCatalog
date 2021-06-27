
using Alamut.Abstractions.Structure;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BehinRahkar.Catalog.Api.Middlewares
{   
    public class JsonExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        public JsonExceptionFilter(IWebHostEnvironment hostingEnvironment)
        {
            _env = hostingEnvironment;
        }
        public void OnException(ExceptionContext context)
        {
            var error = new Result();
            if (context.Exception is ValidationException fve)
            {
                error = new Result
                {
                    Succeed = false,
                    StatusCode = 400,
                    Message = fve.Errors.FirstOrDefault()?.ErrorMessage
                };
            }
            if (context.Exception is DbUpdateConcurrencyException ef)
            {
                error = new Result
                {
                    Succeed = false,
                    StatusCode = 400,
                    Message = "The catalog being updates has already been updated by another user."
                };
            }
            else
            {
                error = new Result
                {
                    Succeed = false,
                    Message = context.Exception.Message,
                    StatusCode = 500,
                };
            }

            context.Result = new ObjectResult(error)
            {
                StatusCode = error.StatusCode
            };
        }
    }
}
