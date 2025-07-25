﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEventManager : MonoBehaviour
{
    public static CharacterEventManager Instance { get; private set; }

    // 所有事件数据
    public List<EventData> allEventData = new List<EventData>();

    // 当前激活的事件实例
    private List<EventBase> activeEvents = new List<EventBase>();

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

    /// <summary>
    /// 注册一个事件实例
    /// </summary>
    public void RegisterEvent(EventBase evt)
    {
        if (!activeEvents.Contains(evt))
            activeEvents.Add(evt);
    }

    /// <summary>
    /// 注销一个事件实例
    /// </summary>
    public void UnregisterEvent(EventBase evt)
    {
        if (activeEvents.Contains(evt))
            activeEvents.Remove(evt);
    }

    /// <summary>
    /// 获取所有激活的事件
    /// </summary>
    public List<EventBase> GetActiveEvents()
    {
        return activeEvents;
    }

    /// <summary>
    /// 通过ID查找事件数据
    /// </summary>
    public EventData GetEventDataByID(string id)
    {
        return allEventData.Find(e => e.EventID == id);
    }

    /// <summary>
    /// 批量推进所有事件一天
    /// </summary>
    public void AddTimeToAllEvents()
    {
        foreach (var evt in activeEvents)
        {
            evt.AddTime();
        }
    }

    #region 创建、关闭事件
    /// <summary>
    /// 关闭一个事件
    /// </summary>
    /// <param name="evt"></param>
    public void CloseEvent(EventBase evt)
    {
        if (evt != null)
        {
            evt.CloseEvent();
        }
    }

    /// <summary>
    /// 创建一个新的事件实例
    /// </summary>
    /// <param name="eventID"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public EventBase CreateEvent(string eventID, Transform parent)
    {
        var eventData = DataManager.Instance.GetEventByID(eventID);
        if (eventData == null) return null;

        GameObject eventObj = DataManager.Instance.InstantiateEventPrefab(eventData, parent);
        EventBase evt = eventObj.GetComponentInChildren<EventBase>();
        if (evt != null)
        {
            evt.Initialize(eventData);
            //RegisterEvent(evt);会自动注册
        }
        return evt;
    }

    #endregion
}
