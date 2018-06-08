using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaaby.Abstractions;

namespace Zaaby
{
    internal class WebApiResultFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null) return;
            if (context.Result is ObjectResult objectResult)
                context.Result = new JsonResult(new ZaabyDtoBase {Data = objectResult.Value, Status = Status.Success});
        }
    }
}