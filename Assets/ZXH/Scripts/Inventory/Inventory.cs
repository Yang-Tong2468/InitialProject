using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͨ�õ���Ʒ����ϵͳ����������
/// </summary>
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public Backpack Backpack;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
