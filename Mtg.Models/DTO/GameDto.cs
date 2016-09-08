using System;

namespace Mtg.Models.DTO
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public bool FirstPlayerReady { get; set; }
        public bool SecondPlayerReady { get; set; }
        public string Opponent { get; set; }
    }
}