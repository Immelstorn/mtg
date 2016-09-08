using System;

namespace Mtg.Models.DTO
{
    public class GameRequest
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public DateTime RequestTime { get; set; }
        public string Token { get; set; }
    }
}