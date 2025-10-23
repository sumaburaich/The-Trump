using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public int pl_state;
    public int en_state;
    public trash_zone pl_trash;
    public trash_zone en_trash;
    public List<GameObject> enable_obj = new List<GameObject>();
    public bool is_set;
    public bool is_battle;
    public int win_num;
    public int lose_num;
    public enter_discard ed;
    public HandManager en_hand;
    public FieldDropHandler field_drop;
    public ResultManager rm;
    public bool pl_boost;
    public bool en_boost;


    void Update()
    {
        //勝負を押した時
        if (ed.is_enter)
        {
            ChoiceEnemyCard(en_hand);
            enable_obj[0].SetActive(false);
            ed.is_enter = false;
            field_drop.cant_drop = true;
            StartCoroutine(Decision(1.0f, en_state, pl_state, field_drop));
        }
    }

    public void set_start(int set_num, bool boost)
    {
        //UI,場のカードなどの表示ON
        for (int i = 0; enable_obj.Count - 2 > i; i++)
        {
            enable_obj[i].SetActive(true);
        }
        //場に出してください非表示
        enable_obj[3].SetActive(false);

        //カードのステータス設定
        card_state cs = enable_obj[1].GetComponent<card_state>();
        pl_state = set_num;
        cs.card_num = set_num;
        cs.TMP_Reflection();
        cs.is_boost = boost;
        if (cs.is_boost)
        {
            cs.BoostEnable();
            pl_boost = true;
        }
        else
        {
            cs.BoostDisable();
            pl_boost = false;
        }
    }

    public void ExchangeCard(GameObject new_card)
    {
        card_state new_card_state = new_card.GetComponent<card_state>();
        int old_card_state = pl_state;
        bool old_card_boost = pl_boost;
        //場に出すカードのステータス設定
        card_state cs = enable_obj[1].GetComponent<card_state>();
        int set_num = new_card_state.card_num;
        bool boost = new_card_state.is_boost;
        pl_state = set_num;
        cs.card_num = set_num;
        cs.TMP_Reflection();
        cs.is_boost = boost;
        if (cs.is_boost)
        {
            cs.BoostEnable();
            pl_boost = true;
        }
        else
        {
            cs.BoostDisable();
            pl_boost = false;
        }

        //手札に戻るカードのステータス設定
        new_card_state.card_num = old_card_state;
        new_card_state.is_boost = old_card_boost;
        new_card_state.TMP_Reflection();
        if (new_card_state.is_boost)
        {
            new_card_state.BoostEnable();
        }
        else
        {
            new_card_state.BoostDisable();
        }
    }

    //敵のカード選択AI
    public void ChoiceEnemyCard(HandManager enemy_hand)
    {
        //場のカードを有効化してそこからcard_stateを持ってくる
        enable_obj[2].SetActive(true);
        card_state enemy_state = enable_obj[2].GetComponent<card_state>();

        //カード選定(乱数)
        int rnd = (int)Random.Range(0, enemy_hand.cs.Count);
        enemy_state.card_num = enemy_hand.cs[rnd].card_num;
        en_state = enemy_hand.cs[rnd].card_num;
        enemy_state.is_boost = enemy_hand.cs[rnd].is_boost;

        //準ジョーカーの有効化、数字の更新
        enemy_state.TMP_Reflection();
        if (enemy_state.is_boost)
        {
            enemy_state.BoostEnable();
            en_boost = true;
        }
        else
        {
            enemy_state.BoostDisable();
            en_boost = false;
        }

        //敵の手札の情報削除、手札の並び調整
        GameObject destoroy_obj = enemy_hand.cs[rnd].gameObject;
        enemy_hand.cs.Remove(enemy_hand.cs[rnd]);
        enemy_hand.hand_cards.Remove(enemy_hand.hand_cards[rnd]);
        Destroy(destoroy_obj);
        enemy_hand.UpdateHandLayoutInstant();
    }


    //勝敗判定関数
    public IEnumerator Decision(float wait_time, int enemy_num, int player_num, FieldDropHandler fdh)
    {
        yield return new WaitForSeconds(wait_time);

        if (player_num == 14 && en_boost) { win_num++; }//プレイヤーがジョーカーでエネミーがブーストしてる時
        else if (pl_boost && enemy_num == 14) { lose_num++; }//エネミーがジョーカーでプレイヤーがブーストしてる時
        else if (pl_boost && !en_boost) { win_num++; }//もしプレイヤーにブーストがかかってたら
        else if (en_boost && !pl_boost) { lose_num++; }//もしエネミーにブーストがかかってたら
        else if (enemy_num == 14 && player_num == 1) { win_num++; }//エネミーがジョーカーでプレイヤーがAの時
        else if (player_num == 14 && enemy_num == 1) { lose_num++; }//プレイヤーがジョーカーでエネミーがAの時
        else if ((player_num >= 1 && player_num <= 3) && (enemy_num >= 11 && enemy_num <= 13)) { win_num++; }//プレイヤーが1,2,3でエネミーが11,12,13の時
        else if ((enemy_num >= 1 && enemy_num <= 3) && (player_num >= 11 && player_num <= 13)) { lose_num++; }//エネミーが1,2,3でプレイヤーが11,12,13の時
        else if (player_num > enemy_num) { win_num++; }//プレイヤーの数字が大きいとき
        else if (enemy_num > player_num) { lose_num++; }//エネミーの数字が大きいとき


        if (win_num < 3 && lose_num < 3)
        {
            enable_obj[1].SetActive(false);
            enable_obj[2].SetActive(false);
            enable_obj[3].SetActive(true);
            pl_trash.AddTrash(player_num, true);
            en_trash.AddTrash(enemy_num, true);
            fdh.is_full = false;
            fdh.cant_drop = false;
            pl_boost = false;
            en_boost = false;
        }

        //もし3点以上取ったら
        else
        {
            enable_obj[1].SetActive(false);
            enable_obj[2].SetActive(false);
            enable_obj[3].SetActive(false);
            pl_trash.AddTrash(player_num, true);
            en_trash.AddTrash(enemy_num, true);
            fdh.is_full = true;
            pl_boost = false;
            en_boost = false;
            //リザルト開始
            rm.StartResult(win_num, lose_num);
            GameFlowRunner.donext_phase = true;
        }
    }
}
