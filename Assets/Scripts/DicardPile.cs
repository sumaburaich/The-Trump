using System.Collections.Generic;
using System.Xml.Serialization;

namespace CardGame
{
    public class DiscardPile
    {
        public readonly List<Card> cards = new List<Card>();
        public void Add(Card c) => cards.Add(c);
        public void AddRange(IEnumerable<Card> cardscol) => cards.AddRange(cardscol);
        public void Clear() => cards.Clear();
    }
}
