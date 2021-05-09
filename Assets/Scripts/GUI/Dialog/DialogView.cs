using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Swarming;
using Swarming.Controller;
using UnityEngine.Events;
using DG.Tweening;

public class DialogView : GUIView
{

    public Dictionary<string, List<string>> chatDic = new Dictionary<string, List<string>>();
    [HideInInspector]public string order;//外部传值，文案的段落
    private string currentOrder;//当前要进行对话的段落 1-1  1-2  2-1.......
    private int textIndex = 1;//每一行的下标
    private bool chatTrigger;//对话触发
    private bool isFinish;//单行是否播放完
    private Text m_text;
    private TextAsset m_textAsset;
    private Transform m_BG;
    public UnityAction callBack;


    public override void Enable()
    {
        m_BG = FindChildrenGameObject("BG");
        CharactorManager.Instance.SetAllCharMotor(false);
        m_textAsset = Resources.Load("UI/Dialog/Copywriting") as TextAsset;
        m_text = GetControl<Text>("XText");
        GetDataFromAsset();
        Tweener bger = m_BG.DOScaleX(1f,1f);
        bger.onComplete = () => StartChat(order);
        EventCenter.Instance.AddEventListener(EventDefine.INTERACTION_KEY, Chat);
        //UIManager.AddCustomEventListener(m_text, EventTriggerType.PointerClick, Chat);
    }

    public override void Disable()
    {
        CharactorManager.Instance.ReSetCharMotor();
        EventCenter.Instance.RemoveEventListener(EventDefine.INTERACTION_KEY, Chat);
    }

    private void Chat()
    {
        if (order != null && chatTrigger)
        {
            if (textIndex <= chatDic[order].Count - 1 && isFinish)
                MonoManager.Instance.StartCoroutine(ChatDely(chatDic[order][textIndex++], 0.1f));
            else if (isFinish)
                EndChat();
        }
    }

    //逐字播放
    private IEnumerator ChatDely(string list, float dely)
    {
        m_text.text = "";
        isFinish = false;
        for (int i = 0; i < list.Length; i++)
        {
            m_text.text += list[i];
            yield return new WaitForSeconds(dely);
        }
        isFinish = true;
    }

    //开始对话
    private void StartChat(string order)
    {
        chatTrigger = true;
        isFinish = true;
        currentOrder = order;
        MonoManager.Instance.StartCoroutine(ChatDely(chatDic[order][textIndex++], 0.1f));
    }

    private void EndChat()
    {
        chatTrigger = false;
        currentOrder = null;
        textIndex = 1;
        if (callBack != null)
            callBack();
        UIManager.Instance.CloseView("Dialog/DialogBarView");
        //m_text.gameObject.SetActive(false);
    }

    //读取文本数据
    private void GetDataFromAsset()
    {
        var levelTextArray = m_textAsset.text.Split('\n');
        string chatKey = null;
        foreach (var item in levelTextArray)
        {
            if (item[0] == '[')
            {
                chatKey = item.Substring(1, 3);
                if (!chatDic.ContainsKey(chatKey))
                {
                    chatDic.Add(chatKey, new List<string>());
                }
            }
            chatDic[chatKey].Add(item);
        }
    }
}
