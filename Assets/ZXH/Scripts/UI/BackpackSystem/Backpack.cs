using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ϵͳ
/// </summary>
public class Backpack : MonoBehaviour
{
    [Header("ͷ����ť (Buttons)")]
    public Button roleButton;
    public Button intelButton;
    public Button bookButton;
    public Button coinButton;

    [Header("����������� (Panels)")]
    public Transform rolePanel;   // ��Ž�ɫ���Ƶ����
    public Transform intelPanel;  // ����鱨���Ƶ����
    public Transform bookPanel;   // ����鼮���Ƶ����
    public Transform coinPanel;   // ��Ž�ҿ��Ƶ����

    [Header("Ԥ���� (Prefabs)")]
    public GameObject CardSlotPrafbe; // ����Ԥ����
    public GameObject CardPrafbe;     // ����Ԥ����

    // ʹ���ֵ䰴���ͷ���洢���п�������
    private Dictionary<CardType, List<CardData>> cardCollection = new Dictionary<CardType, List<CardData>>();
    public List<CardData> cardDatabase;

    private Transform currentActivePanel; // ��¼��ǰ��ʾ�����

    void Start()
    {
        // 1. ��ʼ�����ݴ洢�ṹ��Ϊÿ�����ʹ���һ�����б�
        cardCollection.Add(CardType.Role, new List<CardData>());
        cardCollection.Add(CardType.Intel, new List<CardData>());
        cardCollection.Add(CardType.Book, new List<CardData>());
        cardCollection.Add(CardType.Coin, new List<CardData>());

        // 2. Ϊÿ����ť��ӵ���¼��ļ�����
        roleButton.onClick.AddListener(() => SwitchToCategory(CardType.Role));
        intelButton.onClick.AddListener(() => SwitchToCategory(CardType.Intel));
        bookButton.onClick.AddListener(() => SwitchToCategory(CardType.Book));
        coinButton.onClick.AddListener(() => SwitchToCategory(CardType.Coin));

        // �����п���������ӵ�������
        cardDatabase = CardManager.Instance.cardDatabase;
        foreach (CardData card in cardDatabase)
        {
            AddCard(card); 
        }

        // 3. ��Ϸ��ʼʱ��Ĭ����ʾ��һ������ (���� "Role")
        SwitchToCategory(CardType.Role);
    }

    /// <summary>
    /// �򱳰������һ���¿��ơ���ֻ�Ǽ������ݣ���������
    /// �����ű�������Ϸ���������������ȣ������ô˷���
    /// </summary>
    /// <param name="data">Ҫ��ӵĿ�������</param>
    public void AddCard(CardData data)
    {
        if (data == null)
        {
            Debug.LogWarning("������ӿյĿ������ݣ�");
            return;
        }

        // ����ֵ����Ƿ���ڸÿ������͵��б�
        if (cardCollection.ContainsKey(data.cardType))
        {
            // ������������ӵ���Ӧ���б���
            cardCollection[data.cardType].Add(data);
            Debug.Log($"�ѽ����� '{data.cardName}' ��ӵ������� {data.cardType} ������");

            // �������ӵĿ����������ڵ�ǰ���ڲ鿴�ķ��࣬��ˢ�������������ʾ�¿���
            if (currentActivePanel == GetPanelForType(data.cardType))
            {
                PopulatePanel(currentActivePanel, data.cardType);
            }
        }
        else
        {
            Debug.LogWarning($"δ֪�Ŀ�������: {data.cardType}, �޷���ӵ�������");
        }
    }

    /// <summary>
    /// �л�����ʾָ�����͵Ŀ������
    /// </summary>
    /// <param name="category">Ҫ��ʾ�Ŀ�������</param>
    private void SwitchToCategory(CardType category)
    {
        // �������������
        rolePanel.gameObject.SetActive(false);
        intelPanel.gameObject.SetActive(false);
        bookPanel.gameObject.SetActive(false);
        coinPanel.gameObject.SetActive(false);

        // �ҵ�������Ŀ�����
        Transform panelToActivate = GetPanelForType(category);
        if (panelToActivate != null)
        {
            panelToActivate.gameObject.SetActive(true);
            currentActivePanel = panelToActivate; // ���µ�ǰ���������¼

            // �ڼ����������������ж�Ӧ�Ŀ���
            PopulatePanel(panelToActivate, category);
        }
    }

    /// <summary>
    /// ��ָ��������������͵����п���
    /// </summary>
    /// <param name="panel">����Ҫ���������ĸ��������</param>
    /// <param name="category">Ҫ���ɵĿ�������</param>
    private void PopulatePanel(Transform panel, CardType category)
    {
        Transform contentParent = panel.transform.Find("Viewport/Content");

        // 1. �����������оɵĿ���/���ۣ���ֹ�ظ�����
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 2. �����ݼ����л�ȡ�÷�������п�������
        List<CardData> cardsToDisplay = cardCollection[category];

        // 3. ���������б�Ϊÿ�ſ��ƴ���UIʵ��
        foreach (CardData data in cardsToDisplay)
        {
            GameObject slotObj = Instantiate(CardSlotPrafbe, contentParent);
            CardSlot slot = slotObj.GetComponent<CardSlot>();
            slot.acceptedCardType = data.cardType;
            slot.SetSlotColor(data.cardType);

            GameObject cardObj = Instantiate(CardPrafbe);
            Card card = cardObj.GetComponent<Card>();
            card.cardData = data;
            card.SetupCard();

            slot.SetChild(card);
        }
    }

    /// <summary>
    /// ���ݿ������ͷ��ض�Ӧ��Transform���
    /// </summary>
    private Transform GetPanelForType(CardType type)
    {
        return type switch
        {
            CardType.Role => rolePanel,
            CardType.Intel => intelPanel,
            CardType.Book => bookPanel,
            CardType.Coin => coinPanel,
            _ => null,
        };
    }

    /// <summary>
    /// �ӱ������Ƴ�һ��ָ���Ŀ���
    /// </summary>
    /// <param name="dataToRemove">Ҫ�Ƴ��Ŀ��Ƶ�����</param>
    public void RemoveCard(CardData dataToRemove)
    {
        if (dataToRemove == null)
        {
            Debug.LogWarning("�����Ƴ�һ���յĿ������ݣ�");
            return;
        }

        // ����ֵ����Ƿ���ڸÿ��Ƶķ���
        if (cardCollection.ContainsKey(dataToRemove.cardType))
        {
            // �Ӷ�Ӧ���б����Ƴ���һ��ƥ��Ŀ�������
            bool removed = cardCollection[dataToRemove.cardType].Remove(dataToRemove);

            if (removed)
            {
                Debug.Log($"�Ѵ��������Ƴ����� '{dataToRemove.cardName}'");

                // ������ű��Ƴ��Ŀ������ڵķ�����嵱ǰ�Ƿ�Ϊ����״̬
                if (currentActivePanel == GetPanelForType(dataToRemove.cardType))
                {
                    // ����ǣ������PopulatePanel��ˢ��������壬
                    // ����Զ��Ƴ����ٴ����������б��еĿ���UI
                    PopulatePanel(currentActivePanel, dataToRemove.cardType);
                }
            }
            else
            {
                Debug.LogWarning($"�ڱ���������δ�ҵ�Ҫ�Ƴ��Ŀ��� '{dataToRemove.cardName}'��");
            }
        }
    }
}