using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Mtg.Models;
using Mtg.Models.DTO;

namespace Mtg.Api.Controllers
{
    public class CustomFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            var request = actionContext.ActionArguments["request"] as GameRequest;
            if(request == null || request.RequestTime.AddMinutes(1) < DateTime.Now.ToUniversalTime() || !Cryptography.ValidateRequest(request))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request");
            }
        }
    }
}