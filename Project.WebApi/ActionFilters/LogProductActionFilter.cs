using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Project.WebApi.ActionFilters
{
    public class LogProductActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("Product ekleme işlemi başlıyor...");
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("Product ekleme işlemi tamamlandı.");
            base.OnActionExecuted(context);
        }
    }
}
