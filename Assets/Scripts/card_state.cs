using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class card_state : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool is_secret;
    public bool player_card;
    public bool is_boost;
    public bool is_redraw;
    public int card_num;
    private TextMeshPro tmp;
    private Image img;

    [Header("�z�o�[�ݒ�")]
    public float hoverHeight = 50f;   // Inspector�Œ����\
    public float hoverDuration = 0.2f;

    private Vector3 initialPos;
    private bool hovering;
    private bool posInitialized;
    private HandManager handManager;

    void Awake()
    {
        handManager = GetComponentInParent<HandManager>();

        if (handManager == null)
        {
            Debug.LogWarning($"{name} �̐e�� HandManager ��������܂���");
        }
        else
        {
            if (handManager.card_type == HandManager.CARD_TYPE.PLAYER && !player_card)
            {
                player_card = true;
            }
        }
    }

    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        if (handManager.card_type == HandManager.CARD_TYPE.ENEMY)
        {

        }
    }

    public void TMP_Reflection()
    {
        if (tmp == null) tmp = GetComponentInChildren<TextMeshPro>();
        tmp.text = card_num.ToString();
        switch (card_num)
        {
            case 1:
                tmp.text = "A";
                break;

            case 11:
                tmp.text = "J";
                break;

            case 12:
                tmp.text = "Q";
                break;

            case 13:
                tmp.text = "K";
                break;

            case 14:
                tmp.text = "JK";
                break;
        }

        if (is_secret)
        {
            tmp.text = "";
        }
    }

    public void BoostEnable()
    {
        is_boost = true;
        //������D�̒����B���\��Ȃ烊�^�[��
        if (transform.parent.name == "EnemyHand" && is_secret) return;

        if (img == null) img = GetComponentInChildren<Image>();
        img.color = new Color(0.83f, 0.192f, 0.192f);
    }

    public void BoostDisable()
    {
        is_boost = false;
        if (img == null) img = GetComponentInChildren<Image>();
        img.color = new Color(1f, 1f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //�S�f�B�[�������܂ł̓z�o�[�֎~
        if (handManager == null || !handManager.allDealt) return;

        if (!hovering)
        {
            hovering = true;

            if (!posInitialized)
            {
                initialPos = transform.localPosition;
                posInitialized = true;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hovering) return;
        hovering = false;
    }
}
