using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming.Controller;
using UnityEngine.UI;
using DG.Tweening;

public class TipsView : GUIView
{
    private Text m_text;
    private Transform m_bg;

    [HideInInspector]public string tipsText;

    public override void Enable()
    {
        m_bg = FindChildrenGameObject("BG");
        m_text = GetControl<Text>("tipsText");
        m_text.text = tipsText;
        CharactorManager.Instance.SetAllCharMotor(false);
        Tweener bger = m_bg.DOScale(1.5f, 0.5f);
        bger.onComplete = () =>
        {
            EventCenter.Instance.AddEventListener(EventDefine.INTERACTION_KEY, CloseAni);
            //Invoke("CloseAni", tipsText.Length * 0.15f);
        };
    }

    public override void Disable()
    {
        CharactorManager.Instance.ReSetCharMotor();
        EventCenter.Instance.RemoveEventListener(EventDefine.INTERACTION_KEY, CloseAni);
    }

    private void CloseAni()
    {
        Tweener bger = m_bg.DOScale(0.2f, 0.5f);
        bger.onComplete = () =>
        {
            Close();
        };
    }

}
