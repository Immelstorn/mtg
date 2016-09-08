using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mtg.Models.CardModels;

namespace Mtg.Models.GameModels
{
    public class Stack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

//        public Card Card { get; set; }
        public CardLink CardLink { get; set; }
        public int Number { get; set; }
    }
}