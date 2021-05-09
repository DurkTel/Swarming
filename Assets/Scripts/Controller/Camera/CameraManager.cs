using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System;

namespace Swarming.Controller
{
    public class CameraManager : SingletonBase<CameraManager>
    {
        private List<CinemachineVirtualCamera> m_cameras2D = new List<CinemachineVirtualCamera>();
        private CinemachineVirtualCamera m_cameras3D;
        private GameObject MainCamera;
        public CameraProjectionChange m_CameraProjection;

        public void Initialize()
        {
            InitCamera();
            var temp = GameObject.FindGameObjectsWithTag("Camera2D");
            m_cameras3D = GameObject.FindGameObjectWithTag("Camera3D").transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            SortByName(temp);
            for (int i = 0; i < temp.Length; i++)
            {
                m_cameras2D.Add(temp[i].GetComponent<CinemachineVirtualCamera>());
                //Debug.Log(temp[i].name);
            }
            m_cameras3D.Follow = GameManager.Instance.WindPlayer.transform;
            m_cameras3D.LookAt = GameManager.Instance.WindPlayer.transform;
        }

        //返回的数组不一定有序，按名字后的数字进行排序
        void SortByName(GameObject[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    if (Convert.ToInt32(array[j].name.Substring(7, 2)) > Convert.ToInt32(array[j + 1].name.Substring(7, 2)))
                    {
                        GameObject temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }

        void InitCamera()
        {
            m_cameras3D = null;
            m_cameras2D.Clear();
            if (MainCamera == null)
            {
                MainCamera = ResManager.Instance.Load<GameObject>("Main Camera");
                MonoController.DontDestroyOnLoad(MainCamera);
            }
            if (m_CameraProjection == null) m_CameraProjection = MainCamera.GetComponent<CameraProjectionChange>();
            m_CameraProjection.Camera2D = GameObject.Find("CM vcam2D");
            m_CameraProjection.Camera3D = GameObject.Find("CM vcam3D");
            EventCenter.Instance.AddEventListener(EventDefine.CHANGE_CHAR, ChangeChar);
            EventCenter.Instance.AddEventListener(EventDefine.CHANGE_CAM, ChangeCam);
        }

        public void ChangeChar()
        {
            foreach (var item in m_cameras2D)
            {
                if (item == null) return;
                item.Follow = CharactorManager.Instance.CurrentPlayer.transform;
                item.LookAt = CharactorManager.Instance.CurrentPlayer.transform;
            }
        }

        public void ChangeChar(Transform tran)
        {
            foreach (var item in m_cameras2D)
            {
                if (item == null) return;
                item.Follow = tran;
                item.LookAt = tran;
            }
        }

        void ChangeCam(object idx)
        {
            foreach (var item in m_cameras2D)
            {
                if (item == null) return;
                item.gameObject.SetActive(item.name.Substring(7, 2) == idx.ToString());
            }
            //m_cameras2D[(int)idx - 1].gameObject.SetActive(true);
        }
        
    }
}