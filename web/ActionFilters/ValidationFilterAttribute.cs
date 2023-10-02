using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.ActionFilters;

public class ValidationFilterAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        #region return BadRequest response if the Dto argument sent to the action is null 
        // these values are added by Routing Middleware after processing the incoming request
        var controllerName = context.RouteData.Values["controller"];
        var actionName = context.RouteData.Values["action"];

        // the default implemention of ToString() contains tha name of the type
        var actionArgument = context.ActionArguments.Values
            .SingleOrDefault(arg => arg.ToString().Contains("Dto"));

        if (actionArgument is null)
        {
            context.Result = new BadRequestObjectResult($"Object is null, Controller:{controllerName}, Action:{actionName}");
            return;
        }
        #endregion

        #region return UnprocessableEntity response if the data sent by client is invalid
        if (!context.ModelState.IsValid)
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        #endregion
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
