using System.Collections;
using UnityEngine;

namespace CardGame
{
    public class DrawPhase : IPhase
    {
        private readonly int _drawCount;
        public DrawPhase(int drawCount = 5) { _drawCount = drawCount; }
        [Header("カード設定")]
        public HandManager handManager;   // 手札管理
        public int startHandCount = 5;    // 開始時の手札枚数

        public IEnumerator Execute(GameContext ctx)
        {
            //プレイヤーと敵のドローを同時実行する
            bool playerDone = false;
            bool enemyDone = false;

            ctx.Runner.StartCoroutine(RunDeal(ctx.handmanager, startHandCount, () => playerDone = true));
            ctx.Runner.StartCoroutine(RunDeal(ctx.en_handmanager, startHandCount, () => enemyDone = true));

            // 両方が完了するまで待機
            yield return new WaitUntil(() => playerDone && enemyDone);

            ctx.RaisePhase(Phase.Draw);
            ctx.RaiseHand();

            yield return null; // 必要なら演出待ち
            GameFlowRunner.donext_phase = true;
        }

        //DealCards 実行後にコールバックを呼ぶ
        private IEnumerator RunDeal(HandManager hm, int count, System.Action onComplete)
        {
            yield return hm.DealCards(count, hm.card_type);
            onComplete?.Invoke();
        }
    }
}
