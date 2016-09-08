using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mtg.Models.CardModels;

namespace Mtg.Database.Models
{
    public class BattlefieldCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public CardLink CardLink { get; set; }
        public Player Player { get; set; }
        public int State { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
    }
}