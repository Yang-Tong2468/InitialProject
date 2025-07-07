// CardData.cs
using System.Collections.Generic;
using UnityEngine;

// ʹ��CreateAssetMenu���ԣ����ǿ�����Unity�༭���� "Create" �˵���ֱ�Ӵ���������Դ�ļ���
[CreateAssetMenu(fileName = "New CardData", menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    [Header("���ƻ�����Ϣ")]
    public string id;
    public string cardName;
    public CardType cardType;
    public Attributes attributes; // ���Ƶ�����
    public string description; // ���Ƶ�����

    [Header("�����Ӿ�����")]
    public Sprite artwork; // ���Ƶ�ͼ��
                           // public GameObject cardPrefab; // ��������и��ӵ�3Dģ�ͻ���Ч����������������

    // ��ȡ����
    public Attributes GetAttributes()
    {
        return attributes;
    }

    // �������ԣ������滻��
    public void SetAttributes(Attributes newAttributes)
    {
        attributes = newAttributes;
    }

    // ��������ĳ������
    public void SetAttributeValue(string attrName, int value)
    {
        switch (attrName)
        {
            case "physique": attributes.physique = value; break;
            case "social": attributes.social = value; break;
            case "survival": attributes.survival = value; break;
            case "intelligence": attributes.intelligence = value; break;
            case "charm": attributes.charm = value; break;
            case "combat": attributes.combat = value; break;
            case "support": attributes.support = value; break;
        }
    }

    // ������ȡĳ������
    public int GetAttributeValue(string attrName)
    {
        return attrName switch
        {
            "physique" => attributes.physique,
            "social" => attributes.social,
            "survival" => attributes.survival,
            "intelligence" => attributes.intelligence,
            "charm" => attributes.charm,
            "combat" => attributes.combat,
            "support" => attributes.support,
            _ => 0
        };
    }

    // ���Ե��ӣ���ӳɣ�
    public void AddAttributes(Attributes add)
    {
        attributes.physique += add.physique;
        attributes.social += add.social;
        attributes.survival += add.survival;
        attributes.intelligence += add.intelligence;
        attributes.charm += add.charm;
        attributes.combat += add.combat;
        attributes.support += add.support;
    }

}

/// <summary>
/// �������ݶ��У����ڴ洢�͹��������ݵĶ���
/// </summary>
public class CardDataQueue
{
    private readonly Queue<CardData> queue = new Queue<CardData>();

    /// <summary>
    /// ������������ӵ�������
    /// </summary>
    /// <param name="data"></param>
    public void Enqueue(CardData data)
    {
        queue.Enqueue(data);
    }


    /// <summary>
    /// �Ӷ������Ƴ���������ǰ��Ŀ�������
    /// </summary>
    /// <returns></returns>
    public CardData Dequeue()
    {
        if (queue.Count == 0) return null;
        return queue.Dequeue();
    }

    public int Count => queue.Count;
}

/// <summary>
/// ���Խṹ�壬���ڴ洢���Ƶĸ�������ֵ
/// </summary>
[System.Serializable]
public struct Attributes
{
    public int physique;   // ����
    public int social;     // �罻
    public int survival;   // ����
    public int intelligence; // �ǻ�
    public int charm;      // ����
    public int combat;     // ս��
    public int support;    // ֧��
}