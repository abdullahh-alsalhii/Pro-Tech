using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProـTech_Web.Middlewares
{
    [AttributeUsage(AttributeTargets.Method)]

    public class IsLogin : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var qrLogin = context.HttpContext.Request.Query["login"];
            if (qrLogin != "true")
            {
                context.HttpContext.Response.Redirect("/Auth");
                return;
            }
            await next();

        }
    }
}
