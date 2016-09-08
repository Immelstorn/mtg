using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mtg.Models.GameModels;

namespace Mtg.Database.Models
{
    public class Game
    {
        public Game()
        {
            Top = new List<Top>();
            Bottom = new List<Bottom>();
            Grave = new List<Grave>();
            Exile = new List<Exile>();
            HandCards = new List<HandCard>();
            BattlefieldCards = new List<BattlefieldCard>();
            FirstPlayerHp = SecondPlayerHp = 20;
            MyCards = EnemyCards = 7;
            FirstPlayerReady = SecondPlayerReady = false;
        }

        [Key]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }
        public List<Top> Top { get; set; }
        public List<Bottom> Bottom { get; set; }
        public List<Grave> Grave { get; set; }
        public List<Exile> Exile { get; set; }
        public List<HandCard> HandCards { get; set; }
        public List<BattlefieldCard> BattlefieldCards { get; set; }

        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }
        public int FirstPlayerHp { get; set; }
        public int SecondPlayerHp { get; set; }
        public bool FirstPlayerReady { get; set; }
        public bool SecondPlayerReady { get; set; }

        [NotMapped]
        public int MyCards { get; set; }
        [NotMapped]
        public int EnemyCards { get; set; }
        [NotMapped]
        public bool AmIFirstPlayer { get; set; }

    }
}