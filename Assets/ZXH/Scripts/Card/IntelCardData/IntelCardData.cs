using UnityEngine;

[CreateAssetMenu(fileName = "New IntelCardData", menuName = "Card/Intel Card Data")]
public class IntelCardData : CardData
{


    private void Awake()
    {
        cardType = CardType.Intel; // ���ÿ�Ƭ����Ϊ Intel
    }
}