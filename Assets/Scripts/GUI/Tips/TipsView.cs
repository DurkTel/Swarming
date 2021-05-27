using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming.Controller;
using UnityEngine.UI;
using DG.Tweening;

public class TipsView : GUIView
{
    private Text m_text;
    private Text m_text2;
    private Transform m_bg;
    private Image m_ImgSingle;
    private Image m_ImgDouble;

    [HideInInspector] public bool hadImg;
    [HideInInspector] public string tipsText;
    [HideInInspector] public bool isSingle;
    [HideInInspector] public string imgScr;

    public override void Enable()
    {
        m_bg = FindChildrenGameObject("BG");
        m_text = GetControl<Text>("tipsText");
        m_text2 = GetControl<Text>("tipsTextNoImg");
        m_ImgSingle = GetControl<Image>("TipsImgSingle");
        m_ImgDouble = GetControl<Image>("TipsImgDouble");

        m_text.text = tipsText;
        m_text2.text = tipsText;
        m_ImgSingle.sprite = ResManager.Instance.Load<Sprite>("UI/Tips/Image/" + imgScr);
        m_ImgDouble.sprite = ResManager.Instance.Load<Sprite>("UI/Tips/Image/" + imgScr);

        m_text.gameObject.SetActive(hadImg);
        m_text2.gameObject.SetActive(!hadImg);
        m_ImgSingle.gameObject.SetActive(hadImg && isSingle);
        m_ImgDouble.gameObject.SetActive(hadImg && !isSingle);

        CharactorManager.Instance.SetAllCharMotor(false);
        Tweener bger = m_bg.DOScale(1f, 0.5f);
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
