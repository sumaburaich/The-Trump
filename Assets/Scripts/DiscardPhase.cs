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

            // UI�ɑI��v���i�C�Ӗ����j
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
                // ���͖��ڑ��Ȃ� 0���̂Ẵf�t�H���g
                ctx.Pending_discards = new List<Card>();
                decided = true;
            }

            // UI����̌����҂�
            yield return new WaitUntil(() => decided);

            // ���f
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