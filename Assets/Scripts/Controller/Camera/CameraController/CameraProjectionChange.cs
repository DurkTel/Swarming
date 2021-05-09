using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Swarming
{
    public class CameraProjectionChange : MonoBehaviour
    {
        /// <summary>
        /// 相机透视改变是否触发(调用只需把此值改为true)
        /// </summary>
        public bool ChangeProjection = false;
        private bool _changing = false;
        public float ProjectionChangeTime = 0.5f;
        private float _currentT = 0.0f;
        private EdgeDetectNormalsAndDepth cameraEffect;
        public GameObject Camera3D;
        public GameObject Camera2D;

        private void Start()
        {
            Resources.Load<Material>("Materials/LowPoryEX").shader = Shader.Find("Unlit/Lambert_Template_test");
            cameraEffect = GetComponent<EdgeDetectNormalsAndDepth>();
        }

        private void Update()
        {///检测，避免变换过程中发生混乱
            if (_changing)
            {
                ChangeProjection = false;
            }
            else if (ChangeProjection)
            {
                _changing = true;
                _currentT = 0.0f;
            }
        }

        private void LateUpdate()
        {
            if (!_changing)
            {
                return;
            }
            //将当前的 是否正视图值 赋值给currentlyOrthographic变量
            bool currentlyOrthographic = Camera.main.orthographic;
            //定义变量存放当前摄像机的透视和正视矩阵信息；
            Matrix4x4 orthoMat, persMat;
            if (currentlyOrthographic)//如果当前摄像机为正视状态
            {
                orthoMat = Camera.main.projectionMatrix;

                Camera.main.orthographic = false;
                Camera.main.ResetProjectionMatrix();
                persMat = Camera.main.projectionMatrix;
            }
            else//否则当前摄像机为透视状态
            {
                persMat = Camera.main.projectionMatrix;

                Camera.main.orthographic = true;
                Camera.main.ResetProjectionMatrix();
                orthoMat = Camera.main.projectionMatrix;
            }
            Camera.main.orthographic = currentlyOrthographic;

            _currentT += (Time.deltaTime / ProjectionChangeTime);
            if (_currentT < 1.0f)
            {
                if (currentlyOrthographic)
                {
                    Camera.main.projectionMatrix = MatrixLerp(orthoMat, persMat, _currentT * _currentT);
                }
                else
                {
                    Camera.main.projectionMatrix = MatrixLerp(persMat, orthoMat, Mathf.Sqrt(_currentT));
                }
            }
            else
            {
                _changing = false;
                Camera.main.orthographic = !currentlyOrthographic;
                //Camera.main.ResetProjectionMatrix();
            }
        }

        private Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
        {
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            Matrix4x4 newMatrix = new Matrix4x4();
            newMatrix.SetRow(0, Vector4.Lerp(from.GetRow(0), to.GetRow(0), t));
            newMatrix.SetRow(1, Vector4.Lerp(from.GetRow(1), to.GetRow(1), t));
            newMatrix.SetRow(2, Vector4.Lerp(from.GetRow(2), to.GetRow(2), t));
            newMatrix.SetRow(3, Vector4.Lerp(from.GetRow(3), to.GetRow(3), t));
            return newMatrix;
        }

        public void CamEffect()
        {
            if (_changing) return;
            MusicManager.Instance.PlaySound("up", false, 0.8f);
            StartCoroutine(CamEffectStart(true, 1f, () => 
            {
                MusicManager.Instance.PlaySound("down", false, 0.8f);
                StartCoroutine(CamEffectStart(false, 1f)); ; 
            },()=> 
            {
                ChangeCamProjection();
                Transform windTran = GameManager.Instance.windMotor.transform;
                windTran.position = new Vector3(GameManager.Instance.RobotPlayer.transform.position.x, windTran.position.y, windTran.position.z);
                Physics.SyncTransforms();

            }));
        }

        private  IEnumerator CamEffectStart(bool order, float time, UnityAction callback = null,UnityAction callback2 = null)
        {
            float currentTime = 0f;
            float eO = order ? 0f : 1f;
            float sD = order ? 0.4f : 1f;
            float seD = order ? 0f : 8f;
            float sN = order ? 0f : 6f;
            float eOE = order ? 1f : 0f;
            float sDE = order ? 1f : 0.4f;
            float seDE = order ? 8f : 0f;
            float sNE = order ? 6f : 0f;
            while (currentTime < time)
            {
                yield return new WaitForFixedUpdate();
                cameraEffect.edgesOnly = Mathf.Lerp(eO,eOE,currentTime);
                cameraEffect.sampleDistance = Mathf.Lerp(sD, sDE, currentTime);
                cameraEffect.sensitivityDepth = Mathf.Lerp(seD, seDE, currentTime);
                cameraEffect.sensitivityNormals = Mathf.Lerp(sN, sNE, currentTime);
                currentTime += Time.deltaTime / time;
            }
            if (callback2 != null)
                callback2();
            if (callback != null)
                callback();
        }

        private void ChangeCamProjection()
        {
            bool state = Camera.main.orthographic;
            ChangeProjection = true;
            Camera2D.SetActive(!state);
            Camera3D.SetActive(state);
            if (Camera2D.activeSelf)
            {
                Resources.Load<Material>("Materials/LowPoryEX").shader = Shader.Find("Unlit/Lambert_Template_test");
            }
            else
            { 
                Resources.Load<Material>("Materials/LowPoryEX").shader = Shader.Find("Unlit/Lambert");
            }
        }
    }
}
