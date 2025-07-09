using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Event_ZXH : MonoBehaviour
{
    [Header("�����ֶ�")] 
    [SerializeField]private int currentDay = 1; // ����ʱ����ʼΪ1
    private EventData eventData; // ���ڴ洢��ǰ�¼�����

    [Header("��ק����")]
    public GameObject One;
    public GameObject Two;
    public GameObject Three;

    [Header("One��̬�¼�UI_��ק")]
    [SerializeField] private TextMeshProUGUI EventName; // �¼�����
    [SerializeField] private TextMeshProUGUI EventTime; // �¼�ʱ��

    // �¼�UI�����á���ÿ��UI���Ǻ�ģ�����Ԥ����������ͬ�ģ�����ֱ��ͨ�����ֻ�ȡ������ü��ɣ���Ȼÿ��UI��Ҫ����дһ���ű�����ȡ������ã�̫�鷳��
    [Header("Two��̬�¼�UI")]
    [SerializeField] private TextMeshProUGUI Story;
    [SerializeField] private TextMeshProUGUI Tips;
    [SerializeField] private TextMeshProUGUI DurationDays;// �¼���������
    //[SerializeField] private TextMeshProUGUI RequiredAttributes;// ��Ҫ�������б���ͨ����ȡ��������д�������ж��ͬ���ģ�����Ҫ��˳��������������RequiredAttributes_Value��Ӧ����
    //[SerializeField] private TextMeshProUGUI RequiredAttributes_Value;// �����б��Ӧ������ֵ������ȡ���Ʋ۵���ֵ�����
    [SerializeField] private TextMeshProUGUI SuccessThreshold;
    [SerializeField]private float successProbability = 1f;//�ɹ�����
    [SerializeField]private int t= 0;//�ɹ�����
    [SerializeField]private bool isSuccess;

    [Header("Two��̬��������")]
    [SerializeField] private CardSlot[] CardSlots; // ���Ʋ����飬���ڻ�ȡ����ֵ����ͨ��CardSlot���õ����Ʋ�
    [Tooltip("������ʾ���������'�б���'Ԥ����")]
    [SerializeField] private GameObject attributeRequirementItemPrefab;//����������Ԥ���壬���ڶ�̬�������������б�
    [Tooltip("���ڷ������������б��������")]
    [SerializeField] private Transform attributesContainer; //����ֱ������
    [SerializeField] private List<GameObject> spawnedAttributeItems = new List<GameObject>(); // �洢��ǰ���ɵ��б��������ֵ


    [Header("Three��̬�¼�UI_��ק")]
    [SerializeField] private TextMeshProUGUI Result_Story; // �¼�����İ�
    [SerializeField] private TextMeshProUGUI Reward_Card; // �¼������������ơ���ֱ�Ӱѽ����Ŀ������ִ�ӡ����������ֱ�Ӽ��뵽���ƿ���
    [SerializeField] private TextMeshProUGUI Result_Dice; // ���ӵĽ��
    [SerializeField] private TextMeshProUGUI Name; // �¼�����


    private void Awake()
    {
        // ��ȡ������Ҫ�����
        Story = FindTMPDeep("Story");
        Tips = FindTMPDeep("Tips");
        DurationDays = FindTMPDeep("DurationDays");
        //SuccessThreshold = FindTMPDeep("SuccessThreshold");

        CardSlots = transform.GetComponentsInChildren<CardSlot>(); 
    }


    private void Update()
    {
        // ʵʱˢ��ÿ�������������������ֵ
        foreach (var item in spawnedAttributeItems)
        {
            var nameText = item.transform.Find("AttributeNameText").GetComponent<TextMeshProUGUI>();
            var valueText = item.transform.Find("AttributeValueText").GetComponent<TextMeshProUGUI>();
            if (nameText != null && valueText != null)
            {
                string attrName = nameText.text;
                
                valueText.text = GetTotalAttributeValue(attrName).ToString();
                
            }
        }
    }

    #region ����
    /// <summary>
    /// ���е���ʱ������������ʱ����
    /// </summary>
    public void AddTime()
    {
        currentDay++;
        if(eventData.DurationDays <= currentDay)
        {
            ExecutionEvent(eventData); // ִ���¼��߼�
        }
    }

    /// <summary>
    /// ʹ���¼���������ʼ�����������UI���
    /// </summary>
    public void Initialize(EventData eventData)
    {
        this.eventData = eventData;

        //One
        EventName.text = eventData.EventName;
        EventTime.text = eventData.DurationDays.ToString();

        //Two
        // 1. ��侲̬����
        Story.text = eventData.Story;
        Tips.text = eventData.Tips;
        DurationDays.text = $"����ʱ��: {eventData.DurationDays} ��";
        //SuccessThreshold.text = $"�ɹ���ֵ: {eventData.SuccessThreshold}";

        // 2. ������һ�����ɵĶ�̬�б�
        foreach (var item in spawnedAttributeItems)
        {
            Destroy(item);
        }
        spawnedAttributeItems.Clear();

        // 3. ��̬�������������б�
        foreach (string attributeName in eventData.RequiredAttributes)
        {
            Debug.Log($"Event_ZXH:�������{attributeName}");
            // ʵ�����б���Ԥ����
            GameObject itemInstance = Instantiate(attributeRequirementItemPrefab, attributesContainer);

            // ��ȡ��Ԥ�����ϵ��ı����
            // �������Ǽ�����Ԥ�����ϵ����Ҳ�� "AttributeNameText" �� "AttributeValueText"
            TextMeshProUGUI nameText = itemInstance.transform.Find("AttributeNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI valueText = itemInstance.transform.Find("AttributeValueText").GetComponent<TextMeshProUGUI>();

            // �������
            nameText.text = attributeName;

            // ���Ʋ۵�ǰ��������ֵ

            valueText.text = "0";

            // ���������Բ��㣬���Ըı���ֵ��ɫ������ʾ
            // if (playerValue < some_threshold) valueText.color = Color.red;

            // �����ɵ�ʵ����ӵ��б��У��Ա��´�����
            spawnedAttributeItems.Add(itemInstance);


            //Three
            Name.text = eventData.EventName;
            //ʣ�µ�ҪͶ�����Ӳ�����

        }
    }

    #endregion


    #region One


    #endregion


    #region Two

    /// <summary>
    /// ��ȡ���п�����ָ�����Ե��ܺ�
    /// </summary>
    private int GetTotalAttributeValue(string attrName)
    {
        int total = 0;

        foreach (var slot in CardSlots)
        {
            var card = slot.GetComponentInChildren<Card>();
            if (card != null)
            {
                total += card.cardData.GetAttributeValue(attrName);
            }
        }
        return total;
    }


    /// <summary>
    /// ��Ȳ����������е� TextMeshProUGUI ���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private TextMeshProUGUI FindTMPDeep(string name)
    {
        var tmps = GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var tmp in tmps)
        {
            if (tmp.name == name)
                return tmp;
        }
        Debug.LogError($"Event_ZXH: δ�ҵ���Ϊ {name} �� TextMeshProUGUI �����");
        return null;
    }

    /// <summary>
    /// ��ȡ��������valueText��ֵ֮��
    /// </summary>
    private int GetAllValueTextSum()
    {
        int sum = 0;
        foreach (var item in spawnedAttributeItems)
        {
            var valueText = item.transform.Find("AttributeValueText").GetComponent<TextMeshProUGUI>();
            if (valueText != null && int.TryParse(valueText.text, out int val))
            {
                sum += val;
            }
        }
        return sum;
    }

    #endregion

    #region Three

    /// <summary>
    /// ִ���¼��߼��������¼����ݺͳɹ������������¼����
    /// </summary>
    /// <param name="eventData"></param>
    public void ExecutionEvent(EventData eventData)
    {
        if (RollTheDice(eventData,successProbability))
        {
            // �ɹ��߼�
            Result_Story.text = eventData.SuccessfulResults;
            Result_Dice.text = $"�ɹ����ӵĸ�����{t}";
            Reward_Card.text = $"��ã�{eventData.RewardItemIDs}"; // ��������滻Ϊʵ�ʵĽ����߼�
            
            GiveRewards(eventData); // ���Ž�������
        }
        else
        {
            // ʧ���߼�
            Result_Story.text = eventData.FailedResults;
            Result_Dice.text = "�ɹ����ӵĸ�����{t}";
            Reward_Card.text = "û�н���";
        }
        // չ��Three���
        ExpandThree();
    }

    /// <summary>
    /// �����¼�����ID�б���CardManager.cardDatabase���Ҳ�ʵ�����������Ƶ�����
    /// </summary>
    public void GiveRewards(EventData eventData)
    {
        if (eventData == null || eventData.RewardItemIDs == null || eventData.RewardItemIDs.Count == 0)
            return;

        // ��ȡCardManagerʵ�������賡����ֻ��һ��CardManager��
        CardManager cardManager = FindObjectOfType<CardManager>();
        if (cardManager == null)
        {
            Debug.LogError("CardManager δ�ҵ���");
            return;
        }

        foreach (string rewardName in eventData.RewardItemIDs)
        {
            // ����cardDatabase������ƥ���CardData
            CardData rewardCard = cardManager.cardDatabase.FirstOrDefault(cd => cd.cardName == rewardName);
            if (rewardCard != null)
            {
                cardManager.AddCard(rewardCard);
                Debug.Log($"�������ƣ�{rewardCard.cardName} �Ѽ�������");
            }
            else
            {
                Debug.LogWarning($"δ��cardDatabase���ҵ���Ϊ {rewardName} �Ŀ��ƣ��޷����Ž�����");
            }
        }
    }

    /// <summary>
    /// �����ӣ������Ƿ�ɹ�,successProbability=0.5��ʾ50%���ʳɹ�
    /// </summary>
    public bool RollTheDice(EventData eventData, float successProbability)
    {
        Debug.Log($"Event_ZXH: �����ӣ��ɹ�����Ϊ {successProbability * 100}%");
        int diceSum = GetAllValueTextSum();//���Ӹ���
        int threshold = eventData.SuccessThreshold;//�ɹ���ֵ

        t = 0;//�ɹ�����

        successProbability = Mathf.Clamp01(successProbability);

        for(int i = 1;i <=diceSum; i++)
        {
            // ������
            float rand = Random.value; // [0,1)
            bool isSuccess = rand < successProbability;
            if (isSuccess)
            {
                t++;
            }
        }

        if(t >= threshold)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }

        return isSuccess;
    }
    #endregion

    #region ������岿��

    // չ��One�ķ���
    public void ExpandOne()
    {
        if (One != null)
        {
            One.SetActive(true);
        }
    }

    //�ر�One�ķ���
    public void CloseOne()
    {
        if (One != null)
        {
            One.SetActive(false);
        }
    }


    // չ��Two�ķ���
    public void ExpandTwo()
    {
        if (Two != null)
        {
            Two.SetActive(true);
            CloseOne();
        }
    }

    //�ر�Two�ķ���
    public void CloseTwo()
    {
        if (Two != null)
        {
            Two.SetActive(false);
            ExpandOne();
        }
    }


    // չ��Three�ķ���
    public void ExpandThree()
    {
        if (Three != null)
        {
            Three.SetActive(true);
        }
        CloseTwo();
    }

    //�ر�Three�ķ���
    public void CloseThree()
    {
        if (Three != null)
        {
            Three.SetActive(false);
        }
        ExpandOne();
    }

    #endregion



    // һ���򵥵ķ��뺯��
    private string TranslateAttributeName(string key)
    {
        switch (key.ToLower())
        {
            case "intelligence": return "����";
            case "charm": return "����";
            case "strength": return "����";
            default: return key;
        }
    }
}
