using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldDropHandler : MonoBehaviour, IDropHandler
{
    public bool cant_drop;
    public bool is_full;
    private card_state field_card;
    public int field_num;
    public BattleManager bm;
    public bool is_exchange;
    public HandManager pl_hand;

    public void OnDrop(PointerEventData eventData)
    {
        //�h���b�v�����Ⴂ���Ȃ����͑����^�[��
        if (cant_drop) return;
        if (GameFlowRunner.cant_card_drag) return;
        GameObject dropped = eventData.pointerDrag;
        card_state drop_card = dropped.GetComponent<card_state>();
        if (dropped == null) return;
        if (!drop_card.player_card) return;

        if (!is_full && dropped != null)
        {
            Summon(dropped);
        }

        //�����Ή�
        else if (is_full && dropped != null)
        {
            bm.ExchangeCard(dropped);
        }
    }

    public void Summon(GameObject summon_card)
    {
        //�e��1P_field�ɕύX���ď�ɌŒ�
        is_full = true;

        //��ɏo���J�[�h�̏��L�^
        field_card = summon_card.GetComponent<card_state>();
        field_num = field_card.card_num;

        //��D�̃J�[�h������
        GameObject destoroy_obj = summon_card;
        card_state destoroy_state = summon_card.GetComponent<card_state>();
        for (int i = 0; i < pl_hand.cs.Count; i++)
        {
            if (pl_hand.cs[i] == destoroy_state)
            {
                pl_hand.cs.Remove(pl_hand.cs[i]);
                pl_hand.hand_cards.Remove(pl_hand.hand_cards[i]);
            }
        }
        Destroy(destoroy_obj);
        pl_hand.UpdateHandLayoutInstant();
        //�o�g���}�l�[�W���[�ɒʒm
        bm.is_set = true;
        bm.set_start(field_num, field_card.is_boost);
    }
}
