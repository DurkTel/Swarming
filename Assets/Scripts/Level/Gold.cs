using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarming 
{
    public class Gold : MonoBehaviour
    {
        public GameObject gameObj;
        private void OnTriggerEnter(Collider coll)
        {
            if (coll.CompareTag("RobotPlayer") || coll.CompareTag("WindPlayer"))
            {
                MusicManager.Instance.PlaySound("道具拾取", false);
                if (GameManager.Instance.GoldNum == 0) UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Mid, (i) => { i.tipsText = "获得金币！金币可以衡量您对关卡的完成度哦！"; });
                GameManager.Instance.GoldNum++;
                EventCenter.Instance.DispatchEvent(EventDefine.GOLDUPDATE);
                if (gameObj != null)
                    Destroy(gameObj);
                Destroy(gameObject);
            }
        }
    }
}