using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mtg.Models.CardModels;

namespace Mtg.Database.Models
{
    public class HandCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public CardLink CardLink { get; set; }
        public Player Player { get; set; }
    }
}