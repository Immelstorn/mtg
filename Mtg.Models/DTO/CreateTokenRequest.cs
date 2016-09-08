namespace Mtg.Models.DTO
{
    public class CreateTokenRequest:GameRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string[] Color { get; set; }
        public string Text { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
    }
}