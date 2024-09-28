using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes;

public class ValidationActionFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var modelState = context.ModelState;
        if (modelState.IsValid) base.OnActionExecuting(context);
        else context.Result = new BadRequestObjectResult(modelState);
    }
}
