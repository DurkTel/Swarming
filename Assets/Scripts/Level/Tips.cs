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
            UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Mid, (p) => p.tipsText = "请按L键进行互动");
            if (isClose) Destroy(gameObject);
        }
    }
}
