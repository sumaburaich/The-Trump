using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class select_discard : MonoBehaviour, IPointerClickHandler
{
    public bool is_select;
    public Outline outline;
    public Image image;
    public discard_manager dm;
    public card_state state;

    public void Start()
    {
        state = GetComponent<card_state>();
        state = GetComponent<card_state>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!is_select)
        {
            is_select = true;
            outline.enabled = true;
            image.color = new Color(0.86f, 0.86f, 0.86f);
            dm.discard_state.Add(state.card_num);
        }

        else
        {
            is_select = false;
            outline.enabled = false;
            image.color = new Color(0.73f, 0.73f, 0.73f);

            for (int i = 0; i < dm.discard_state.Count; i++)
            {
                if (dm.discard_state[i] == state.card_num)
                {
                    dm.discard_state.Remove(state.card_num);
                }
            }
        }
    }
}
