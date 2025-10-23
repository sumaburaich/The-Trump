using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class CardDragHandler : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    private Canvas canvas;
    private Transform original_parent;
    private CanvasGroup cg;
    private RectTransform rectTransform;
    public HandManager hm;

    [Header("ホバー拡大設定")]
    public float hover_scale = 1.2f;
    public float hover_duration = 0.2f;
    public float hover_time;
    private float ease_timer;

    private Vector3 original_scale;
    private float y_offset;
    private float hover_base_y;

    private Vector3 drag_offset_world;
    private bool is_dragging = false;
    private bool is_hovered = false;
    public bool is_ease_up;
    public bool is_ease_down;
    private float ease_start;

    [HideInInspector] public bool hoverLocked = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        cg = gameObject.AddComponent<CanvasGroup>();
        original_scale = rectTransform.localScale;


        y_offset = (hover_scale - 1f) * 0.5f * rectTransform.rect.height;
    }

    private void Update()
    {
        if (is_dragging)
        {
            // ドラッグ中はマウスに完全に追従
            Vector3 worldPoint;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out worldPoint))
            {
                rectTransform.position = worldPoint + drag_offset_world;
            }
        }

        if (is_ease_up)
        {
            ease_timer += Time.deltaTime;

            if (ease_timer < hover_time)
            {
                ease_timer += Time.deltaTime;
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x,
                    Easing.CircOut(ease_timer, hover_time, ease_start, ease_start + y_offset), rectTransform.localPosition.z);
            }

            else
            {
                ease_timer = 0f;
                is_ease_up = false;
            }
        }

        if (is_ease_down)
        {
            ease_timer += Time.deltaTime;

            if (ease_timer < hover_time)
            {
                ease_timer += Time.deltaTime;
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x,
                    Easing.CircOut(ease_timer, hover_time, ease_start, ease_start - y_offset), rectTransform.localPosition.z);
            }

            else
            {
                ease_timer = 0f;
                is_ease_down = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //掴めないタイミングは即リターン
        if (GameFlowRunner.cant_card_drag) return;

        var cs = GetComponent<card_state>();
        if (!cs.player_card || hoverLocked) return;

        is_dragging = true;

        transform.DOKill();
        if (is_hovered)
        {
            var pos = rectTransform.localPosition;
            pos.y = hover_base_y;
            rectTransform.localPosition = pos;
            rectTransform.localScale = original_scale;
            is_hovered = false;
        }

        original_parent = transform.parent;
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
        cg.blocksRaycasts = false;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector3 worldPoint))
        {
            drag_offset_world = rectTransform.position - worldPoint;
        }

        rectTransform.localRotation = Quaternion.identity;
        hm.UpdateHandLayoutInstant();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ドラッグ中はUpdateで直接追従
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //掴めないタイミングは即リターン
        if (GameFlowRunner.cant_card_drag) return;

        if (hoverLocked) return;

        is_dragging = false;
        cg.blocksRaycasts = true;

        Transform parent = transform.parent;
        if (parent == canvas.transform)
        {
            transform.SetParent(original_parent, true);
            parent = original_parent;
        }

        var handManager = parent.GetComponent<HandManager>();
        if (handManager != null)
        {
            handManager.UpdateHandLayoutInstant();
        }
        else
        {
            rectTransform.localRotation = Quaternion.identity;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var cs = GetComponent<card_state>();
        if (!cs.player_card || hoverLocked || is_dragging) return;

        is_hovered = true;
        hover_base_y = rectTransform.localPosition.y;

        //transform.DOKill();
        rectTransform.localScale = original_scale;
        //rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, hover_base_y, rectTransform.localPosition.z);

        //rectTransform.DOScale(original_scale * hover_scale, hover_duration).SetEase(Ease.OutQuad).SetUpdate(true);
        //rectTransform.DOLocalMoveY(hover_base_y - y_offset, hover_duration).SetEase(Ease.OutQuad).SetUpdate(true);


        if (!is_ease_up && !is_ease_down)
        {
            is_ease_up = true;
            ease_start = rectTransform.localPosition.y;
            ease_timer = 0f;
        }
        else if (is_ease_down && !is_ease_up)
        {
            is_ease_down = false;
            is_ease_up = true;
            ease_timer = 0f;
            ease_start = ease_start - y_offset;
        }
        else
        {
            ease_timer = 0f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var cs = GetComponent<card_state>();
        if (!cs.player_card || hoverLocked || is_dragging) return;

        is_hovered = false;

        transform.DOKill();
        //rectTransform.localScale = original_scale;
        //rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, hover_base_y - y_offset, rectTransform.localPosition.z);

        //rectTransform.DOScale(original_scale, hover_duration).SetEase(Ease.OutQuad).SetUpdate(true);
        //rectTransform.DOLocalMoveY(hover_base_y, hover_duration).SetEase(Ease.OutQuad).SetUpdate(true);
        if (!is_ease_down && !is_ease_up)
        {
            is_ease_down = true;
            ease_start = rectTransform.localPosition.y;
            ease_timer = 0f;
        }
        else if (is_ease_up && !is_ease_down)
        {
            is_ease_up = false;
            is_ease_down = true;
            ease_timer = 0f;
            ease_start = ease_start + y_offset;
        }
        else
        {
            ease_timer = 0f;
        }
    }

    public void RefreshHoverBase()
    {
        hover_base_y = rectTransform.localPosition.y;
    }

    public IEnumerator HoverCard(float now_pos, float end_pos, float ease_time)
    {
        yield return null;
        float time = 0f;

        if (time < ease_time)
        {
            time += Time.deltaTime;
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, Easing.CircOut(time, ease_time, now_pos, end_pos), rectTransform.localPosition.z);
        }
    }
}
