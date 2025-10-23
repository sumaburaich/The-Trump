using System.Collections.Generic;
using System.Linq;

namespace CardGame
{
    public class Hand
    {
        public readonly List<Card> cards = new List<Card>();
        public int count => cards.Count;

        public void AddRange(IEnumerable<Card> cardscol)
        {
            cards.AddRange(cardscol);
        }

        public void Remove(Card c)
        {
            cards.Remove(c);
        }

        public void Clear()
        {
            cards.Clear();
        }

        public IEnumerable<Card> ALL() => cards;
    }
}