using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mtg.Models.CardModels
{
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string ImgLink { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public string Color { get; set; }
        public string CardType { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
    }
}