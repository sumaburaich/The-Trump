using System.Collections;
using UnityEngine;

namespace CardGame
{
    public class RoundStartPhase : IPhase
    {
        public IEnumerator Execute(GameContext ctx)
        {
            ctx.RaisePhase(Phase.RoundStart);

            //�R�D���Z�b�g(�����E���h)
            ctx.player_hand.Clear();
            ctx.discard.Clear();
            ctx.Boosted_Card_ID = null;
            ctx.Player_Wins = 0;
            ctx.Enemy_Wins = 0;

            yield return new WaitForSeconds(0.5f);
            GameFlowRunner.donext_phase = true;
        }
    }
}