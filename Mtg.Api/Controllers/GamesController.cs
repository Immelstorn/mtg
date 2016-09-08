using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Mtg.Api.Helper;
using Mtg.Database.Models;
using Mtg.Models.CardModels;
using Mtg.Models.DTO;

namespace Mtg.Api.Controllers
{
    [CustomFilter]
    [CatchException]
    public class GamesController : BaseController
    {
        [Route("api/games/create")]
        [HttpPost]
        public async Task<dynamic> Create(GameRequest request)
        {
            var id = Guid.NewGuid();
            var game = new Game {Id = id};
            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if(player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }
            game.FirstPlayer = player;
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return Json(new Response<GameDto>
            {
                Result = MakeDto(game, request.PlayerId)
            });
        }

        [Route("api/games/changeLife")]
        [HttpPost]
        public async Task<dynamic> ChangeLife(ChangeLifeRequest request)
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
                    .FirstOrDefaultAsync();

            if (game == null)
            {
                return GameNotFound(request.GameId);
            }

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if(player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            var delta = request.Increment ? 1 : -1;
            if(game.FirstPlayer == player)
            {
                game.FirstPlayerHp += delta;

            }
            else if(game.SecondPlayer == player)
            {
                game.SecondPlayerHp += delta;
            }

            await _context.SaveChangesAsync();
            return Json(new Response<bool>
            {
                Result = true
            });
        }

        [Route("api/games/start")]
        [HttpPost]
        public async Task<dynamic> Start(GameRequest request)
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
                    .FirstOrDefaultAsync();

            if(game == null)
            {
                return GameNotFound(request.GameId);
            }

            if(game.FirstPlayerReady)
            {
                return Json(new Response<bool>
                {
                    Result = true
                });
            }

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if(player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            if (game.FirstPlayer != player && game.SecondPlayer != player)
            {
                return PlayerNotInGame(request.PlayerId, request.GameId);
            }

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

            var handCards = randomCardLinks.Select(c => new HandCard
            {
                CardLink = c,
                Player = player
            });

            game.HandCards.AddRange(handCards);
            game.FirstPlayerReady = true;
            await _context.SaveChangesAsync();
            return Json(new Response<bool>
            {
                Result = true
            });
        }

        [Route("api/games/invite")]
        [HttpPost]
        public async Task<dynamic> Invite(GameRequest request)
        {
            Guid gameIdGuid;
            if(!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }
            var game = await _context.Games
                    .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer)
                    .Where(g => g.Id == gameIdGuid)
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

            if(game.SecondPlayer == null && game.FirstPlayer != player)
            {
                game.SecondPlayer = player;
                if(!game.SecondPlayerReady)
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

                    var handCards = randomCardLinks.Select(c => new HandCard
                    {
                        CardLink = c,
                        Player = player
                    });

                    game.HandCards.AddRange(handCards);
                    game.SecondPlayerReady = true;
                }

                await _context.SaveChangesAsync();
            }
            return Json(new Response<GameDto>
            {
                Result = MakeDto(game, request.PlayerId)
            });
        }

        [Route("api/games/delete")]
        [HttpPost]
        public async Task<dynamic> Delete(GameRequest request)
        {
            Guid gameIdGuid;
            if(!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }
            var game = await _context.Games
                    .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer)
                    .Where(g => g.Id == gameIdGuid)
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

            if(game.FirstPlayer == player)
            {
                game.FirstPlayer = null;
            }
            else if(game.SecondPlayer == player)
            {
                game.SecondPlayer = null;
            }
            else
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return Json(new Response<bool>
            {
                Result = true
            });
        }

        [Route("api/games/mygames")]
        [HttpPost]
        public async Task<dynamic> MyGames(GameRequest request)
        {
            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PlayerId);
            if(player == null)
            {
                return PlayerNotFound(request.PlayerId);
            }

            var games = _context.Games
                    .Where(g => g.FirstPlayer != null && g.FirstPlayer.Id == player.Id || g.SecondPlayer != null && g.SecondPlayer.Id == player.Id)
                    .Select(g => g)
                     .Include(g => g.FirstPlayer)
                    .Include(g => g.SecondPlayer)
                    .ToList();

            return Json(new Response<List<GameDto>>
            {
                Result = games.Select(g => MakeDto(g, request.PlayerId)).ToList()
            });
        }

        [Route("api/games/info")]
        [HttpPost]
        public async Task<dynamic> Info(GameRequest request)
        {
            Guid gameIdGuid;
            if(!Guid.TryParse(request.GameId, out gameIdGuid))
            {
                return InvalidGameId(request.GameId);
            }
            var game = await _context.Games
                    .Where(g => g.Id == gameIdGuid)
                    .Include(g => g.Grave.Select(t => t.CardLink.Card.rulings))
                    .Include(g => g.Exile.Select(t => t.CardLink.Card.rulings))
                    .Include(g => g.BattlefieldCards.Select(t => t.CardLink.Card.rulings))
                    .Include(g => g.HandCards.Select(t => t.CardLink.Card.rulings))
                    .Include(g => g.HandCards.Select(t => t.Player))
                    .Include(g=>g.FirstPlayer)
                    .Include(g=>g.SecondPlayer)
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
            game.MyCards = game.HandCards.Count(c => c.Player == player);
            game.EnemyCards = game.HandCards.Count(c => c.Player != player);
            game.HandCards = game.HandCards.Where(hc => hc.Player.Id == request.PlayerId).ToList();
            game.AmIFirstPlayer = game.FirstPlayer == player;
            game.BattlefieldCards = game.BattlefieldCards.Where(bc => bc.Player == player).ToList();

            Utils.FillImages(game.Grave.Select(g => g.CardLink.Card));
            Utils.FillImages(game.Exile.Select(g => g.CardLink.Card));
            Utils.FillImages(game.HandCards.Select(g => g.CardLink.Card));
            Utils.FillImages(game.BattlefieldCards.Select(g => g.CardLink.Card));

            return Json(new Response<Game>
            {
                Result = game
            });
        }

        #region Private
        private GameDto MakeDto(Game game, string currentUser)
        {
            var opponent = game.FirstPlayer.Id == currentUser
                    ? game.SecondPlayer
                    : game.SecondPlayer.Id == currentUser
                            ? game.FirstPlayer
                            : null;

            var dto = new GameDto
            {
                Id = game.Id,
                Created = game.Created,
                FirstPlayerReady = game.FirstPlayerReady,
                SecondPlayerReady = game.SecondPlayerReady,
                Opponent = opponent == null ? String.Empty : opponent.UserName
            };
            return dto;
        }
        #endregion
    }
}