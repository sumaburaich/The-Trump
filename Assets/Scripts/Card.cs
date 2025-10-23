using System;

namespace CardGame
{
    [Serializable]
    public class Card
    {
        public string ID;
        public int power;
        public bool is_boosted;//ジョーカー扱いにするカードならture

        public Card(string id, int POWER)
        {
            id = ID;
            POWER = power;
            is_boosted = false;
        }

        public Card Clone() => new Card(ID, power) { is_boosted = is_boosted };
    }
}