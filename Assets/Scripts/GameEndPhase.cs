using System.Collections;
using UnityEngine;

namespace CardGame
{
    public class GameEndPhase : IPhase
    {
        public IEnumerator Execute(GameContext ctx)
        {
            ctx.RaisePhase(Phase.GameEnd);

            //���Ҕ���(3�{���)
            bool playerWins = ctx.Player_Wins >= ctx.Win_Target;
            Debug.Log($"GameEnd Winner = {(playerWins ? "PLAYER" : "ENEMY")}");

            //�����ŏ������o������
            yield return new WaitForSeconds(0.8f);
        }
    }
}