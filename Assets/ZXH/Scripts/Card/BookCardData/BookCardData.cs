using UnityEngine;

[CreateAssetMenu(fileName = "New BookCardData", menuName = "Card/Book Card Data")]
public class BookCardData : CardData
{


    private void Awake()
    {
        cardType = CardType.Book; // ���ÿ�Ƭ����Ϊ Book
    }
}