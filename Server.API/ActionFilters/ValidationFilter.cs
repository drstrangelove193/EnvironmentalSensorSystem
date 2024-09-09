using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.API.ActionFilters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // First, check the model state for any validation errors from the body
        if (!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            return;
        }

        // Now, validate the deviceId from the route data
        if (context.ActionArguments.ContainsKey("deviceId"))
        {
            var deviceId = context.ActionArguments["deviceId"] as string;
            if (!Guid.TryParse(deviceId, out _))
            {
                context.Result = new BadRequestObjectResult("Invalid device ID format.");
                return;
            }
        }
        else
        {
            context.Result = new BadRequestObjectResult("Device ID is required.");
            return;
        }
    }
}

