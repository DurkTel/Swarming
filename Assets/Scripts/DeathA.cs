using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming.Controller;

namespace Swarming
{
    public class DeathA : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            MusicManager.Instance.PlaySound("死亡", false);
            GameManager.Instance.GetHurt();
            CharactorManager.Instance.ChangeChar(CharactorManager.Instance.GetCurrentChar());
            SaveManager.Instance.Load();
        }
    }
}