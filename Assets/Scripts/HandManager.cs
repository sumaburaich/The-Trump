using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandManager : MonoBehaviour
{
    public enum CARD_TYPE
    {
        PLAYER,
        ENEMY
    }
    [Header("敵の場合はチェックを入れる")]
    public bool is_enemy;
    public CARD_TYPE card_type;

    [Header("カード配置設定")]
    public float radius = 500f;        // 扇の半径
    public float angle_step = 10f;     // 各カードの角度差

    [Header("手札管理")]
    public List<GameObject> hand_cards = new List<GameObject>();
    public List<card_state> cs = new List<card_state>();
    public discard_manager dm;

    [Header("デッキ")]
    public Deck pl_deck;
    public Transform deck_pos;         // デッキの位置
    public GameObject card_prefab;
    public float deal_interval = 0.3f; // 1枚ずつ配る間隔
    public float move_duration = 0.5f; // 移動時間

    [HideInInspector] public bool isDealing = false; // 1枚でも配布中は true
    [HideInInspector] public bool allDealt = false; // 全て配布完了後 true

    /// <summary>
    /// 手札のカードを扇形に並べる
    /// </summary>
    public void UpdateHandLayoutInstant()
    {
        int count = hand_cards.Count;
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            GameObject card = hand_cards[i];
            CardDragHandler handler = card.GetComponent<CardDragHandler>();
            handler.RefreshHoverBase();
            float angle = (i - (count - 1) / 2f) * angle_step;
            float rad = Mathf.Deg2Rad * angle;

            // Y の符号を反転
            Vector3 target_pos = new Vector3(Mathf.Sin(rad) * radius,
                                             Mathf.Cos(rad) * radius,
                                             0);

            card.transform.localPosition = target_pos;
            card.transform.localRotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    public void AddCard(GameObject card)
    {
        hand_cards.Add(card);
        card.transform.SetParent(transform, false);
        UpdateHandLayoutInstant();
    }

    public void RemoveCard(GameObject card)
    {
        hand_cards.Remove(card);
        UpdateHandLayoutInstant();
    }

    public IEnumerator OrganizHnad()
    {
        yield return null;
        for (int i = 0; i < cs.Count; i++)
        {
            if (cs[i] == null)
            {
                cs.RemoveAt(i);
                hand_cards.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// デッキから手札にカードを1枚ずつ配る
    /// </summary>
    public IEnumerator DealCards(int count, CARD_TYPE card_type, bool secret = false)
    {
        isDealing = true;
        allDealt = false;//ディール開始時にリセット

        for (int i = 0; i < count; i++)
        {
            GameObject card = Instantiate(card_prefab, transform);

            // CardDragHandler を取得してホバー禁止に（個別のロック）
            CardDragHandler dragHandler = card.GetComponent<CardDragHandler>();
            dragHandler.hm = gameObject.GetComponent<HandManager>();
            if (dragHandler != null) dragHandler.hoverLocked = true;

            //cardのステータスを入れるタイミング
            card_state state = card.GetComponent<card_state>();
            state.is_secret = secret;
            state.card_num = pl_deck.cards[0];
            state.TMP_Reflection();
            dm.candidate_state.Add(state.card_num);
            pl_deck.cards.RemoveAt(0);
            cs.Add(state);
            if (card_type == CARD_TYPE.PLAYER)
            {
                cs[i].player_card = true;
            }

            Vector3 startPos = transform.InverseTransformPoint(deck_pos.position);
            card.transform.localPosition = startPos;

            hand_cards.Add(card);
            UpdateHandLayoutInstant();
            Vector3 targetPos = card.transform.localPosition;

            // 配布アニメーション
            card.transform.localPosition = startPos;
            Tween moveTween = card.transform.DOLocalMove(targetPos, move_duration)
                                             .SetEase(Ease.OutQuad);

            // 移動完了後に個別ホバー解禁
            moveTween.OnComplete(() =>
            {
                if (dragHandler != null) dragHandler.hoverLocked = false;
            });

            yield return moveTween.WaitForCompletion();
            yield return new WaitForSeconds(deal_interval);
        }

        // すべてのカード配布が終わったら
        isDealing = false;
        allDealt = true; //card_stateから完了を判定
    }
}
