using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using NUnit.Framework;

public class ResultManager : MonoBehaviour
{
    public GameObject enable_obj;
    public TextMeshProUGUI text;
    public TextMeshProUGUI shadaw;


    void Update()
    {

    }

    public void StartResult(int win, int lose)
    {
        enable_obj.SetActive(true);

        if (win > lose)
        {
            text.text = "YOU WIN!!";
            shadaw.text = "YOU WIN!!";
        }
        else if (win < lose)
        {
            text.text = "YOU LOSE!!";
            shadaw.text = "YOU LOSE!!";
        }
    }
}
