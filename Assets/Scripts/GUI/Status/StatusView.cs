using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming;
using UnityEngine.UI;

public class StatusView : GUIView
{
    private Text m_goldNum;
    private Image[] lifes;
    public override void Enable()
    {
        m_goldNum = GetControl<Text>("gold");
        lifes = new Image[3] { GetControl<Image>("life_1"), GetControl<Image>("life_2"), GetControl<Image>("life_3") };
        UpdateGold();
        UpdateLife();
        EventCenter.Instance.AddEventListener(EventDefine.GOLDUPDATE, UpdateGold);
        EventCenter.Instance.AddEventListener(EventDefine.LIFEUPDATE, UpdateLife);
    }

    public override void Disable()
    {
        EventCenter.Instance.RemoveEventListener(EventDefine.GOLDUPDATE, UpdateGold);
        EventCenter.Instance.RemoveEventListener(EventDefine.LIFEUPDATE, UpdateLife);
    }

    private void UpdateGold()
    {
        int goldNum = GameManager.Instance.GoldNum;
        m_goldNum.text = goldNum.ToString();
    }

    private void UpdateLife()
    {
        int life = GameManager.Instance.Life;
        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[i].enabled = life - 1 >= i;
        }
    }
}
