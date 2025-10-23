using System.Collections;

namespace CardGame
{
    public class RevealBoostCardPhase : IPhase
    {
        public IEnumerator Execute(GameContext ctx)
        {
            ctx.RaisePhase(Phase.RevealBoost);

            //1〜14 のIDを前提（Deck.ResetToDefaultに準拠）
            int id = ctx.random.Next(1, 15); // 1..14
            ctx.Boosted_Card_ID = $"C{id}";
            ctx.RaiseBoost(ctx.Boosted_Card_ID);

            //手札に該当があればフラグ付与（場に出す時のジョーカー扱い用）
            foreach (var c in ctx.player_hand.cards)
            {
                if (c.ID == ctx.Boosted_Card_ID) c.is_boosted = true;
            }

            yield return null; // 演出待ち
        }
    }
}
