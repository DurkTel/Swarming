using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swarming.Controller
{
    public class WindMotor : BaseMotor,IChangeVisual
    {
        public Animator m_animator;
        private float m_radius;
        private bool isRot;

        protected override void Start()
        {
            base.Start();
            //m_WindState = GetComponent<WindState>();
            //m_rigidbody = GetComponent<Rigidbody>();
            m_animator = GetComponentInChildren<Animator>();
            m_State = GetComponent<WindState>();
            m_radius = GetComponent<CharacterController>().radius;
            //InitEvent();
        }

        public void InitEvent()
        {
            EventCenter.Instance.AddEventListener(EventDefine.CHANGE_VISUAL_STATE, ChangeVisual);
            EventCenter.Instance.AddEventListener(EventDefine.INTERACTION_KEY, ShotEffect);
            EventCenter.Instance.AddEventListener(EventDefine.JUMP_KEY, JumpEffect);

        }

        public void ChangeVisual()
        {
            CameraManager.Instance.m_CameraProjection.CamEffect();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            //CheckHinder();
            //float weight = isRot ? 0f : 1f;
            //m_animator.SetLayerWeight(1, weight);
            //isRot = !isRot;
            //switch (m_State.GetCurrentVisual())
            //{
            //    case VisualState.perspective:
            //        m_animator.SetLayerWeight(1, 1.0f);
            //        break;
            //    case VisualState.orthographic:
            //        m_animator.SetLayerWeight(1, 0.0f);
            //        break;
            //    default:
            //        break;
            //}
            Physics.SyncTransforms();
        }

        private void ShotEffect()
        {
            if (isEnable && CharactorManager.Instance.GetCurrentChar() == CharactorManager.Charactors.windplayer)
                PoolManager.Instance.Get("Effect/DashBlue", (p) => {
                    MusicManager.Instance.PlaySound("windIn", false, 0.3f);
                    p.transform.position = transform.position;
                    p.transform.rotation = transform.rotation;
                });
        }

        private void JumpEffect()
        {
            if (isEnable && CharactorManager.Instance.GetCurrentChar() == CharactorManager.Charactors.windplayer)
                PoolManager.Instance.Get("Effect/BeamupCloudBlue", (p) => {
                    p.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                    MusicManager.Instance.PlaySound("windJump", false, 0.5f);
                });
        }

        //private void Update()
        //{
        //Vector3 currentPos = transform.position;
        //Vector3 point_1 = new Vector3(currentPos.x - 12f, currentPos.y, currentPos.x);
        //Vector3 point_2 = new Vector3(currentPos.x + 12f, currentPos.y, currentPos.x);
        //var colls = Physics.OverlapCapsule(point_1, point_2, m_radius, LayerMask.GetMask("Ground"));
        //foreach (var item in colls)
        //{
        //    print(item.name);
        //}
        //}

        /// <summary>
        ///在切换回2D状态时检测两边是否有障碍物 
        /// </summary>
        private void CheckHinder()
        {
            RaycastHit raycastHit;
            Vector3 currentPos = transform.position;
            Vector3 point_1 = new Vector3(12f, currentPos.y, currentPos.z);
            Vector3 point_2 = new Vector3(-12f, currentPos.y, currentPos.z);
            var colls = Physics.Linecast(point_1, point_2, out raycastHit, 1 << 8);
            if (colls)
            {
                transform.position = GameManager.Instance.RobotPlayer.transform.position + new Vector3(0, 2f, 0);
            }
            else
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }


    }
}
