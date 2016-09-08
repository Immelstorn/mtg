using Mtg.Models.Enums;

namespace Mtg.Models.DTO
{
    public class ViewCardsRequest: GameRequest
    {
        public ViewFrom From { get; set; }
        public int Count { get; set; }
    }
}