using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming.Controller;

namespace Swarming
{
    public class DeathB : MonoBehaviour
    {
        public GameObject rollBall;

        private void OnTriggerEnter(Collider other)
        {
            MusicManager.Instance.PlaySound("死亡", false);

            SaveManager.Instance.Load();
            GameManager.Instance.robotMotor.isEnable = true;
            GameManager.Instance.RobotPlayer.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

            //GameManager.Instance.RobotPlayer.SetActive(true);
            rollBall.transform.position = new Vector3(0, 6.32f, 130.8f);
            rollBall.SetActive(false);
            CharactorManager.Instance.ChangeChar(CharactorManager.Charactors.robotPlayer);
            CameraManager.Instance.ChangeChar();
            //EventCenter.Instance.DispatchEvent(EventDefine.ROBOT_OPEN, true);
            GameManager.Instance.GetHurt();

            Physics.SyncTransforms();
        }
    }
}