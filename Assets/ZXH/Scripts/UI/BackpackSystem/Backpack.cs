using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ϵͳ - ���޸�Ϊʹ�������˵�ɸѡ
/// </summary>
public class Backpack : MonoBehaviour
{
    [Header("ͷ��")]
    public TMP_Dropdown categoryDropdown; // ���������˵�
    public Button equip; // װ����ť
    public Button synthesis; // �ϳɰ�ť
    public Button system; // ϵͳ��ť

    [Header("�������")]
    public Transform cardContentParent; // ���п���UI�ĸ����壬����Ψһ����ʾ����

    [Header("Ԥ���� (Prefabs)")]
    public GameObject CardSlotPrafbe; // ����Ԥ����
    public GameObject CardPrafbe;     // ����Ԥ����

    [Header("����")]
    // ʹ���ֵ䰴���ͷ���洢���п�������
    private Dictionary<CardType, List<CardData>> cardCollection = new Dictionary<CardType, List<CardData>>();
    public List<CardData> cardDatabase; // ��ʼ���ֶ���ӵĿ���

    [Header("װ��")]
    // �洢��ǰ��״̬
    [SerializeField]private EquipState currentState = EquipState.None;
    // ��ʱ��ŵ�һ��ѡ�еĽ�ɫ��
    [SerializeField]private RoleCardData selectedRole;

    // ����װ�����̵ĸ���״̬
    private enum EquipState
    {
        None,               // �������״̬
        SelectingCharacter, // �ȴ����ѡ��һ����ɫ��
        SelectingEquipment  // �ȴ����ѡ��һ��װ����
    }


    private void OnEnable()
    {
        equip.onClick.AddListener(StartEquipProcess);
    }

    private void OnDisable()
    {
        equip.onClick.RemoveListener(StartEquipProcess);
    }

    private void Start()
    {
        InitializeBackpack();
    }

    #region ���÷���

    /// <summary>
    /// �򱳰������һ���¿�������
    /// </summary>
    public void AddCard(CardData data)
    {
        if (data != null && cardCollection.ContainsKey(data.cardType))
        {
            cardCollection[data.cardType].Add(data);
            cardDatabase.Add(data);
            Debug.Log($"�ѽ����� '{data.cardName}' ��ӵ����������С�");

            // ˢ����ʾ���Է�ӳ���ݵı仯
            OnFilterChanged(categoryDropdown.value);
        }

        //�������ƶ�
        CardManager.Instance.cardQueue.Enqueue(data);
        CardManager.Instance.AddCardData();
    }

    /// <summary>
    /// �ӱ������Ƴ�һ��ָ���Ŀ�������
    /// </summary>
    public void RemoveCard(CardData dataToRemove)
    {
        if (dataToRemove != null && cardCollection.ContainsKey(dataToRemove.cardType))
        {
            bool removed = cardCollection[dataToRemove.cardType].Remove(dataToRemove);
            if (removed)
            {
                cardDatabase.Remove(dataToRemove); // �����ݿ����Ƴ�
                Debug.Log($"�Ѵ��������Ƴ����� '{dataToRemove.cardName}'");

                // ˢ����ʾ���Է�ӳ���ݵı仯
                OnFilterChanged(categoryDropdown.value);
            }
        }

        //�����ƶ����Ƴ�
        CardManager.Instance.RemoveCard(dataToRemove);
    }
    #endregion

    #region  ������
    /// <summary>
    /// ��ʼ�����������ݺ�UI��������
    /// </summary>
    private void InitializeBackpack()
    {
        // ��ʼ�����ݴ洢�ṹ
        cardCollection.Clear();
        foreach (CardType type in System.Enum.GetValues(typeof(CardType)))
        {
            cardCollection.Add(type, new List<CardData>());
        }

        // ���س�ʼ����
        //cardDatabase = CardManager.Instance.cardDatabase;
        foreach (CardData card in cardDatabase)
        {
            if (cardCollection.ContainsKey(card.cardType))
            {
                cardCollection[card.cardType].Add(card);
            }
        }

        // ���������˵���ѡ��
        SetupFilterDropdown();

        // �������˵���ֵ�仯�¼�
        categoryDropdown.onValueChanged.AddListener(OnFilterChanged);

        // ��Ϸ��ʼʱ��Ĭ����ʾ
        OnFilterChanged(0);
    }

    /// <summary>
    /// ��ʾ��Ӧ�Ŀ��ơ����������˵�ѡ��ı�ʱ����
    /// </summary>
    /// <param name="index">ѡ���������</param>
    private void OnFilterChanged(int index)
    {
        ResetEquipProcess();//���װ������

        // ���ѡ����ǵ�һ�ȫ����
        if (index == 0)
        {
            PopulateDisplay(null); // ����null������ʾȫ��
        }
        // �õ�����ѡ��
        else
        {
            // ���ǵ�ѡ���б�index=1��Ӧö�ٵ�index=0������Ҫ��1
            string typeName = categoryDropdown.options[index].text;
            CardType selectedType = (CardType)System.Enum.Parse(typeof(CardType), typeName);
            PopulateDisplay(selectedType);
        }
    }

