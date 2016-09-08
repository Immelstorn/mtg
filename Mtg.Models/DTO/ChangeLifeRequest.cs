namespace Mtg.Models.DTO
{
    public class ChangeLifeRequest: GameRequest
    {
        public bool Increment { get; set; }
    }
}