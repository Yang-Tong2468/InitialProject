using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Event_ZXH : MonoBehaviour
{
    // �¼�UI�����á���ÿ��UI���Ǻ�ģ�����Ԥ����������ͬ�ģ�����ֱ��ͨ�����ֻ�ȡ������ü��ɣ���Ȼÿ��UI��Ҫ����дһ���ű�����ȡ������ã�̫�鷳��
    [Header("��̬�¼�UI")]
    private TextMeshProUGUI Story;
    private TextMeshProUGUI Tips;
    private TextMeshProUGUI DurationDays;// �¼���������
    private TextMeshProUGUI RequiredAttributes;// ��Ҫ�������б���ͨ����ȡ��������д�������ж��ͬ���ģ�����Ҫ��˳��������������RequiredAttributes_Value��Ӧ����
    private TextMeshProUGUI RequiredAttributes_Value;// �����б��Ӧ������ֵ������ȡ���Ʋ۵���ֵ�����
    private TextMeshProUGUI SuccessThreshold;

    [Header("��̬����")]
    private CardSlot[] CardSlots; // ���Ʋ����飬���ڻ�ȡ����ֵ����ͨ��CardSlot���õ����Ʋ�
    [Tooltip("������ʾ���������'�б���'Ԥ����")]
    [SerializeField] private GameObject attributeRequirementItemPrefab;//����������Ԥ���壬���ڶ�̬�������������б�
    [Tooltip("���ڷ������������б��������")]
    [SerializeField] private Transform attributesContainer; //����ֱ������

    // �洢��ǰ���ɵ��б���
    private List<GameObject> spawnedAttributeItems = new List<GameObject>();



    private void Awake()
    {
        // ��ȡ������Ҫ�����
        Story = transform.Find("Story").GetComponent<TextMeshProUGUI>();
        Tips = transform.Find("Tips").GetComponent<TextMeshProUGUI>();
        DurationDays = transform.Find("DurationDays").GetComponent<TextMeshProUGUI>();
        //RequiredAttributes = transform.Find("RequiredAttributes").GetComponent<TextMeshProUGUI>();
        //RequiredAttributes_Value = transform.Find("RequiredAttributes_Value").GetComponent<TextMeshProUGUI>();
        SuccessThreshold = transform.Find("SuccessThreshold").GetComponent<TextMeshProUGUI>();

        CardSlots = transform.GetComponentsInChildren<CardSlot>(); 
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
        SuccessThreshold.text = $"�ɹ���ֵ: {eventData.SuccessThreshold}";

        // 2. ������һ�����ɵĶ�̬�б�
        foreach (var item in spawnedAttributeItems)
        {
            Destroy(item);
        }
        spawnedAttributeItems.Clear();

        // 3. ��̬�������������б�
        foreach (string attributeName in eventData.RequiredAttributes)
        {
            // ʵ�����б���Ԥ����
            GameObject itemInstance = Instantiate(attributeRequirementItemPrefab, attributesContainer);

            // ��ȡ��Ԥ�����ϵ��ı����
            // �������Ǽ�����Ԥ�����ϵ����Ҳ�� "AttributeNameText" �� "AttributeValueText"
            TextMeshProUGUI nameText = itemInstance.transform.Find("AttributeNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI valueText = itemInstance.transform.Find("AttributeValueText").GetComponent<TextMeshProUGUI>();

            // �������
            nameText.text = attributeName; 

            // ���Ʋ۵�ǰ��������ֵ
            
            //valueText.text = playerValue.ToString();

            // ���������Բ��㣬���Ըı���ֵ��ɫ������ʾ
            // if (playerValue < some_threshold) valueText.color = Color.red;

            // �����ɵ�ʵ����ӵ��б��У��Ա��´�����
            spawnedAttributeItems.Add(itemInstance);
        }
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