    /// <summary>
    /// �������������俨��
    /// </summary>
    /// <param name="filterType">Ҫɸѡ�����͡����Ϊnull������ʾ�������͵Ŀ��ơ�</param>
    private void PopulateDisplay(CardType? filterType)
    {
        // �����������оɵĿ���/����
        foreach (Transform child in cardContentParent)
        {
            Destroy(child.gameObject);
        }

        // ����ɸѡ��������Ҫ��ʾ��Щ����
        if (filterType.HasValue)
        {
            // ��ʾ�ض����͵Ŀ��� 
            if (cardCollection.ContainsKey(filterType.Value))
            {
                List<CardData> cardsToDisplay = cardCollection[filterType.Value];
                CreateCardUI(cardsToDisplay);
            }
        }
        else
        {
            // ��ʾȫ������ 
            foreach (var categoryList in cardCollection.Values)
            {
                CreateCardUI(categoryList);
            }
        }
    }

    /// <summary>
    /// ���ݸ����������б���UIԪ��
    /// </summary>
    /// <param name="dataList">���������б�</param>
    private void CreateCardUI(List<CardData> dataList)
    {
        foreach (CardData data in dataList)
        {
            // ʵ�������ۺͿ���
            GameObject slotObj = Instantiate(CardSlotPrafbe, cardContentParent);
            CardSlot slot = slotObj.GetComponent<CardSlot>();
            slot.acceptedCardType = data.cardType;
            slot.SetSlotColor(data.cardType);

            GameObject cardObj = Instantiate(CardPrafbe);
            Card card = cardObj.GetComponent<Card>();
            card.cardData = data;
            card.SetupCard();

            // �����Ʒ��뿨��
            slot.SetChild(card);
        }
    }

    /// <summary>
    /// ����ɸѡ�����˵���ѡ�����ʼ��ʱ����
    /// </summary>
    private void SetupFilterDropdown()
    {
        categoryDropdown.ClearOptions();
        List<string> options = new List<string> { "ȫ��" };

        // ö���л�ȡ�����������Ʋ���ӵ�ѡ����
        options.AddRange(System.Enum.GetNames(typeof(CardType)));

        categoryDropdown.AddOptions(options);
    }

    #endregion

    #region װ��

    /// <summary>
    /// ��ʼװ�����̣����
    /// </summary>
    private void StartEquipProcess()
    {
        Debug.Log("��ʼװ�����̣���ѡ��һ����ɫ");
        currentState = EquipState.SelectingCharacter;

        // ɸѡ��ֻ��ʾ��ɫ����
        PopulateDisplay(CardType.Role);
    }

    /// <summary>
    /// ����/ȡ��װ�����̣� ����
    /// </summary>
    private void ResetEquipProcess()
    {
        currentState = EquipState.None;
        selectedRole = null;
        Debug.Log("װ��������ȡ�������");
    }

    /// <summary>
    /// �����Ʊ����ʱ
    /// </summary>
    /// <param name="cardData">������Ŀ���</param>
    public void SelectCard(CardData cardData)
    {
        // ���ݵ�ǰ��װ��״̬�����������Ӧ��ε��
        switch (currentState)
        {
            // ���ڵȴ�ѡ���ɫ
            case EquipState.SelectingCharacter:
                if (cardData.cardType == CardType.Role)
                {
                    // ��ҵ����һ�Ž�ɫ��
                    selectedRole = cardData as RoleCardData; // �洢ѡ�еĽ�ɫ
                    Debug.Log($"��ѡ���ɫ: {selectedRole.cardName}����ѡ��һ��װ��");

                    // ѡ��װ��
                    currentState = EquipState.SelectingEquipment;
                    PopulateDisplay(CardType.Equip); // ɸѡ��ֻ��ʾװ����
                }
                break;

            // ���ڵȴ�ѡ��װ��
            case EquipState.SelectingEquipment:
                if (cardData.cardType == CardType.Equip)
                {
                    // ��ҵ����һ��װ����
                    EquipCardData selectedEquip = cardData as EquipCardData;
                    Debug.Log($"׼��Ϊ {selectedRole.cardName} װ�� {selectedEquip.cardName}");

                    // �������Ѿ�д�õ�װ���߼�
                    Equip(selectedEquip, selectedRole);

                    // װ����ɣ�������������
                    ResetEquipProcess();

                    // ˢ��UI�ص�Ĭ�ϵ�ȫ��״̬
                    OnFilterChanged(0);
                    categoryDropdown.value = 0; // �������˵�����ʾҲ����Ϊ��ȫ����
                }
                break;

            // �����״̬��ʲô������ 
            case EquipState.None:

            default:
                Debug.Log($"����˿���: {cardData.cardName} (��ǰΪ��ͨ���ģʽ)");
                break;
        }
    }

    /// <summary>
    /// װ�����滻�������ɫ�����ϼ�װһ��װ����
    /// </summary>
    /// <param name="equipCard"></param>
    public void Equip(CardData equipCard, CardData roleCard)
    {
        RoleCardData role = roleCard as RoleCardData;
        EquipCardData equip = equipCard as EquipCardData;

        role.Equip(equip);
    }

    /// <summary>
    /// ж��װ�������ӽ�ɫ������ж��װ����
    /// </summary>
    /// <param name="equipCard"></param>
    /// <param name="roleCard"></param>
    public void UnEquip(CardData equipCard, CardData roleCard)
    {
        RoleCardData role = roleCard as RoleCardData;
        EquipCardData equip = equipCard as EquipCardData;

        role.UnEquip(equip);
    }

    #endregion

    #region �ϳ�

    /// <summary>
    /// 
    /// </summary>
    /// <param name="equip">Ŀ��װ��</param>
    /// <param name="craftMaterials">���Ĳ���</param>
    public void Craft(EquipCardData equip, List<CardData> craftMaterials)
    {
        
    }

    #endregion
}