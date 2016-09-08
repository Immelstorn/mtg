using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Mtg.Database;
using Mtg.Models.CardModels;

namespace Mtg.Api.Helper
{
    public class Utils
    {
        public const int HandSize = 7;

        private const string LinkToImg = "http://magiccards.info/scans/{0}/{1}/{2}.jpg";
        private const string Language = "en";

        public static void FillImages(IEnumerable<Card> cards)
        {
            foreach(var card in cards)
            {
                FillOneImage(card);
            }
        }

        public static void FillOneImage(Card card)
        {
            if(card != null)
            {
                card.ImgLink = string.Format(LinkToImg, Language, card.Set.magicCardsInfoCode ?? card.Set.code, card.number.ToLower());
            }
        }

        public static async Task<List<Card>> GetRandomCards(int count, MtgDbContext context)
        {
            var cards = await context.Cards.Where(c => !c.rarity.Equals("Basic Land")).OrderBy(r => Guid.NewGuid()).Include(c => c.rulings).Take(count).ToListAsync();
            FillImages(cards);
            return cards;
        }
    }
}
