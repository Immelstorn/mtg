using System.Collections.Generic;

namespace Mtg.Models.DTO
{
    public class ChangeOrderRequest:GameRequest
    {
        public List<string> Order { get; set; }
    }
}