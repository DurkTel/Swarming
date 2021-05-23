using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Swarming;
using Swarming.Controller;

public class MenuView : GUIView
{
    private GameObject panel;
    private GameObject mask2;
    public override void Enable()
    {
        panel = FindChildrenGameObject("Panel").gameObject;
        mask2 = FindChildrenGameObject("Mask_2").gameObject;
        //panel.SetActive(false);
        Image mask = GetControl<Image>("Mask");
        Image Title = GetControl<Image>("Title");
        Image StartGame = GetControl<Image>("StartGame");
        Image ExitGame = GetControl<Image>("ExitGame");
        GetControl<Button>("StartGame").Select();
        Tweener maskTweener = mask.DOFade(0, 3f);
        maskTweener.OnComplete(() => {
            Title.DOFade(1, 3f);
            StartGame.DOFade(1, 3f);
            ExitGame.DOFade(1, 3f);
        });
    }

    public override void Disable()
    {
        GameManager.Instance.windMotor.isEnable = true;

    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "StartGame":
                UIManager.Instance.OpenView<StatusView>("Status/Status_View", UI_Layer.Bot);
                //UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Top, (p) => p.tipsText = "利用WASD，SPACE，L键可以移动、跳跃和互动");
                UIManager.Instance.OpenView<DialogView>("Dialog/DialogBarView", UI_Layer.Mid, (DialogView p) => { p.order = "1-1"; });
                FindChildrenGameObject("UICamera").gameObject.SetActive(false);
                CameraManager.Instance.m_CameraProjection.ChangeProjection = true;
                panel.SetActive(false);
                mask2.SetActive(false);
                MusicManager.Instance.PlayBkMusic("BGMusic");
                break;
            case "ExitGame":
                Application.Quit();
                break;
            default:
                break;
        }
        Invoke("DelyClose", 1f);
    }

    void DelyClose()
    {
        //UIManager.Instance.CloseView("Menu/MainMenu_View");
        Close();
    }

    //IEnumerator AplByValue(Image image,float start, float end, float time)
    //{
    //    float currentTime = 0;
    //    while (image.do != scale * Vector3.one)
    //    {
    //        yield return new WaitForFixedUpdate();
    //        currentTime += Time.deltaTime / time;
    //        Vector3 currentScale = tran.transform.localScale;
    //        tran.transform.localScale = Vector3.Lerp(currentScale, scale * Vector3.one, currentTime);
    //    }
    //    callback(tran);
    //}

}
