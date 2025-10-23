using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    public BattleManager bm;
    public enum SCORE_TYPE
    {
        PLAYER,
        ENEMY
    }
    public SCORE_TYPE score_type;
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        if (score_type == SCORE_TYPE.PLAYER)
        {
            tmp.text = bm.win_num.ToString();
        }
        else
        {
            tmp.text = bm.lose_num.ToString();
        }
    }
}
