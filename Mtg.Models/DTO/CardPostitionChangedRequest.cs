namespace Mtg.Models.DTO
{
    public class CardPostitionChangedRequest:GameRequest
    {
        public string CardLinkId { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
    }
}