using UnityEngine;

[CreateAssetMenu(fileName = "New CoinCardData", menuName = "Card/Coin Card Data")]
public class CoinCardData : CardData
{
    [Header("�������")]
    public int amount;


    private void Awake()
    {
        cardType = CardType.Coin; // ���ÿ�Ƭ����Ϊ Coin
    }
}