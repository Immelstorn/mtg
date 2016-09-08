using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mtg.Models.CardModels
{
    public class Set
    {
        public string name { get; set; }
        public string code { get; set; }
        public string oldCode { get; set; }
        [Key]
        public string magicCardsInfoCode { get; set; }
        public string releaseDate { get; set; }
        public string border { get; set; }
        public string type { get; set; }
        public string block { get; set; }
        public List<object> booster { get; set; }
        public List<Card> cards { get; set; }
    }
}