using Mtg.Models.Enums;

namespace Mtg.Models.DTO
{
    public class MoveToRequest: GameRequest
    {
        public string CardLinkId { get; set; }
        public WhereToMove Where { get; set; }
        public MoveFrom From { get; set; }
    }
}