using CardGame;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameContext
{
    public readonly MonoBehaviour Runner;

    public readonly System.Random random;
    public readonly Hand player_hand = new Hand();
    public readonly DiscardPile discard = new DiscardPile();
    public HandManager handmanager;
    public HandManager en_handmanager;

    //バトル勝利数
    public int Player_Wins;
    public int Enemy_Wins;
    public int Win_Target = 3;

    //現在ラウンドでジョーカー扱いになるカードID(公開する)
    public string Boosted_Card_ID;

    //UI入力用イベント
    public event Action<Phase> OnPhaseChanged;//フェーズ表示更新
    public event Action<List<Card>> OnHandUpdated;//手札更新
    public event Action<List<Card>> OnDiscardUpdated;//ステ札更新
    public event Action<string> OnBoostRevealed;//ブーストカード更新
    public Action<int> OnBattleScoreChanged;//スコア演出(プレイヤー側)
    public Action<int> OnEnemyBattleScoreChanged;//スコア演出(エネミー側)

    //プレイヤーの入力要求用
    public Action<Action<List<Card>>> RequestDiscardSelection;//任意枚数捨てる
    public Action<Action<Card>> RequestBattleSelection;//場に出すカード

    //内部的な一時領域(UIから受けとる値)
    public List<Card> Pending_discards;//UIで選ばれた捨てカード
    public Card Pending_battle_card;//UIで選ばれた出すカード

    public GameContext(MonoBehaviour runner, int seed = 0)
    {
        Runner = runner;
        random = seed == 0 ? new System.Random() : new System.Random(seed);
    }

    //ヘルパー
    public void RaisePhase(Phase p) => OnPhaseChanged?.Invoke(p);
    public void RaiseHand() => OnHandUpdated?.Invoke(new List<Card>(player_hand.cards));
    public void RaiseDiscard() => OnDiscardUpdated?.Invoke(new List<Card>(discard.cards));
    public void RaiseBoost(string id) => OnBoostRevealed?.Invoke(id);
}
