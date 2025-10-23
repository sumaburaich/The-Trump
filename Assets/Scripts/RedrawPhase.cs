using System.Collections;
using System.Diagnostics;
using UnityEngine;


namespace CardGame
{
    public class RedrawPhase : IPhase
    {
        private readonly int _targetHandSize;
        public RedrawPhase(int targetHandSize = 5) { _targetHandSize = targetHandSize; }

        public IEnumerator Execute(GameContext ctx)
        {
            //�v���C���[�ƓG�̃h���[�𓯎����s����
            bool playerDone = false;
            bool enemyDone = false;

            ctx.Runner.StartCoroutine(RunDeal(ctx.handmanager, 5 - ctx.handmanager.hand_cards.Count, () => playerDone = true));
            ctx.Runner.StartCoroutine(RunDeal(ctx.en_handmanager, 5 - ctx.en_handmanager.hand_cards.Count, () => enemyDone = true, true));

            // ��������������܂őҋ@
            yield return new WaitUntil(() => playerDone && enemyDone);
            ctx.RaisePhase(Phase.Redraw);

            int need = _targetHandSize - ctx.player_hand.count;
            if (need > 0)
            {
                ctx.RaiseHand();
            }

            yield return null;
            GameFlowRunner.donext_phase = true;
        }

        //DealCards ���s��ɃR�[���o�b�N���Ă�
        private IEnumerator RunDeal(HandManager hm, int count, System.Action onComplete, bool secret = false)
        {
            yield return hm.DealCards(count, hm.card_type, secret);
            onComplete?.Invoke();
        }
    }
}
