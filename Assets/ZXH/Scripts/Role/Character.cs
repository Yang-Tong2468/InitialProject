using Opsive.UltimateInventorySystem.Core.InventoryCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("��ɫ��������")]
    protected string characterName;
    protected int currentHealth = 30;
    protected int maxHealth = 100;
    protected int damage;
    protected int armor;

    public Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    [ContextMenu("����2��Armor������")]
    public void AddToInventory()
    {
        
        inventory.AddItem("Armor",2);
    }

    public void Heal(int amount)
    {
        if(currentHealth <= 0) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
