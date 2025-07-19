using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Opsive.UltimateInventorySystem.ItemActions;
using Opsive.UltimateInventorySystem.Core.DataStructures;
using System;
using Opsive.Shared.Game;
using Opsive.UltimateInventorySystem.Core.AttributeSystem;

[Serializable]
public class HealAction : ItemAction
{
    [SerializeField] protected string m_AttributeName = "HealAmount";

    /// <summary>
    /// �ж��Ƿ����ִ������
    /// </summary>
    /// <param name="itemInfo"></param>
    /// <param name="itemUser"></param>
    /// <returns></returns>
    protected override bool CanInvokeInternal(ItemInfo itemInfo, ItemUser itemUser)
    {
        var character = itemUser.gameObject.GetCachedComponent<Character>();

        if(character == null) return false;

        if(itemInfo.Item.GetAttribute(m_AttributeName) == null) return false;

        return true;
    }

    /// <summary>
    /// ִ��
    /// </summary>
    /// <param name="itemInfo"></param>
    /// <param name="itemUser"></param>
    protected override void InvokeActionInternal(ItemInfo itemInfo, ItemUser itemUser)
    {
        var character = itemUser.gameObject.GetCachedComponent<Character>();
        int healAmount = itemInfo.Item.GetAttribute<Attribute<int>>(m_AttributeName).GetValue();
        character.Heal(healAmount);

        // ʹ�� ItemInfo ����ɾ������ȷ����ȷ����
        var removeRequest = (ItemInfo)(1, itemInfo);     // �½�һ����ͬ ItemInfo ��ʾ�Ƴ� 1 ��
        var removed = itemInfo.Inventory.RemoveItem(removeRequest);

        // ��ѡ��Debug ���
        Debug.Log($"�����Ƴ� 1 �� {itemInfo.Item.name}��ʵ���Ƴ� {removed.Amount} ������Դ��ջ��ʣ�� {(removed.ItemStack?.Amount ?? 0)} ����");
    }
}
