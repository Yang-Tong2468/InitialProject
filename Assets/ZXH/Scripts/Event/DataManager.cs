using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// ���ݹ�������������غʹ洢�����¼�����
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // ʹ���ֵ�洢�����¼���Key��EventID��Value���¼����ݣ����ǿ���֪���¼�������������IDȻ�����õ�����
    // ͨ��ID�����¼�
    private Dictionary<string, EventData> eventDatabase = new Dictionary<string, EventData>();
    private Dictionary<string, string> eventNameToIdMap = new Dictionary<string, string>();


    // CSV�ļ����������Ժ��������� Resources �ļ�����
    private const string EVENT_DATA_FILE_NAME = "EventData/EventData_ZXH";
    // Լ�������¼�UIԤ�����ŵ�Resources��·��
    public const string EVENT_PREFAB_FOLDER_PATH = "EventPrefabs/";

    void Awake()
    {
        // ʵ�ֵ���ģʽ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ȷ�����л�����ʱ���ݹ�������������
            LoadEventData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �� Resources �ļ��м��ز������¼�����CSV�ļ�
    /// </summary>
    private void LoadEventData()
    {
        // ��Resources�ļ��м����ı��ļ�
        TextAsset csvFile = Resources.Load<TextAsset>(EVENT_DATA_FILE_NAME);
        if (csvFile == null)
        {
            Debug.LogError($"Failed to load event data file: '{EVENT_DATA_FILE_NAME}.csv'. Make sure it's in a 'Resources' folder.");
            return;
        }

        // ���зָ��ı����ݣ���������һ�У���ͷ��
        //ÿһ�е�����
        string[] lines = csvFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            //�����¼���ÿһ����������
            string[] values = lines[i].Split(','); // �򵥵�CSV����

            // ע�⣺���ּ򵥵�Split��ʽ��������ı����ݣ���Story���а������ţ��ᵼ�½�������    
            // ���������н���ʹ�ø���׳��CSV�����⣬������ԭ�ͣ������㹻   

            if (values.Length > 0 && !string.IsNullOrEmpty(values[0]))
            {
                EventData newEvent = new EventData(values);
                if (!eventDatabase.ContainsKey(newEvent.EventID))
                {
                    eventDatabase.Add(newEvent.EventID, newEvent);

                    //������ֵ�ID��ӳ���ֵ�
                    if (!eventNameToIdMap.ContainsKey(newEvent.EventName))
                    {
                        eventNameToIdMap.Add(newEvent.EventName, newEvent.EventID);
                    }
                    else
                    {
                        // ���������¼��ľ���
                        Debug.LogWarning($"Duplicate EventName found: '{newEvent.EventName}'. Searching by this name will only return the first loaded event with ID '{eventNameToIdMap[newEvent.EventName]}'.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Duplicate EventID found: {newEvent.EventID}. The later entry will be ignored.");
                }
            }
        }

        Debug.Log($"Successfully loaded {eventDatabase.Count} events into the database.");
    }


    #region �����ӿ�
    /// <summary>
    /// ����ID��ȡ�¼�����
    /// </summary>
    /// <param name="eventID">�¼���ΨһID</param>
    /// <returns>�����¼����ݣ����δ�ҵ��򷵻�null</returns>
    public EventData GetEventByID(string eventID)
    {
        if (eventDatabase.TryGetValue(eventID, out EventData eventData))
        {
            return eventData;
        }

        Debug.LogWarning($"Event with ID '{eventID}' not found in the database.");
        return null;
    }

    /// <summary>
    /// �����¼����ƻ�ȡ�¼�����
    /// </summary>
    public EventData GetEventByName(string eventName)
    {
        if (eventNameToIdMap.TryGetValue(eventName, out string eventID))
        {
            // ��ͨ�������ҵ�ID����ͨ��ID��ȡ��������
            return GetEventByID(eventID);
        }

        Debug.LogWarning($"Event with Name '{eventName}' not found in the database.");
        return null;
    }

    /// <summary>
    /// ���ز�ʵ����һ���¼���UIԤ����
    /// </summary>
    /// <param name="eventData">ҪΪ�����Ԥ������¼�</param>
    /// <param name="parent">ʵ������Ԥ����Ҫ���صĸ��ڵ�</param>
    /// <returns>����ʵ������GameObject�����ʧ���򷵻�null</returns>
    public GameObject InstantiateEventPrefab(EventData eventData, Transform parent = null)
    {
        if (eventData == null || string.IsNullOrEmpty(eventData.EventPrefabName))
        {
            Debug.LogError("Cannot instantiate prefab: EventData is null or PrefabName is empty.");
            return null;
        }

        // ƴ��������Resources·��
        string prefabPath = EVENT_PREFAB_FOLDER_PATH + eventData.EventPrefabName;

        // ��Resources����Ԥ����
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"Failed to load event prefab from path: 'Resources/{prefabPath}'. Make sure the prefab exists and the name is correct.");
            return null;
        }

        // ʵ���������ø��ڵ�
        return Instantiate(prefab, parent);
    }
    #endregion
}
