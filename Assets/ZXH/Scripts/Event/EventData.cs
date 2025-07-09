using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

/// <summary>
/// �洢�����¼�������Ϣ��������
/// �����̳� MonoBehaviour����һ������� C# ��������
/// </summary>
[System.Serializable]
public class EventData
{
    public string EventID { get; private set; }
    public EventType EventType { get; private set; }
    public SelectType SelectType { get; private set; }
    public string EventName { get; private set; }
    public string EventPrefabName { get; private set; } // �洢Prefab��Resources�µ�·��
    public string Story { get; private set; }// �¼��Ĺ����ı�
    public string Tips { get; private set; }// �¼�����ʾ�ı�
    public List<string> RequiredAttributes { get; private set; } // ��Ҫ�����ԣ��ѽ���Ϊ�б�
    public int DurationDays { get; private set; }// �¼�����������
    public int SuccessThreshold { get; private set; } // �ɹ�������/��ֵ��Ͷ���ĳɹ�������Ҫ���������
    public string SuccessfulResults { get; private set; } // �ɹ��Ľ���ı�
    public string FailedResults { get; private set; } // ʧ�ܵĽ���ı�

    public List<string> RewardItemIDs { get; private set; } // ������ƷID���ѽ���Ϊ�б�

    // ���캯�������𽫴�CSV��ȡ��ԭʼ�ַ������ݣ���������䵽���������
    public EventData(string[] rawData)
    {
        try
        {
            EventID = rawData[0];

            // ʹ�� Enum.Parse ���ַ���ת��Ϊö�٣�true��ʾ���Դ�Сд
            EventType = (EventType)Enum.Parse(typeof(EventType), rawData[1], true);
            SelectType = (SelectType)Enum.Parse(typeof(SelectType), rawData[2], true);

            EventName = rawData[3];
            EventPrefabName = rawData[4]; // ����ֱ�Ӵ洢�ַ���·��
            Story = rawData[5];
            Tips = rawData[6];

            // �������ŷָ����ַ���Ϊ�б�
            // Trim()����ȥ��ÿ��Ԫ��ǰ��Ŀո��Է� "a, b" �������
            RequiredAttributes = rawData[7].Split('��').Select(s => s.Trim()).ToList();

            DurationDays = int.Parse(rawData[8]);
            SuccessThreshold = int.Parse(rawData[9]);

            SuccessfulResults = rawData[10];
            FailedResults = rawData[11];

            RewardItemIDs = rawData[12].Split('��').Select(s => s.Trim()).ToList();
        }
        catch (Exception e)
        {
            // ���ĳһ�����ݸ�ʽ�������ܰ������ǿ��ٶ�λ����
            Debug.LogError($"Error parsing event data for row with ID {rawData[0]}. Check your CSV format. Error: {e.Message}");
        }
    }
}