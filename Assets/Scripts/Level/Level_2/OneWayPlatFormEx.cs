using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarming
{
    public class OneWayPlatFormEx : MonoBehaviour
    {
        public BoxCollider m_boxColloder;

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.CompareTag("RobotPlayer") || coll.CompareTag("WindPlayer"))
            {
                m_boxColloder.isTrigger = true;
            }
        }


    }
}
