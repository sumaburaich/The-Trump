using UnityEngine;

public class BoostManager : MonoBehaviour
{
    public int boost_min;
    public int boost_max;
    public int boost_num;
    public bool is_boost;
    public GameObject enable_obj;
    public static bool was_boost;
    public HandManager pl_hm;
    public HandManager en_hm;
    public trash_zone pl_trash;
    public trash_zone en_trash;
    void Start()
    {
        was_boost = false;
    }


    void Update()
    {
        if (is_boost)
        {
            boost_num = (int)Random.Range(boost_min, boost_max + 1);
            enable_obj.SetActive(true);
            card_state boost_cs = enable_obj.GetComponent<card_state>();
            boost_cs.card_num = boost_num;
            boost_cs.TMP_Reflection();
            is_boost = false;
            was_boost = true;
        }

        if (was_boost)
        {
            // ƒvƒŒƒCƒ„[èD
            foreach (var card in pl_hm.cs)
            {
                if (card.card_num == boost_num)
                {
                    card.BoostEnable();
                    break;
                }
            }

            // “GèD
            foreach (var card in en_hm.cs)
            {
                if (card.card_num == boost_num)
                {
                    card.BoostEnable();
                    break;
                }
            }

            // ƒvƒŒƒCƒ„[‚ÌÌ‚ÄD
            foreach (var state in pl_trash.states)
            {
                if (state.card_num == boost_num)
                {
                    state.BoostEnable();
                    break;
                }
            }

            // “G‚ÌÌ‚ÄD
            foreach (var state in en_trash.states)
            {
                if (state.card_num == boost_num)
                {
                    state.BoostEnable();
                    break;
                }
            }

            was_boost = false;
            GameFlowRunner.donext_phase = true;
        }
    }
}
