namespace Store.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;
    using System;

    /*
     * Packages:
     * Install-Package Microsoft.AspNetCore.Mvc
     * Install-Package Microsoft.AspNetCore.Mvc.Core
     */

    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                if (!context.ModelState.IsValid)
                {
                    var controller = context.Controller as Controller;

                    if (controller == null)
                    {
                        return;
                    }

                    var model = context
                        .ActionArguments
                        .FirstOrDefault(a => a.Key.ToLower().Contains("model"))
                        .Value;

                    if (model == null)
                    {
                        return;
                    }

                    context.Result = controller.View(model);
                }
            }
        }
    }
}
