using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : SingletonBase<EventCenter>
{
    public Dictionary<EventDefine, UnityAction<object>> eventDic = new Dictionary<EventDefine, UnityAction<object>>();
    public Dictionary<EventDefine, UnityAction> eventDic_2 = new Dictionary<EventDefine, UnityAction>();

    /// <summary>
    /// 添加事件侦听
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">回调函数</param>
    public void AddEventListener(EventDefine name, UnityAction<object> action)
    {
        if (eventDic.ContainsKey(name))
        {
            if (Array.IndexOf(eventDic[name].GetInvocationList(), action) == -1)
                eventDic[name] += action;
        }
        else
        {
            eventDic.Add(name, action);
        }
    }
    /// <summary>
    /// 添加事件侦听
    /// </summary>
    /// <param name="name">事件名</param>
    public void AddEventListener(EventDefine name, UnityAction action)
    {
        if (eventDic_2.ContainsKey(name))
        {
            if (Array.IndexOf(eventDic_2[name].GetInvocationList(), action) == -1)
                eventDic_2[name] += action;
        }
        else
        {
            eventDic_2.Add(name, action);
        }
    }

    /// <summary>
    /// 发布事件
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="parameter">回调参数</param>
    public void DispatchEvent(EventDefine name,object parameter)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name].Invoke(parameter);
        }
    }

    /// <summary>
    /// 发布事件
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="parameter">回调参数</param>
    public void DispatchEvent(EventDefine name)
    {
        if (eventDic_2.ContainsKey(name))
        {
            eventDic_2[name].Invoke();
        }
    }

    /// <summary>
    /// 移除事件侦听
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">要移除的回调函数</param>
    public void RemoveEventListener(EventDefine name, UnityAction<object> action)
    {
        if (eventDic.ContainsKey(name))
        {
            Debug.Log(name);
            eventDic[name] -= action;
            if(eventDic[name] == null)
                eventDic.Remove(name);
        }
    }

    /// <summary>
    /// 移除事件侦听
    /// </summary>
    /// <param name="name">事件名</param>
    public void RemoveEventListener(EventDefine name, UnityAction action)
    {
        if (eventDic_2.ContainsKey(name))
        {
            eventDic_2[name] -= action;
            if (eventDic_2[name] == null)
                eventDic_2.Remove(name);
        }
    }

    /// <summary>
    /// 移除所有事件
    /// </summary>
    public void EventListenerClear()
    {
        
        eventDic.Clear();
        eventDic_2.Clear();
    }
}
