using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Swarming
{
    public class CameraChange : MonoBehaviour
    {
        GameObject m_vCam;

        private void Start()
        {
            m_vCam = transform.GetChild(0).gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                m_vCam.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                m_vCam.SetActive(false);
        }
    }
}
