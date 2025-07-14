// Card.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// ȷ�����ƶ�������Image��CanvasGroup���
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("��������")]
    public CardData cardData; // ����
    public CardType cardType; // ��������
    public TextMeshProUGUI id; // ����ID
    public TextMeshProUGUI cardName; // ��������
    public TextMeshProUGUI type; // ��������

    private Transform originalParent; // ��¼��קǰ�ĸ�����
    public Transform OriginalParent { get { return originalParent; } }  

    private CanvasGroup canvasGroup;
    private Image cardImage;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        cardImage = GetComponent<Image>();
        SetupCard();
    }

    /// <summary>
    /// ����CardData��ʼ�����Ƶ���ʾ
    /// </summary>
    public void SetupCard()
    {
        if (cardData != null)
        {
            if (cardImage != null && cardData.artwork != null)
            {
                cardImage.sprite = cardData.artwork;
            }

            gameObject.name = "Card_" + cardData.cardName;

            id.text = "id:" + cardData.id;
            cardName.text = "Name:" + cardData.cardName;
            type.text = "Type:" + cardData.cardType.ToString();
        }

        //PrintAllAttributes(); // ��ӡ����������Ϣ
    }

    /// <summary>
    /// ��ʼ��ק
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 1. ��¼ԭʼ�����壬������קʧ��ʱ����
        originalParent = transform.parent;

        // 2. �����Ƶĸ���������ΪCanvas�ĸ��ڵ㣬ʹ����Ⱦ�����ϲ�
        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); // ȷ�������ϲ���Ⱦ

        // 3. ����CanvasGroup���ÿ�������קʱ���Դ�͸���߼��
        // �����·��Ŀ��Ʋ۲��ܽ��յ�OnDrop�¼�
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f; // ��͸��Ч�������û���ק���Ӿ�����
    }

    /// <summary>
    /// ��ק������
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // ���¿��Ƶ�λ�ø������/��ָ
        transform.position = eventData.position;
    }

    /// <summary>
    /// ������ק
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // �ָ����Ƶ����߼���͸����
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        // ��鿨���Ƿ񱻷��õ����µĸ����壨��һ����Ч�Ŀ��Ʋۣ���
        // ���������û�иı䣬˵��û���ҵ����ʵĿ��Ʋ�
        if (transform.parent == transform.root || transform.parent == originalParent)
        {
            // û�з��õ���Ч���ۣ�����ԭλ
            SetCardPos();
        }
    }

    /// <summary>
    /// �Ҽ����ʱ������������
    /// </summary>
    /// <param name="eventData">��������</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CardDetailPanel.Instance.Show(cardData, Input.mousePosition);
        }
    }


    /// <summary>
    /// ��¼��ǰ�����塪�����õ������۳ɹ��������ɿ��۵���
    /// </summary>
    /// <param name="newParent">���ÿ��Ƶ�ԭʼ������</param>
    public void SetNewParent(Transform newParent)
    {
        originalParent = newParent;
    }

    /// <summary>
    /// ���ÿ��Ƶ�λ��Ϊԭʼ�������λ��
    /// </summary>
    public void SetCardPos()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// ��ӡ���Ƶ�����������Ϣ������̨
    /// </summary>
    public void PrintAllAttributes()
    {
        if (cardData == null)
        {
            Debug.LogWarning("CardData Ϊ�գ��޷���ӡ���ԡ�");
            return;
        }

        string[] attrNames = new string[]
        {
        "physique", "social", "survival", "intelligence", "charm", "combat", "support"
        };

        string info = $"���� [{cardData.cardName}] ���ԣ�";
        foreach (var attrName in attrNames)
        {
            int value = cardData.GetAttributeValue(attrName);
            info += $"\n{attrName}: {value}";
        }
        Debug.Log(info);
    }
}