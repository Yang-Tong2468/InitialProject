// CardManager.cs
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ƹ�����
/// </summary>
public class CardManager : MonoBehaviour
{
    [Header("�������ݿ�")]
    public List<CardData> cardDatabase; // �洢���п��ܵĿ������ݣ�ScriptableObjects��

    [Header("��������")]
    public GameObject cardPrefab;       // ���Ƶ�Ԥ����
    public GameObject cardSlotPrefab;  // ���Ʋ۵�Ԥ����
    public Transform handParent;         // ��������ĸ�����Transform

    [Header("����")]
    [Tooltip("�ڿ�ʼʱ�Զ����ɼ�����")]
    public int startingHandSize = 5;

    void Start()
    {
        // ��Ϸ��ʼʱ����
        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard();
        }
    }

    // �����ݿ��������ȡһ�ſ��Ʋ�����
    public void DrawCard()
    {
        if (cardDatabase == null || cardDatabase.Count == 0)
        {
            Debug.LogError("�������ݿ�Ϊ�գ��޷��鿨��");
            return;
        }

        if (cardPrefab == null || handParent == null)
        {
            Debug.LogError("����CardManager������Card Prefab��Hand Panel��");
            return;
        }

        // 1. ���ѡ��һ�ſ�������
        CardData randomCardData = cardDatabase[Random.Range(0, cardDatabase.Count)];

        // 2. ʵ�������Ʋ�Ԥ����
        GameObject newCardSlotObject = Instantiate(cardSlotPrefab, handParent);
        CardSlot newCardSlot = newCardSlotObject.GetComponent<CardSlot>();


        // 3. ��ȡ���ƽű�����������
        Card newCard = newCardSlotObject.GetComponent<Card>();
        if (newCard != null)
        {
            newCard.cardData = randomCardData;
            // �ֶ�����SetupCard�����¿��Ƶ��Ӿ�����
            newCard.SetupCard();
        }
        else
        {
            Debug.LogError("����Ԥ������û���ҵ�Card�ű���");
        }
    }
}