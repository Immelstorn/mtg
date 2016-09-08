using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Mtg.Database.Models;
using Mtg.Models.CardModels;
using Mtg.Models.DTO;
using Mtg.Models.Enums;
using Mtg.Web.Utils;

namespace Mtg.Web.Controllers
{
    public class PlayController: Controller
    {
        public string UserId
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }
       
        [HttpPost]
        public dynamic CreateGame()
        {
            var request = new GameRequest
            {
                PlayerId = UserId
            };
            var result = Network.MakePostRequest<GameDto>("games/create", request);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
                Data = result.Data
            });
        }

        public ActionResult Start(Guid id)
        {
            var gameRequest = new GameRequest
            {
                GameId = id.ToString(),
                PlayerId = UserId
            };
            var result = Network.MakePostRequest<bool>("games/start", gameRequest);

            if(result.StatusCode != HttpStatusCode.OK || result.Data.Failed || !result.Data.Result)
            {
                return View("~/Views/Shared/Error.cshtml", result.Content);
            }
            return View("Index", id);
        }

        public ActionResult Invite(string id)
        {
            var request = new GameRequest
            {
                PlayerId = UserId,
                GameId = id
            };
            var result = Network.MakePostRequest<List<GameDto>>("games/invite", request);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return View("~/Views/Shared/Error.cshtml", result.Content);
            }
            return View("Index", Guid.Parse(id));
        }

        [HttpPost]
        public dynamic Delete(Guid id)
        {
            var request = new GameRequest
            {
                PlayerId = UserId,
                GameId = id.ToString()
            };
            var result = Network.MakePostRequest<bool>("games/delete", request);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;

            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
            });
        }

        [HttpPost]
        public dynamic GameInfo(string gameId)
        {
            var gameRequest = new GameRequest
            {
                GameId = gameId,
                PlayerId = UserId
            };
            var result = Network.MakePostRequest<Game>("games/info", gameRequest);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            return Json(new
            {

                Failed = failed,
                Error = failed ? result.Content : null,
                Data = result.Data
            });
        }

        [HttpPost]
        public dynamic ChangeLife(string gameId, bool increment)
        {
            var gameRequest = new ChangeLifeRequest
            {
                GameId = gameId,
                PlayerId = UserId,
                Increment = increment
            };
            var result = Network.MakePostRequest<bool>("games/changeLife", gameRequest);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
                Data = result.Data.Result
            });
        }

        [HttpPost]
        public dynamic Draw(string gameId)
        {
            var gameRequest = new GameRequest
            {
                GameId = gameId,
                PlayerId = UserId
            };
            var result = Network.MakePostRequest<CardLink>("cards/draw", gameRequest);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
                Data = new CardDto(result.Data.Result)
            });
        }
        
        [HttpPost]
        public dynamic CreateToken(string gameId, string name, string type, string[] color, string text, string power, string toughness)
        {
            var gameRequest = new CreateTokenRequest
            {
                GameId = gameId,
                PlayerId = UserId,
                Name = name,
                Type = type,
                Color = color,
                Text = text,
                Power = power,
                Toughness = toughness
            };
            var result = Network.MakePostRequest<CardLink>("cards/createToken", gameRequest);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
                Data = result.Data.Result
            });
        }

        [HttpPost]
        public dynamic ChangeOrder(string gameId, List<string> order)
        {
            order.Reverse();
            var gameRequest = new ChangeOrderRequest
            {
                GameId = gameId,
                PlayerId = UserId,
                Order = order
            };
            var result = Network.MakePostRequest<bool>("cards/ChangeOrder", gameRequest);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
            });
        }

        [HttpPost]
        public dynamic GetRandomHand(string gameId)
        {
            var gameRequest = new GameRequest
            {
                GameId = gameId,
                PlayerId = UserId
            };
            var result = Network.MakePostRequest<List<CardLink>>("cards/hand", gameRequest);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            var hand = failed ? null : result.Data.Result.Select(c => new CardDto(c)).ToList();
            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
                Data = hand
            });
        }

        [HttpPost]
        public dynamic MoveTo(string gameId, string cardLinkId, string where, string from)
        {
            var gameRequest = new MoveToRequest
            {
                GameId = gameId,
                PlayerId = UserId,
                CardLinkId = cardLinkId,
                Where = (WhereToMove)Enum.Parse(typeof(WhereToMove), @where),
                From = (MoveFrom)Enum.Parse(typeof(MoveFrom), from),
            };

            var result = Network.MakePostRequest<bool>("cards/moveTo", gameRequest);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
                Data = result.Data
            });
        } 
        
        [HttpPost]
        public dynamic CardPostitionChanged(string gameId, string cardLinkId, string top, string left)
        {
            var gameRequest = new CardPostitionChangedRequest
            {
                GameId = gameId,
                PlayerId = UserId,
                CardLinkId = cardLinkId,
                Top = top,
                Left = left
            };

            var result = Network.MakePostRequest<bool>("cards/cardPositionChanged", gameRequest);
            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;
            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
                Data = result.Data
            });
        }



        [HttpPost]
        public dynamic ViewCards(string gameId, string from, string count)
        {
            var gameRequest = new ViewCardsRequest
            {
                GameId = gameId,
                PlayerId = UserId,
                From = (ViewFrom)Enum.Parse(typeof(ViewFrom), from),
                Count = string.IsNullOrEmpty(count) ? 0 : int.Parse(count)
            };
            var result = Network.MakePostRequest<List<CardLink>>("cards/viewCards", gameRequest);
            var cards = result.Data.Result.Select(c => new CardDto(c)).ToList();

            var failed = result.StatusCode != HttpStatusCode.OK || result.Data.Failed;

            return Json(new
            {
                Failed = failed,
                Error = failed ? result.Content : null,
                Data = cards
            });
        }
    }
}