using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("�� OnDrop ����");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("�� PointerEnter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("�� PointerExit");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("�N���b�N��ꂽ��I");
    }
}
