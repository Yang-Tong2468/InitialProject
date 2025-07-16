using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// װ������/��λ
/// </summary>
public enum EquipmentType
{
    Weapon,//����
    Armor,//����
    Amlet,//�����
}

/// <summary>
/// װ������������
/// </summary>
[CreateAssetMenu(fileName = "New EquipCardData", menuName = "Card/Equip Card Data")]
public class EquipCardData : CardData
{
    public EquipmentType equipmentType;

    public List<CardData> craftMaterials;// �������ϡ�����ֻ�ǵ��ſ��Ϳ��Ժϳɣ�����3�Ž𽣿�������


    private void Awake()
    {
        cardType = CardType.Equip; 
    }
}
