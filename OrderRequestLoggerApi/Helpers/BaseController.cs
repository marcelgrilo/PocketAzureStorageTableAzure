﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace OrderRequestLoggerApi.Helpers
{
    public class BaseController : Controller
    {
        [NonAction]
        public UnprocessableObjectResult Unprocessable(ModelStateDictionary modelState)
        {
            return new UnprocessableObjectResult(modelState);
        }

        [NonAction]
        public ObjectResult Unprocessable(object value)
        {
            return new UnprocessableObjectResult(value);
        }
    }
}
