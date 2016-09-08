using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Mtg.Api.Helper;
using Mtg.Database.Models;
using Mtg.Models.CardModels;
using Mtg.Models.DTO;
using Mtg.Models.Enums;
using Mtg.Models.GameModels;
using RestSharp;

namespace Mtg.Api.Controllers
{
    [CustomFilter]
    [CatchException]
    public class CardsController: BaseController
    {
        [Route("api/cards/draw")]
        [HttpPost]
        public async Task<dynamic> Draw(GameRequest request)
        {
            Guid gameIdGuid;
            if(!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }

            var game = await _context.Games
                    .Where(g => g.Id == gameIdGuid)
                    .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer)
                    .Include(g => g.Top.Select(t => t.CardLink.Card.rulings))
                    .Include(g => g.HandCards)
                    .FirstOrDefaultAsync();
            if(game == null)
            {
                return GameNotFound(request.GameId);
            }

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if(player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            if(game.FirstPlayer != player && game.SecondPlayer != player)
            {
                return PlayerNotInGame(request.PlayerId, request.GameId);
            }

            CardLink cardLink;
            if(game.Top.Any())
            {
                var top = game.Top.OrderByDescending(t => t.Number).Take(1).First();
                cardLink = top.CardLink;
                Utils.FillOneImage(cardLink.Card);

                _context.Tops.Remove(top);
                await _context.SaveChangesAsync();
            }
            else
            {
                cardLink = new CardLink {Card = (await Utils.GetRandomCards(1, _context)).First()};
                _context.CardLinks.Add(cardLink);
                await _context.SaveChangesAsync();
                _context.SaveChanges();
            }

            game.HandCards.Add(new HandCard
            {
                CardLink = cardLink,
                Player = player
            });

            await _context.SaveChangesAsync();

            return Json(new Response<CardLink>
            {
                Result = cardLink
            });
        }

        [Route("api/cards/cardPositionChanged")]
        [HttpPost]
        public async Task<dynamic> CardPositionChanged(CardPostitionChangedRequest request)
        {
            Guid gameIdGuid;
            if(!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }
            var gameQuery = _context.Games
                    .Where(g => g.Id == gameIdGuid)
                    .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer)
                    .Include(g => g.BattlefieldCards);

            var game = await gameQuery.FirstOrDefaultAsync();
            if(game == null)
            {
                return GameNotFound(request.GameId);
            }

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if(player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            if(game.FirstPlayer != player && game.SecondPlayer != player)
            {
                return PlayerNotInGame(request.PlayerId, request.GameId);
            }

            Guid cardlinkGuid;
            if(!Guid.TryParse(request.CardLinkId, out cardlinkGuid))
            {
                return InvalidCardLinkId(request.CardLinkId);
            }

            var cardLink = await _context.CardLinks.Include(l => l.Token).Include(l => l.Card).FirstOrDefaultAsync(l => l.Id == cardlinkGuid);
            if(cardLink == null)
            {
                return CardNotFound(request.CardLinkId, MoveFrom.Battlefield);
            }

            double top, left;
            if(!double.TryParse(request.Top, out top) || !double.TryParse(request.Left, out left))
            {
                return Json(new Response<bool>
                {
                    Failed = true,
                    Result = false,
                    Error = "Card position is invalid"
                });
            }

            var bfCard = game.BattlefieldCards.FirstOrDefault(c => c.CardLink == cardLink);
            if(bfCard == null)
            {
                return CardNotFound(request.CardLinkId, MoveFrom.Battlefield);
            }
            bfCard.Top = top;
            bfCard.Left = left;

            await _context.SaveChangesAsync();
            return Json(new Response<bool>
            {
                Result = true
            });
        }


        [Route("api/cards/moveTo")]
        [HttpPost]
        public async Task<dynamic> MoveTo(MoveToRequest request)
        {
            Guid gameIdGuid;
            if(!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }

            var gameQuery = _context.Games
                    .Where(g => g.Id == gameIdGuid)
                    .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer);

