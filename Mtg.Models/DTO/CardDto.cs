using System.Collections.Generic;
using Mtg.Models.CardModels;

namespace Mtg.Models.DTO
{
    public class CardDto
    {
        public CardDto(CardLink cardLink)
        {
            ImgLink = cardLink.Card.ImgLink;
            text = cardLink.Card.text;
            rulings = cardLink.Card.rulings;
            id = cardLink.Card.id;
            name = cardLink.Card.name;
            cardLinkId = cardLink.Id.ToString();
        }
        public string ImgLink { get; set; }
        public string text { get; set; }
        public List<Ruling> rulings { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string cardLinkId { get; set; }
    }
}