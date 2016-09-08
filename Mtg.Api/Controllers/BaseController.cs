using System.Web.Http;
using Mtg.Database;
using Mtg.Models.DTO;
using Mtg.Models.Enums;

namespace Mtg.Api.Controllers
{
    public class BaseController: ApiController
    {
        protected readonly MtgDbContext _context;

        public BaseController()
        {
            _context = new MtgDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        protected dynamic PlayerNotFound(string playerId)
        {
            return Json(new Response<object>
            {
                Failed = true,
                Error = "Player with id " + playerId + " not found"
            });
        }

        protected dynamic PlayerNotInGame(string playerId, string gameId)
        {
            return Json(new Response<object>
            {
                Failed = true,
                Error = "Player " + playerId + " is not in game " + gameId
            });
        }

        protected dynamic GameNotFound(string gameId)
        {
            return Json(new Response<object>
            {
                Failed = true,
                Error = "Game with id " + gameId + " not found"
            });
        } 
        
        protected dynamic CardNotFound(string cardId, MoveFrom from)
        {
            string where;
            switch(from)
            {
                case MoveFrom.Hand:
                    where = " in hand.";
                    break;
                case MoveFrom.Exile:
                    where = " on exile.";
                    break;
                case MoveFrom.Graveyard:
                    where = " on graveyard.";
                    break;
                case MoveFrom.Top:
                    where = " on top of the deck.";
                    break;
                case MoveFrom.Battlefield:
                    where = " on the battlefield.";
                    break;
                default:
                    where = ".";
                    break;
            }


            return Json(new Response<object>
            {
                Failed = true,
                Error = "Card with id " + cardId + "not found" + where
            });
        }

        protected dynamic InvalidGameId(string gameId)
        {
            return Json(new Response<object>
            {
                Failed = true,
                Error = "Game ID " + gameId + " is invalid"
            });
        }

        protected dynamic InvalidCardLinkId(string cardLinkId)
        {
            return Json(new Response<object>
            {
                Failed = true,
                Error = "CardLink ID " + cardLinkId + " is invalid"
            });
        }
    }
}