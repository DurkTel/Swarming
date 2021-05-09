using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming;
using UnityEngine.UI;
public class MainUI : MonoBehaviour
{
    private void Start()
    {
        SaveManager.Instance.Clear();
        EventCenter.Instance.EventListenerClear();
        PoolManager.Instance.Clear();
        UIManager.Instance.CloseAllView(new string[] { "Loading/Loading_View" });
        UIManager.Instance.OpenView<MenuView>("Menu/MainMenu_View", UI_Layer.System);
        //UIManager.Instance.OpenView<LoadingView>("Loading/Loading_View", UI_Layer.System);
        ScenesManager.Instance.LoadSceneAsyn("Level_1", () =>
        {
            GameManager.Instance.Initialize();
            SaveManager.Instance.Load(true);
            GameManager.Instance.windMotor.isEnable = false;
            Camera.main.GetComponent<Camera>().orthographic = false;
            MusicManager.Instance.PlayBkMusic("BGMusic2");
            MusicManager.Instance.ChangeBKValue(0.6f);
            UIManager.Instance.CloseView("Loading/Loading_View");
        });
    }

}