            switch(request.Where)
            {
                case WhereToMove.Hand:
                    gameQuery = gameQuery.Include(g => g.HandCards);
                    break;
                case WhereToMove.Top:
                    gameQuery = gameQuery.Include(g => g.Top);
                    break;
                case WhereToMove.Bottom:
                    gameQuery = gameQuery.Include(g => g.Bottom);
                    break;
                case WhereToMove.Graveyard:
                    gameQuery = gameQuery.Include(g => g.Grave);
                    break;
                case WhereToMove.Exile:
                    gameQuery = gameQuery.Include(g => g.Exile);
                    break;
                case WhereToMove.Battlefield:
                    gameQuery = gameQuery.Include(g => g.BattlefieldCards);
                    break;
            }

            switch(request.From)
            {
                case MoveFrom.Hand:
                    gameQuery = gameQuery.Include(g => g.HandCards);
                    break;
                case MoveFrom.Top:
                    gameQuery = gameQuery.Include(g => g.Top);
                    break;
                case MoveFrom.Graveyard:
                    gameQuery = gameQuery.Include(g => g.Grave);
                    break;
                case MoveFrom.Exile:
                    gameQuery = gameQuery.Include(g => g.Exile);
                    break;
                case MoveFrom.Battlefield:
                    gameQuery = gameQuery.Include(g => g.BattlefieldCards);
                    break;
            }
            var game = await gameQuery.FirstOrDefaultAsync();
            if(game == null)
            {
                return GameNotFound(request.GameId);
            }

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if(player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            if(game.FirstPlayer != player && game.SecondPlayer != player)
            {
                return PlayerNotInGame(request.PlayerId, request.GameId);
            }

            Guid cardlinkGuid;
            if(!Guid.TryParse(request.CardLinkId, out cardlinkGuid))
            {
                return InvalidCardLinkId(request.CardLinkId);
            }

            var cardLink = await _context.CardLinks.Include(l => l.Token).Include(l => l.Card).FirstOrDefaultAsync(l => l.Id == cardlinkGuid);
            if(cardLink == null)
            {
                return CardNotFound(request.CardLinkId, MoveFrom.NA);
            }

            //if token moved anywhere it just disappears
            if(cardLink.Card == null && cardLink.Token != null)
            {
                var token = cardLink.Token;
                _context.CardLinks.Remove(cardLink);
                _context.Tokens.Remove(token);
                await _context.SaveChangesAsync();

                return Json(new Response<bool>
                {
                    Result = true
                });
            }

            int max;
            switch(request.Where)
            {
                case WhereToMove.Top:
                    max = game.Top.Any() ? game.Top.Max(t => t.Number) : 0;
                    game.Top.Add(new Top {CardLink = cardLink, Number = ++max});
                    break;
                case WhereToMove.Bottom:
                    max = game.Bottom.Any() ? game.Bottom.Max(t => t.Number) : 0;
                    game.Bottom.Add(new Bottom {CardLink = cardLink, Number = ++max});
                    break;
                case WhereToMove.Graveyard:
                    max = game.Grave.Any() ? game.Grave.Max(t => t.Number) : 0;
                    game.Grave.Add(new Grave {CardLink = cardLink, Number = ++max});
                    break;
                case WhereToMove.Exile:
                    max = game.Exile.Any() ? game.Exile.Max(t => t.Number) : 0;
                    game.Exile.Add(new Exile {CardLink = cardLink, Number = ++max});
                    break;
                case WhereToMove.Hand:
                    game.HandCards.Add(new HandCard {CardLink = cardLink, Player = player});
                    _context.Games.AddOrUpdate(game);
                    break;
                case WhereToMove.Battlefield:
                    game.BattlefieldCards.Add(new BattlefieldCard{CardLink = cardLink, Player = player});
                    _context.Games.AddOrUpdate(game);
                    break;
            }

            switch(request.From)
            {
                case MoveFrom.Hand:
                    var handCard = game.HandCards.FirstOrDefault(c => c.CardLink == cardLink && c.Player == player);
                    if(handCard == null)
                    {
                        return CardNotFound(request.CardLinkId, MoveFrom.Hand);
                    }
                    _context.HandCards.Remove(handCard);
                    break;
                case MoveFrom.Top:
                    var top = game.Top.FirstOrDefault(t => t.CardLink == cardLink);
                    if(top == null)
                    {
                        return CardNotFound(request.CardLinkId, MoveFrom.Top);
                    }
                    _context.Tops.Remove(game.Top.First(t => t.CardLink == cardLink));
                    break;
                case MoveFrom.Graveyard:
                    var grave = game.Grave.FirstOrDefault(g => g.CardLink == cardLink);
                    if(grave == null)
                    {
                        return CardNotFound(request.CardLinkId, MoveFrom.Graveyard);
                    }
                    _context.Graves.Remove(grave);
                    break;
                case MoveFrom.Exile:
                    var exile = game.Exile.FirstOrDefault(g => g.CardLink == cardLink);
                    if(exile == null)
                    {
                        return CardNotFound(request.CardLinkId, MoveFrom.Exile);
                    }
                    _context.Exiles.Remove(exile);
                    break;
                case MoveFrom.Battlefield:
                    var bf = game.BattlefieldCards.FirstOrDefault(b => b.CardLink == cardLink);
                    if(bf == null)
                    {
                        return CardNotFound(request.CardLinkId, MoveFrom.Battlefield);
                    }
                    _context.BattlefieldCards.Remove(bf);
                    break;
            }

            await _context.SaveChangesAsync();

            return Json(new Response<bool>
            {
                Result = true
            });
        }

