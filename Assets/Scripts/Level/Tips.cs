using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    public bool isClose;
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("RobotPlayer") || coll.CompareTag("WindPlayer"))
        {
            UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Top, (p) => p.tipsText = "似乎是可以与某个主角进行互动的物品哦");
            if (isClose) Destroy(gameObject);
        }
    }
}
