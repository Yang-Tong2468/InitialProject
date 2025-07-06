using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Transform EventUIContainer; // �¼�UI����

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

    void Start()
    {
        // �ȴ�һ֡ȷ��DataManager��Awake�Ѿ�ִ�����
        Invoke("TestEventLoading", 1f);
    }

    void TestEventLoading()
    {
        // ͨ��ID��ȡ�������õ��¼�
        EventData testEvent = DataManager.Instance.GetEventByID("E1001");

        if (testEvent != null)
        {
            Debug.Log("--- Event E1001 Loaded Successfully! ---");
            Debug.Log($"�¼�����: {testEvent.EventName}");
            Debug.Log($"�¼�����: {testEvent.EventType}");
            Debug.Log($"����: {testEvent.Story}");
            Debug.Log($"��������: {string.Join(" & ", testEvent.RequiredAttributes)}");
            Debug.Log($"��������: {testEvent.DurationDays}");
            Debug.Log($"�ɹ�������Ʒ: {string.Join(", ", testEvent.RewardItemIDs)}");
            Debug.Log($"Prefab·��: {testEvent.EventPrefabName}");

            // ��һ�����Ǹ������·��ȥ���ز�ʵ����UIԤ������
            GameObject eventUIInstance = DataManager.Instance.InstantiateEventPrefab(testEvent, EventUIContainer);
            Event_ZXH eventUI = eventUIInstance.GetComponentInChildren<Event_ZXH>();
            eventUI.Initialize(testEvent);
        }
    }
}
