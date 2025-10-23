using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class discard_manager : MonoBehaviour
{

    public bool start_discard;
    public List<int> candidate_state = new List<int>();
    public List<GameObject> enable_obj = new List<GameObject>();
    public List<card_state> candidate_cards = new List<card_state>();
    public List<int> discard_state = new List<int>();
    public enter_discard ed;
    public trash_zone trash;
    public HandManager hm;

    void Start()
    {
        start_discard = false;

        //敵の手札ならはじくものをはじく
        if (hm.is_enemy)
        {
            ed = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!hm.is_enemy)
        {
            if (start_discard)
            {
                for (int i = 0; i < candidate_state.Count; i++)
                {
                    candidate_cards[i].card_num = candidate_state[i];
                    candidate_cards[i].TMP_Reflection();
                }
            }
            EnterDiscard();
        }

        else
        {
            //プレイヤー側でディスカードを決めた時
            if (transform.parent.transform.Find("Player_Discard_zone").GetComponent<discard_manager>().ed.is_enter)
            {
                //とりあえず乱数枚捨てる
                int rnd = (int)Random.Range(0f, 6f);

                for (int i = 0; i < rnd; i++)
                {
                    discard_state.Add(candidate_state[i]);
                    trash.AddTrash(discard_state[i], false);
                }

                for (int i = hm.cs.Count - 1; i >= 0; i--)
                {
                    if (discard_state.Contains(hm.cs[i].card_num))
                    {
                        Debug.Log(hm.cs[i].card_num + "を消しますよ");

                        if (i < hm.hand_cards.Count && hm.hand_cards[i] != null)
                        {
                            Destroy(hm.hand_cards[i]);
                        }

                        hm.cs.RemoveAt(i);
                        hm.hand_cards.RemoveAt(i);
                    }
                }

                Date_Clear();
                StartCoroutine(hm.OrganizHnad());
            }
        }
    }


    public void Date_Clear()
    {
        discard_state.Clear();
        candidate_state.Clear();
    }

    public void EnterDiscard()
    {
        if (ed.is_enter)
        {
            foreach (int num in discard_state)
            {
                trash.AddTrash(num, false);
            }

            for (int i = hm.cs.Count - 1; i >= 0; i--)
            {
                if (discard_state.Contains(hm.cs[i].card_num))
                {
                    Debug.Log(hm.cs[i].card_num + "を消しますよ");

                    if (i < hm.hand_cards.Count && hm.hand_cards[i] != null)
                    {
                        Destroy(hm.hand_cards[i]);
                    }

                    hm.cs.RemoveAt(i);
                    hm.hand_cards.RemoveAt(i);
                }
            }

            foreach (var obj in enable_obj)
            {
                obj.SetActive(false);
            }

            Date_Clear();
            StartCoroutine(hm.OrganizHnad());
            GameFlowRunner.donext_phase = true;
            ed.is_enter = false;
        }
    }
}
