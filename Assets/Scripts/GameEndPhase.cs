using System.Collections;
using UnityEngine;

namespace CardGame
{
    public class GameEndPhase : IPhase
    {
        public IEnumerator Execute(GameContext ctx)
        {
            ctx.RaisePhase(Phase.GameEnd);

            //ŸÒ”»’è(3–{ææ)
            bool playerWins = ctx.Player_Wins >= ctx.Win_Target;
            Debug.Log($"GameEnd Winner = {(playerWins ? "PLAYER" : "ENEMY")}");

            //‚±‚±‚ÅŸ—˜‰‰o‚ğ‹²‚Ş
            yield return new WaitForSeconds(0.8f);
        }
    }
}