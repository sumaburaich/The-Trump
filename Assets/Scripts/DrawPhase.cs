using System.Collections;
using UnityEngine;

namespace CardGame
{
    public class DrawPhase : IPhase
    {
        private readonly int _drawCount;
        public DrawPhase(int drawCount = 5) { _drawCount = drawCount; }
        [Header("�J�[�h�ݒ�")]
        public HandManager handManager;   // ��D�Ǘ�
        public int startHandCount = 5;    // �J�n���̎�D����

        public IEnumerator Execute(GameContext ctx)
        {
            //�v���C���[�ƓG�̃h���[�𓯎����s����
            bool playerDone = false;
            bool enemyDone = false;

            ctx.Runner.StartCoroutine(RunDeal(ctx.handmanager, startHandCount, () => playerDone = true));
            ctx.Runner.StartCoroutine(RunDeal(ctx.en_handmanager, startHandCount, () => enemyDone = true));

            // ��������������܂őҋ@
            yield return new WaitUntil(() => playerDone && enemyDone);

            ctx.RaisePhase(Phase.Draw);
            ctx.RaiseHand();

            yield return null; // �K�v�Ȃ牉�o�҂�
            GameFlowRunner.donext_phase = true;
        }

        //DealCards ���s��ɃR�[���o�b�N���Ă�
        private IEnumerator RunDeal(HandManager hm, int count, System.Action onComplete)
        {
            yield return hm.DealCards(count, hm.card_type);
            onComplete?.Invoke();
        }
    }
}
