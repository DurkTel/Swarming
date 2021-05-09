using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Swarming;
using Swarming.Controller;

public class FailView : GUIView
{
    private Text m_goldNum;

    private Image m_Bg_2;

    private Image m_exit;

    private Image m_return;
    public override void Enable()
    {
        CharactorManager.Instance.SetAllCharMotor(false);

        m_goldNum = GetControl<Text>("gold");
        m_Bg_2 = GetControl<Image>("Bg_2");
        m_exit = GetControl<Image>("Exit");
        m_return = GetControl<Image>("Main");
        m_Bg_2.DOFade(1, 1f);
        m_exit.DOFade(1, 1f);
        m_return.DOFade(1, 1f);
        m_goldNum.DOFade(1, 1f);

        m_exit.GetComponent<Button>().Select();
        m_goldNum.text = GameManager.Instance.GoldNum.ToString();
        MusicManager.Instance.PlayBkMusic("BGMusic2");

    }

    public override void Disable()
    {
        CharactorManager.Instance.ReSetCharMotor();
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Exit":
                Application.Quit();
                break;
            case "Main":
                Transform effect = GameManager.Instance.WindPlayer.transform.GetChild(0).Find("effect");
                effect.Find("Energy").gameObject.SetActive(false);
                //GameManager.Instance.windMotor.m_animator.SetLayerWeight(2, 0);
                UIManager.Instance.OpenView<LoadingView>("Loading/Loading_View", UI_Layer.System);
                ScenesManager.Instance.LoadSceneAsyn("Main");
                break;
        }
    }

}
