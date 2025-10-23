using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace CardGame
{
    public class BattlePhase : IPhase
    {
        public IEnumerator Execute(GameContext ctx)
        {
            ctx.RaisePhase(Phase.Battle);

            //3本先取まで繰り返す
            while (ctx.Player_Wins < ctx.Win_Target && ctx.Enemy_Wins < ctx.Win_Target)
            {
                //出すカードをリクエスト
                bool decided = false;
                ctx.Pending_battle_card = null;

                if (ctx.RequestBattleSelection != null)
                {
                    ctx.RequestBattleSelection((selected) =>
                    {
                        ctx.Pending_battle_card = selected;
                        decided = true;
                    });
                }
                else
                {
                    //UI未接続時:手札の先頭を自動で選択
                    ctx.Pending_battle_card = ctx.player_hand.count > 0 ? ctx.player_hand.cards[0] : null;
                    decided = true;
                }

                yield return new WaitUntil(() => decided);

                //敵のカードをランダム生成
                var enemyCard = new Card($"E{ctx.random.Next(1, 15)}", ctx.random.Next(1, 15));
                bool playerWins = Resolve(ctx.Pending_battle_card, enemyCard, ctx);

                if (playerWins)
                {
                    ctx.Player_Wins++;
                    ctx.OnBattleScoreChanged?.Invoke(ctx.Player_Wins);
                }
                else
                {
                    ctx.Enemy_Wins++;
                    ctx.OnEnemyBattleScoreChanged?.Invoke(ctx.Enemy_Wins);
                }

                //出したカードを捨て札へ
                if (ctx.Pending_battle_card != null)
                {
                    ctx.player_hand.Remove(ctx.Pending_battle_card);
                    ctx.discard.Add(ctx.Pending_battle_card);
                    ctx.RaiseHand();
                    ctx.RaiseDiscard();
                }

                //演出待ち
                yield return new WaitForSeconds(0.4f);
            }
            //ここを抜けたら三本先取が決まっているはず
            Debug.Log("勝敗が決まった！");
        }

        //勝敗ロジック
        //ジョーカーは必勝とする(boostも同様)
        //それ以外はpower比較
        private bool Resolve(Card player, Card enemy, GameContext ctx)
        {
            if (player.is_boosted || (ctx.Boosted_Card_ID != null && player.ID == ctx.Boosted_Card_ID))
                return true;
            if (enemy.is_boosted) return false;
            return player.power >= enemy.power;
        }
    }
}