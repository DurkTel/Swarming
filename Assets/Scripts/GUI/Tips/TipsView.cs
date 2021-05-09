using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Tweener bger = m_bg.DOScaleY(1f, 0.5f);
        bger.onComplete = () =>
        {
            Invoke("CloseAni", tipsText.Length * 0.15f);
        };
    }

    private void CloseAni()
    {
        Tweener bger = m_bg.DOScaleY(0.1f, 0.5f);
        bger.onComplete = () =>
        {
            Close();
        };
    }

}
