using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("��UI����")]
    public Backpack Backpack; // ����UI

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

    private void Update()
    {
        // �л�����UI����ʾ״̬
        if (Input.GetKeyDown(KeyCode.M))
        {
            Backpack.gameObject.SetActive(!Backpack.gameObject.activeSelf);
        }
    }
}
