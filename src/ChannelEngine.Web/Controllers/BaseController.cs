namespace ChannelEngine.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class BaseController : Controller
    {
        private readonly ILogger logger;

        public BaseController(ILogger logger) => this.logger = logger;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                logger.LogError(context.Exception, "An error occured while handling your request");
            }

            base.OnActionExecuted(context);
        }
    }
}
