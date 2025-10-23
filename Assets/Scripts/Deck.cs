using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<int> cards= new List<int>();

    public void Start()
    {
        shuffle();
    }
    public void shuffle()
    {
        System.Random rng = new System.Random();
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // 0〜n の範囲
            int value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }
}
