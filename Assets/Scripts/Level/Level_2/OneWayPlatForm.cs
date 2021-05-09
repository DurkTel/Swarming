using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarming
{
    public class OneWayPlatForm : MonoBehaviour
    {
        BoxCollider m_boxColloder;
        void Start()
        {
            m_boxColloder = GetComponent<BoxCollider>();
        }

        void Update()
        {

        }


        private void OnTriggerExit(Collider coll)
        {
            if (coll.CompareTag("RobotPlayer") || coll.CompareTag("WindPlayer"))
            {
                m_boxColloder.isTrigger = false;
            }
        }


    }
}
