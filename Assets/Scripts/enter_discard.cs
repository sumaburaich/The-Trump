using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class enter_discard : MonoBehaviour, IPointerClickHandler
{
    public bool is_enter;

    public void OnPointerClick(PointerEventData eventData)
    {
        is_enter = true;
    }
}
