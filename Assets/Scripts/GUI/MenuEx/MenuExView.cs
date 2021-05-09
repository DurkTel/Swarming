using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Swarming;
using DG.Tweening;


public class MenuExView : GUIView
{
    private Transform panel;
    private Image MaskEnd;
    public override void Enable()
    {
        panel = FindChildrenGameObject("Panel");
        MaskEnd = GetControl<Image>("MaskEnd");
        GetControl<Button>("Back").Select();
        Tweener paneler = panel.DOScale(Vector3.one, 0.5f);
        paneler.onComplete = () => Time.timeScale = 0f;
    }

    public override void Disable()
    {
        
    }

    protected override void OnClick(string btnName)
    {
        Time.timeScale = 1f;
        switch (btnName)
        {
            case "Back":
                SaveManager.Instance.Load();
                Close();
                break;
            case "Main":
                Transform effect = GameManager.Instance.WindPlayer.transform.GetChild(0).Find("effect");
                effect.Find("Energy").gameObject.SetActive(false);
                //GameManager.Instance.windMotor.m_animator.SetLayerWeight(2, 0);
                panel.DOScale(Vector3.one * 0.1f, 0.5f);
                Tweener masker = MaskEnd.DOFade(1f, 1f);
                UIManager.Instance.OpenView<LoadingView>("Loading/Loading_View", UI_Layer.System);
                masker.onComplete = () => ScenesManager.Instance.LoadSceneAsyn("Main");
                break;
            case "Return":
                //UIManager.Instance.CloseView("MenuEx/MenuEx_View");
                Tweener paneler = panel.DOScale(Vector3.one * 0.1f, 0.5f);
                paneler.onComplete = Close;
                break;
            default:
                break;
        }
    }
}
