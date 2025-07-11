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
    public CardData cardData; // �������Ǵ�����ScriptableObject
    public CardType cardType; // �������ͣ����Դ�CardData�л�ȡ��
    public TextMeshProUGUI id; // ����ID�����Դ�CardData�л�ȡ��
    public TextMeshProUGUI cardName; // �������ƣ����Դ�CardData�л�ȡ��
    public TextMeshProUGUI type; // �������ͣ����Դ�CardData�л�ȡ��

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

    // ����CardData��ʼ�����Ƶ���ʾ
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

        PrintAllAttributes(); // ��ӡ����������Ϣ
    }

    // ��ʼ��ק
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

    // ��ק������
    public void OnDrag(PointerEventData eventData)
    {
        // ���¿��Ƶ�λ�ø������/��ָ
        transform.position = eventData.position;
    }

    // ������ק
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

    // �Ҽ����ʱ������������
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CardDetailPanel.Instance.Show(cardData, Input.mousePosition);
        }
    }


    // ��¼��ǰ�����塪�����õ������۳ɹ��������ɿ��۵���
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