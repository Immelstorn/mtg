using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Mtg.Api.Controllers
{
    public class CatchExceptionAttribute : ExceptionFilterAttribute 
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, context.Exception.Message);

            base.OnException(context);
        }
    }
}