        public async Task<dynamic> ViewCards(ViewCardsRequest request)
        {
            Guid gameIdGuid;
            if(!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }

            var gameQuery = _context.Games
                    .Where(g => g.Id == gameIdGuid)
                    .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer);

            switch(request.From)
            {
                case ViewFrom.Graveyard:
                    gameQuery = gameQuery.Include(g => g.Grave.Select(i => i.CardLink.Card.rulings));
                    break;
                case ViewFrom.Exile:
                    gameQuery = gameQuery.Include(g => g.Exile.Select(i => i.CardLink.Card.rulings));
                    break;
                case ViewFrom.TopOfTheDeck:
                    gameQuery = gameQuery.Include(g => g.Top.Select(i => i.CardLink.Card.rulings));
                    break;
                case ViewFrom.EnemyHand:
                    break;
            }

            var game = await gameQuery.FirstOrDefaultAsync();

            if(game == null)
            {
                return GameNotFound(request.GameId);
            }

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if (player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            if (game.FirstPlayer != player && game.SecondPlayer != player)
            {
                return PlayerNotInGame(request.PlayerId, request.GameId);
            }

            List<CardLink> cardLinks = new List<CardLink>();
            switch(request.From)
            {
                case ViewFrom.Graveyard:
                    cardLinks = game.Grave.Select(g => g.CardLink).ToList();
                    break;
                case ViewFrom.Exile:
                    cardLinks = game.Exile.Select(g => g.CardLink).ToList();
                    break;
                case ViewFrom.TopOfTheDeck:
                    if(game.Top.Count < request.Count)
                    {
                        var delta = request.Count - game.Top.Count;
                        var randomCards = await Utils.GetRandomCards(delta, _context);
                        var randomCardLinks = new List<CardLink>();

                        //save links to DB
                        foreach(var randomCard in randomCards)
                        {
                            var cardLink = new CardLink {Card = randomCard};
                            _context.CardLinks.Add(cardLink);
                            await _context.SaveChangesAsync();
                            _context.SaveChanges();
                            randomCardLinks.Add(cardLink);
                        }

                        foreach(var top in game.Top)
                        {
                            top.Number += delta;
                        }

                        for(int i = 0; i < delta; i++)
                        {
                            game.Top.Add(new Top
                            {
                                CardLink = randomCardLinks[i],
                                Number = i + 1
                            });
                        }
                        foreach(var t in game.Top)
                        {
                            _context.Tops.AddOrUpdate(t);
                        }
                        await _context.SaveChangesAsync();
                    }

                    cardLinks = game.Top.OrderByDescending(t => t.Number).Take(request.Count).OrderBy(t => t.Number).Select(g => g.CardLink).ToList();

                    break;
                case ViewFrom.EnemyHand:
                    break;
            }

            Utils.FillImages(cardLinks.Select(l=>l.Card));
            return Json(new Response<List<CardLink>>
            {
                Result = cardLinks
            });
        }

