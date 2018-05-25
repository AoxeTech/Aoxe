using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Zaaby.Middleware
{
    internal class WebApiResultMiddleware : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.HttpContext.Request.Path.HasValue) return;
            if (context.HttpContext.Request.Path.Value.ToLower().IndexOf(".inside.") >= 0) return;
            switch (context.Result)
            {
                case FileContentResult _:
                case EmptyResult _:
                    return;
                case ObjectResult _:
                    var objectResult = context.Result as ObjectResult;
                    var settings = new JsonSerializerSettings
                    {
//                            ContractResolver = new NullToEmptyStringResolver(),
                        DateFormatString = "yyyy-MM-dd HH:mm"
                    };
                    context.Result = new JsonResult(new {data = objectResult.Value}, settings);
                    break;
            }
        }
    }
}