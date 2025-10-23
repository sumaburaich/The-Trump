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

            //3�{���܂ŌJ��Ԃ�
            while (ctx.Player_Wins < ctx.Win_Target && ctx.Enemy_Wins < ctx.Win_Target)
            {
                //�o���J�[�h�����N�G�X�g
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
                    //UI���ڑ���:��D�̐擪�������őI��
                    ctx.Pending_battle_card = ctx.player_hand.count > 0 ? ctx.player_hand.cards[0] : null;
                    decided = true;
                }

                yield return new WaitUntil(() => decided);

                //�G�̃J�[�h�������_������
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

                //�o�����J�[�h���̂ĎD��
                if (ctx.Pending_battle_card != null)
                {
                    ctx.player_hand.Remove(ctx.Pending_battle_card);
                    ctx.discard.Add(ctx.Pending_battle_card);
                    ctx.RaiseHand();
                    ctx.RaiseDiscard();
                }

                //���o�҂�
                yield return new WaitForSeconds(0.4f);
            }
            //�����𔲂�����O�{��悪���܂��Ă���͂�
            Debug.Log("���s�����܂����I");
        }

        //���s���W�b�N
        //�W���[�J�[�͕K���Ƃ���(boost�����l)
        //����ȊO��power��r
        private bool Resolve(Card player, Card enemy, GameContext ctx)
        {
            if (player.is_boosted || (ctx.Boosted_Card_ID != null && player.ID == ctx.Boosted_Card_ID))
                return true;
            if (enemy.is_boosted) return false;
            return player.power >= enemy.power;
        }
    }
}