using Microsoft.AspNetCore.Mvc.Filters;

public class LogActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        Log("OnActionExecuting", actionContext.RouteData);

        base.OnActionExecuting(actionContext);
    }

    public override void OnActionExecuted(ActionExecutedContext actionContext)
    {
        Log("OnActionExecuted", actionContext.RouteData);

        base.OnActionExecuted(actionContext);
    }

    public override void OnResultExecuting(ResultExecutingContext actionContext)
    {
        Log("OnResultExecuting", actionContext.RouteData);

        base.OnResultExecuting(actionContext);
    }

    public override void OnResultExecuted(ResultExecutedContext actionContext)
    {
        Log("OnResultExecuted", actionContext.RouteData);

        base.OnResultExecuted(actionContext);
    }

    private void Log(string methodName, RouteData routeData)
    {
        var controllerName = routeData.Values["controller"];
        var actionName = routeData.Values["action"];
        var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);

        Console.WriteLine(message);
    }
}