using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name}がクリックされました。");
    }

}