        [Route("api/cards/ChangeOrder")]
        [HttpPost]
        public async Task<dynamic> ChangeOrder(ChangeOrderRequest request)
        {
            Guid gameIdGuid;
            if (!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }

            var gameQuery = _context.Games
                    .Where(g => g.Id == gameIdGuid)
                    .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer)
                    .Include(g => g.Top.Select(t => t.CardLink.Card));

            var game = await gameQuery.FirstOrDefaultAsync();

            if (game == null)
            {
                return GameNotFound(request.GameId);
            }

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if (player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            if (game.FirstPlayer != player && game.SecondPlayer != player)
            {
                return PlayerNotInGame(request.PlayerId, request.GameId);
            }

            var max = game.Top.Max(t => t.Number);
            for(int i = 0; i < request.Order.Count; i++)
            {
                var top = game.Top.First(t => t.CardLink.Id.ToString() == request.Order[i]);
                top.Number = max - i;
            }
            await _context.SaveChangesAsync();

            return Json(new Response<bool>
            {
                Result = true
            });
        }

        [Route("api/cards/hand")]
        [HttpPost]
        public async Task<dynamic> GetHand(GameRequest request)
        {
            var randomCards = await Utils.GetRandomCards(Utils.HandSize, _context);
            var randomCardLinks = new List<CardLink>();

            //save links to DB
            foreach (var randomCard in randomCards)
            {
                var cardLink = new CardLink { Card = randomCard };
                _context.CardLinks.Add(cardLink);
                await _context.SaveChangesAsync();
                _context.SaveChanges();
                randomCardLinks.Add(cardLink);
            }

            return Json(new Response<List<CardLink>>
            {
                Result = randomCardLinks
            });
        }

        [Route("api/cards/createToken")]
        [HttpPost]
        public async Task<dynamic> CreateToken(CreateTokenRequest request)
        {

            Guid gameIdGuid;
            if (!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }

            var gameQuery = _context.Games
                    .Where(g => g.Id == gameIdGuid)
                    .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer);

            var game = await gameQuery.FirstOrDefaultAsync();

            if (game == null)
            {
                return GameNotFound(request.GameId);
            }

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if (player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            if (game.FirstPlayer != player && game.SecondPlayer != player)
            {
                return PlayerNotInGame(request.PlayerId, request.GameId);
            }

            var username = player.UserName;

            var colorForUrl = request.Color == null || request.Color.Length == 0 || string.IsNullOrEmpty(request.Color[0])
                    ? "c"
                    : request.Color.Length > 1
                            ? "m"
                            : request.Color[0] == "white"
                                    ? "w"
                                    : request.Color[0] == "blue"
                                            ? "u"
                                            : request.Color[0] == "black"
                                                    ? "b"
                                                    : request.Color[0] == "red"
                                                            ? "r"
                                                            : "g";

            var imgUrl = "http://www.shenafu.com/imgd/ymtc/" + Uri.EscapeDataString(request.Name) + "_by_" + Uri.EscapeDataString(username) + ".png";

            var client = new RestClient("http://www.shenafu.com/magic/cc.php?save=true&frame=modern");
            var restRequest = new RestRequest(Method.GET);
            restRequest.AddParameter("color", colorForUrl);
            restRequest.AddParameter("cardname", request.Name);
            restRequest.AddParameter("cardtype", "00001000000");
            restRequest.AddParameter("subtype", "Token " +request.Type);
            restRequest.AddParameter("powertoughness", request.Power + "/" + request.Toughness);
            restRequest.AddParameter("rulestext", request.Text);
            restRequest.AddParameter("creator",username);
            var result = client.Execute(restRequest);

            var existingToken = await _context.Tokens.Where(t => t.ImgLink.Equals(imgUrl)).FirstOrDefaultAsync();

            var token = existingToken ?? new Token
            {
                Color = colorForUrl,
                CardType = request.Type,
                name = request.Name,
                Power = request.Power,
                Toughness = request.Toughness,
                text = request.Text,
                ImgLink = imgUrl
            };

            var cardLink = new CardLink
            {
                Token = token,
            };

            _context.CardLinks.Add(cardLink);
            await _context.SaveChangesAsync();

            return Json(new Response<CardLink>
            {
                Result = cardLink
            });
        }

//        private async Task<List<Card>> GetRandomCards(int count)
//        {

