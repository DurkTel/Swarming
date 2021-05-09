using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming.Controller;

namespace Swarming
{
    public class Mechanism : MonoBehaviour
    {
        enum MechanismType { Type2D, Type3D, AllType }
        enum TriggerPlayer { Robot, Wind, All }
        [SerializeField]
        private MechanismType m_mechanismType = MechanismType.Type2D;
        [SerializeField]
        private TriggerPlayer m_triggerPlayer = TriggerPlayer.Robot;

        public string EventName;

        public bool AutoClose;

        public bool NeedTrigger;

        public bool Self;


        string playerTag;
        EventDefine m_event;

        private void Start()
        {
            m_event = (EventDefine)Enum.Parse(typeof(EventDefine), EventName);//将字符串转换成eventdefine里的枚举
            playerTag = m_triggerPlayer == 0 ? "RobotPlayer" : "WindPlayer";
        }

        public virtual void Action(GameObject player)
        {
            print("触发机关......................事件名字为" + m_event.ToString());
            this.gameObject.SetActive(!AutoClose);
            EventCenter.Instance.DispatchEvent(m_event, player);
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (m_triggerPlayer == TriggerPlayer.All && (coll.CompareTag("WindPlayer") || coll.CompareTag("RobotPlayer")))
            {
                GameObject obj = Self ? this.gameObject : coll.gameObject;
                Action(obj);
            }
            else if (coll.CompareTag(playerTag) && !NeedTrigger)
            {
                GameObject obj = Self ? this.gameObject : coll.gameObject;
                Action(obj);
            }
        }

        private void OnTriggerStay(Collider coll)
        {
            if (coll.CompareTag(playerTag) && NeedTrigger && InputManager.Instance.InteractTrigger)
            {
                GameObject obj = Self ? this.gameObject : coll.gameObject;
                Action(obj);
            }
        }

    }
}
