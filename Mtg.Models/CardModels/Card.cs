using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Mtg.Models.CardModels
{
    public class Card
    {
        public string artist { get; set; }
        public double cmc { get; set; }
        public List<string> colors { get; set; }
        public string flavor { get; set; }
        public List<ForeignName> foreignNames { get; set; }
        [Key]
        public string id { get; set; }
        public string imageName { get; set; }
        public string layout { get; set; }
        public List<Legality> legalities { get; set; }
        public string manaCost { get; set; }
        public int multiverseid { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string originalText { get; set; }
        public string originalType { get; set; }
        public string power { get; set; }
        public List<string> printings { get; set; }
        public string rarity { get; set; }
        public List<string> subtypes { get; set; }
        public string text { get; set; }
        public string toughness { get; set; }
        public string type { get; set; }
        public List<string> types { get; set; }
        public List<Ruling> rulings { get; set; }
        public List<string> supertypes { get; set; }
        public List<int?> variations { get; set; }
        public bool? starter { get; set; }
        public string ImgLink { get; set; }

        [JsonIgnore] 
        [IgnoreDataMember] 
        public virtual Set Set { get; set; }
    }
}