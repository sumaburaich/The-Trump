using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class DiscardPhase : IPhase
    {
        public IEnumerator Execute(GameContext ctx)
        {
            ctx.RaisePhase(Phase.Discard);

            // UIに選択要求（任意枚数）
            bool decided = false;
            ctx.Pending_discards = null;

            if (ctx.RequestDiscardSelection != null)
            {
                ctx.RequestDiscardSelection((selected) =>
                {
                    ctx.Pending_discards = selected ?? new List<Card>();
                    decided = true;
                });
            }
            else
            {
                // 入力未接続なら 0枚捨てのデフォルト
                ctx.Pending_discards = new List<Card>();
                decided = true;
            }

            // UIからの決定を待つ
            yield return new WaitUntil(() => decided);

            // 反映
            foreach (var c in ctx.Pending_discards)
            {
                ctx.player_hand.Remove(c);
            }
            ctx.discard.AddRange(ctx.Pending_discards);
            ctx.RaiseHand();
            ctx.RaiseDiscard();

            yield return null;
        }
    }
}