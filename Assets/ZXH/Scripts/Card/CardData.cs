// CardData.cs
using UnityEngine;

// ʹ��CreateAssetMenu���ԣ����ǿ�����Unity�༭���� "Create" �˵���ֱ�Ӵ���������Դ�ļ���
[CreateAssetMenu(fileName = "New CardData", menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    [Header("���ƻ�����Ϣ")]
    public string id;
    public string cardName;
    public CardType cardType;

    [Header("�����Ӿ�����")]
    public Sprite artwork; // ���Ƶ�ͼ��
    // public GameObject cardPrefab; // ��������и��ӵ�3Dģ�ͻ���Ч����������������
}