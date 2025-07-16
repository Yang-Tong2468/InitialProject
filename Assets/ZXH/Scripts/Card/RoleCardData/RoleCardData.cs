using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RoleCardData", menuName = "Card/Role Card Data")]
public class RoleCardData : CardData
{
    public List<EquipCardData> equipments = new List<EquipCardData>(); // ��ɫװ���б�
    public Dictionary<EquipmentType, EquipCardData> equipmentDictionary = new Dictionary<EquipmentType, EquipCardData>() ;// װ���ֵ䣬���ٲ���


    private void OnEnable()
    {
        cardType = CardType.Role;
    }

    /// <summary>
    /// ���\�滻װ��
    /// </summary>
    /// <param name="equip"></param>
    public void Equip(EquipCardData equip)
    {
        EquipCardData oldEquip = null;

        if(equipmentDictionary != null)
        {
            foreach (var equipment in equipmentDictionary)
            {
                if (equipment.Key == equip.equipmentType)
                {
                    oldEquip = equipment.Value;
                }
            }
        }

        // ����װ���Żؿ��
        if (oldEquip != null)
        {
            Inventory.Instance.Backpack.AddCard(oldEquip); 
            equipments.Remove(oldEquip);
            equipmentDictionary.Remove(oldEquip.equipmentType);

            // ��������
            RemoveAttributes(oldEquip.attributes);
        }

        // �����װ��
        Inventory.Instance.Backpack.RemoveCard(equip);//ˢ�¹���
        equipments.Add(equip);
        equipmentDictionary[equip.equipmentType] = equip;

        //��������
        AddAttributes(equip.attributes);

    }

    /// <summary>
    /// �Ƴ�װ��
    /// </summary>
    /// <param name="equip"></param>
    public void UnEquip(EquipCardData equip)
    {
        if (equipments.Contains(equip))
        {
            equipments.Remove(equip);
            equipmentDictionary.Remove(equip.equipmentType);

            // ��װ���Żؿ��
            Inventory.Instance.Backpack.AddCard(equip);

            // ��������
            RemoveAttributes(equip.attributes);
        }
    }

}