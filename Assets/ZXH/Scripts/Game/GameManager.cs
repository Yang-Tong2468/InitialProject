using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Time = 1; // ��Ϸʱ�䣬��ʼֵΪ1

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


    /// <summary>
    /// ������Ϸʱ�䣬ÿ�ε�������1��
    /// </summary>
    public void AddTime()
    {
        Time++;
        Debug.Log($"Game Time increased to: {Time}");

        // �������м���� Event_ZXH ������ AddTime������д�¼���������
        var eventPanels = GameObject.FindObjectsOfType<Event_ZXH>(true);
        foreach (var eventPanel in eventPanels)
        {
            // ֻ�Լ�����¼�������
            if (eventPanel.gameObject.activeInHierarchy)
            {
                eventPanel.AddTime();
            }
        }
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
            Debug.Log($"�ɹ����: {testEvent.SuccessfulResults}");
            Debug.Log($"ʧ�ܽ��: {testEvent.FailedResults}");

            // ��һ�����Ǹ������·��ȥ���ز�ʵ����UIԤ������
            GameObject eventUIInstance = DataManager.Instance.InstantiateEventPrefab(testEvent, EventUIContainer);
            Event_ZXH eventUI = eventUIInstance.GetComponentInChildren<Event_ZXH>();
            eventUI.Initialize(testEvent);
        }
    }
}
