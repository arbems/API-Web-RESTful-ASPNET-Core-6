using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

/* Sample action filter */

namespace APIWebRESTful.Filters
{
    public class FilterAction : IActionFilter
    {
        private readonly ILogger<FilterAction> logger;

        public FilterAction(ILogger<FilterAction> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Después de ejecutarse acción.
            logger.LogInformation("Después de ejecutarse acción");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Antes de ejecutarse acción.
            logger.LogInformation("Antes de ejecutarse acción");
        }
    }
}
