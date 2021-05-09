using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarming
{
    public class TrainSwitch : MonoBehaviour
    {

        void ChangeColl()
        {
            if (this == null) return;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.name == "TrainSwitch")
            {
                EventCenter.Instance.DispatchEvent(EventDefine.TRAIN_START);
                EventCenter.Instance.RemoveEventListener(EventDefine.CHANGE_VISUAL_STATE, ChangeColl);
                Destroy(this);
            }
            else if (coll.CompareTag("RobotPlayer") || coll.CompareTag("WindPlayer"))
            {
                EventCenter.Instance.AddEventListener(EventDefine.CHANGE_VISUAL_STATE, ChangeColl);
            }
        }
    }
}