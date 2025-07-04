using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("�������ݿ�")]
    public List<CardData> cardDatabase;

    [Header("��������")]
    public GameObject cardPrefab;// ����Ԥ�Ƽ����������ɿ���ʵ��
    public GameObject cardSlotPrefab;// ���Ʋ�Ԥ�Ƽ������ڷ��ÿ��Ƶ�����
    public Transform handParent;// ���Ʋ۵ĸ����壬ͨ������������

    private CardDataQueue cardQueue = new CardDataQueue();// �������ݶ��У����ڴ洢�͹���������

    void Start()
    {
        // ��ʼ������
        foreach (var data in cardDatabase)
        {
            cardQueue.Enqueue(data);
        }
        // ����
        for (int i = 0; i < 4; i++)
        {
            DrawCard();
        }
    }

    /// <summary>
    /// �ӿ��ƶ����г�ȡһ�ſ��Ʋ����ɶ�Ӧ�Ŀ���ʵ��
    /// </summary>
    public void DrawCard()
    {
        if (cardQueue.Count == 0)
        {
            Debug.LogWarning("û�п��õĿ������ݣ�");
            return;
        }

        CardData data = cardQueue.Dequeue();
        if (data == null) return;

        // 1. ���ɿ��Ʋ�
        GameObject slotObj = Instantiate(cardSlotPrefab, handParent);
        CardSlot slot = slotObj.GetComponent<CardSlot>();
        slot.acceptedCardType = data.cardType;
        slot.SetSlotColor(data.cardType);

        // 2. ���ɿ���
        GameObject cardObj = Instantiate(cardPrefab);
        Card card = cardObj.GetComponent<Card>();
        card.cardData = data;
        card.SetupCard();

        // 3. ���뿨�Ʋ�
        slot.SetChild(card);
    }
}