        //            var randomCard = new Card();
        //            var set = new Set();
        //            while(randomCard.number == null || randomCard.rarity.Equals("Basic Land"))
        //            {
        //                var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("/sets-x"));
        //                var randomSet = files[random.Next(files.Count() - 1)];
        //                set = JsonConvert.DeserializeObject<Set>(File.ReadAllText(randomSet));
        //                randomCard = set.cards[random.Next(set.cards.Count - 1)];
        //            }
        //            randomCard.ImgLink = string.Format(LinkToImg, Language, set.magicCardsInfoCode ?? set.code, randomCard.number.ToLower());
        //            return randomCard;
//        }

//        [Route("api/cards/sets")]
//        [HttpGet]
//        public IEnumerable<string> GetSets()
//        {
//            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("/data"));
//            return files.Select(Path.GetFileNameWithoutExtension).Select(f=>f.Replace("_",""));
//        }

//        [Route("api/cards/clean")]
//        [HttpGet]
//        public void Clean()
//        {
//            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("/sets-x"));
//            var toDelete = new List<string>();
//            foreach (var file in files)
//            {
//                var set = JsonConvert.DeserializeObject<Set>(File.ReadAllText(file));
//                if (!(set.type.Equals("expansion") || set.type.Equals("core")))
//                {
//                    toDelete.Add(file);
//                }
//            }
//            foreach (var todel in toDelete)
//            {
//                File.Delete(todel);
//            }
//        }

//        [Route("api/cards/filldb")]
//        [HttpGet]
//        public void FillDb()
//        {
//            using(var context = new MtgDbContext())
//            {
//                var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("/sets-x"));
//                foreach (var file in files)
//                {
//                    var set = JsonConvert.DeserializeObject<Set>(File.ReadAllText(file));
//                    context.Sets.Add(set);
//                    context.SaveChanges();
//                }
//            }
//            
//        }
//        [Route("api/cards/fillNumbers")]
//        [HttpGet]
//        public void FillNumbers()
//        {
//            var template = "/query?q={0}+e%3A{1}%2Fen&v=card&s=cname";
//            using (var context = new MtgDbContext())
//            {
//                var client = new RestClient(@"http://magiccards.info/");
//                var counter = 0;
//                var saved = 0;
//                var cards = context.Cards.Where(c => c.number == null).Include(c => c.Set).ToList();
//                foreach (var card in cards)
//                {
//                    var url = string.Format(template, HttpUtility.UrlEncode(card.name), card.Set.magicCardsInfoCode);
//                    var request = new RestRequest(url);
//                    var response = client.Execute(request);
//                    var sharp = response.Content.IndexOf("<b>#");
//                    var space = response.Content.IndexOf(' ', sharp);
//                    var number = response.Content.Substring(sharp + 4, space - sharp - 4);
//                    int numberParsed;
//                    if(int.TryParse(number, out numberParsed))
//                    {
//                        card.number = number;
//                        context.Cards.AddOrUpdate(card);
//                        context.SaveChanges();
//                        saved++;
//                    }
//                    counter ++;
//                }
//            }
//        }

//
//        [Route("api/cards/testCodes")]
//        [HttpGet]
//        public dynamic TestCodes()
//        {
//            var cardlist = new List<Card>();
//            using(var context = new MtgDbContext())
//            {
//                foreach(var set in context.Sets.Include("cards.rulings"))
//                {
//                    try
//                    {
//                        var card = set.cards[0];
//                        card.ImgLink = string.Format(LinkToImg, Language, set.magicCardsInfoCode ?? set.code, card.number.ToLower());
//                        cardlist.Add(card);
//
//                    }
//                    catch(Exception e )
//                    {
//                        Console.WriteLine();
//                    }
//                }
//
//            }
//            var result =  Json(new Response<List<Card>>
//            {
//                Result = cardlist
//            });
//            return result;
//        }
    }
}