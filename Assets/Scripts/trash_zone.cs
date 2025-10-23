using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class trash_zone : MonoBehaviour
{
    public GameObject new_trash_card;
    public List<card_state> states = new List<card_state>();
    public BoostManager bm;

    public void AddTrash(int num, bool boost_chack)
    {
        //墓地にカードを出して、トランスフォーム周りをキャンバスに合わせて、カードの数字を反映している
        GameObject trash_card = Instantiate(new_trash_card);
        trash_card.transform.parent = transform;
        RectTransform rt = trash_card.GetComponent<RectTransform>();
        rt.localScale = new Vector3(0.5f, 0.79f, 0.5f);
        rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0);
        card_state cs = trash_card.GetComponent<card_state>();
        cs.card_num = num;
        states.Add(cs);
        cs.TMP_Reflection();

        if (boost_chack)
        {
            if (bm.boost_num == cs.card_num)
            {
                cs.BoostEnable();
            }
        }
    }

    public void ClearTrash()
    {
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }

    }
}
