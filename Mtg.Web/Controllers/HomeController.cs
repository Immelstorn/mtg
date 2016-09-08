using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Mtg.Models.DTO;
using Mtg.Web.Utils;

namespace Mtg.Web.Controllers
{
    public class HomeController: Controller
    {
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var request = new GameRequest
            {
                PlayerId = id
            };
            var result = Network.MakePostRequest<List<GameDto>>("games/mygames", request);
            if(result.StatusCode != HttpStatusCode.OK)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            return View(result.Data.Result);
        }
    }
}