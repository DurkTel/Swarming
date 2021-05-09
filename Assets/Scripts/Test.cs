using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming;
public class Test : MonoBehaviour
{
    private void Awake()
    {
        EventCenter.Instance.EventListenerClear();
        PoolManager.Instance.Clear();
        SaveManager.Instance.Clear();
        UIManager.Instance.OpenView<MenuView>("Menu/MainMenu_View");

        ScenesManager.Instance.LoadSceneAsyn("Scenes/Level_1", () =>
        {
            GameManager.Instance.Initialize();
        });
    }
}
