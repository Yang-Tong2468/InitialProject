using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Event_ZXH : MonoBehaviour
{
    [Header("��ק����")]
    public GameObject One;
    public GameObject Two;
    public GameObject Three;

    // �¼�UI�����á���ÿ��UI���Ǻ�ģ�����Ԥ����������ͬ�ģ�����ֱ��ͨ�����ֻ�ȡ������ü��ɣ���Ȼÿ��UI��Ҫ����дһ���ű�����ȡ������ã�̫�鷳��
    [Header("��̬�¼�UI")]
    [SerializeField] private TextMeshProUGUI Story;
    [SerializeField] private TextMeshProUGUI Tips;
    [SerializeField] private TextMeshProUGUI DurationDays;// �¼���������
    //[SerializeField] private TextMeshProUGUI RequiredAttributes;// ��Ҫ�������б���ͨ����ȡ��������д�������ж��ͬ���ģ�����Ҫ��˳��������������RequiredAttributes_Value��Ӧ����
    //[SerializeField] private TextMeshProUGUI RequiredAttributes_Value;// �����б��Ӧ������ֵ������ȡ���Ʋ۵���ֵ�����
    [SerializeField] private TextMeshProUGUI SuccessThreshold;

    [Header("��̬����")]
    [SerializeField] private CardSlot[] CardSlots; // ���Ʋ����飬���ڻ�ȡ����ֵ����ͨ��CardSlot���õ����Ʋ�
    [Tooltip("������ʾ���������'�б���'Ԥ����")]
    [SerializeField] private GameObject attributeRequirementItemPrefab;//����������Ԥ���壬���ڶ�̬�������������б�
    [Tooltip("���ڷ������������б��������")]
    [SerializeField] private Transform attributesContainer; //����ֱ������

    // �洢��ǰ���ɵ��б��������ֵ
    [SerializeField] private List<GameObject> spawnedAttributeItems = new List<GameObject>();



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

    /// <summary>
    /// ʹ���¼���������ʼ�����������UI���
    /// </summary>
    public void Initialize(EventData eventData)
    {
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
        }
    }

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
    }

    //�ر�Three�ķ���
    public void CloseThree()
    {
        if (Three != null)
        {
            Three.SetActive(false);
        }
    }

    #endregion


    /// <summary>
    /// ��ȡ���п�����ָ�����Ե��ܺ�
    /// </summary>
    private int GetTotalAttributeValue(string attrName)
    {
        int total = 0;
        foreach (var slot in CardSlots)
        {
            if (slot.child != null && slot.child.cardData != null)
            {
                total += slot.child.cardData.GetAttributeValue(attrName);
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
