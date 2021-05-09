using System;
using UnityEngine;
using static Swarming.Controller.BaseStates;

namespace Swarming.Controller
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class BaseMotor : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    
        [SerializeField] private AudioClip m_JumpSound;           
        [SerializeField] private AudioClip m_LandSound;

        private int m_currentIndex;
        private bool m_Jump;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        protected CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        protected BaseStates m_State;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private float rotateDir;
        private AudioSource m_AudioSource;
        public bool isJumping;

        public bool isEnable;

        public bool specialVision;

        protected virtual void Start()
        {
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_CharacterController = GetComponent<CharacterController>();
            m_State = GetComponent<BaseStates>();
        }


        protected virtual void LateUpdate()
        {
            if (!isEnable) return;
            if (InputManager.Instance.Degree > 0.1f)
                EventCenter.Instance.DispatchEvent(EventDefine.WALK, true);
            else
                EventCenter.Instance.DispatchEvent(EventDefine.WALK, false);
            if (!m_Jump)
            {
                m_Jump = InputManager.Instance.JumpTrigger;
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                PlayLandingSound();
                m_MoveDir.y = 0;
                m_Jumping = false;
                EventCenter.Instance.DispatchEvent(EventDefine.JUMP_END);
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
            //if(!m_PreviouslyGrounded)
            //    EventCenter.Instance.DispatchEvent(EventDefine.JUMP);

        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        protected virtual void FixedUpdate()
        {
            m_Input = InputManager.Instance.IPutVec;
            Vector3 desiredMove = Vector3.forward * m_Input.y + Vector3.right * m_Input.x;
            RaycastHit hitInfo;
            //获得平面法线向量
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            //获得投影向量  与接触面平行的向量
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
            //当前角色是风且是2D 或是机器人
            if (((CharactorManager.Instance.GetCurrentChar() == CharactorManager.Charactors.windplayer &&
                m_State.GetCurrentVisual() == VisualState.orthographic) ||
                CharactorManager.Instance.GetCurrentChar() == CharactorManager.Charactors.robotPlayer) && isEnable)
            {
                if (desiredMove.x == 1)
                    rotateDir = 0;
                else if (desiredMove.x == -1)
                    rotateDir = 180;
                transform.rotation = Quaternion.Euler(new Vector3(0, rotateDir, 0));
                m_MoveDir.x = 0;
                m_MoveDir.z = desiredMove.x * m_WalkSpeed;
            }
            //风的3d特殊视角（特殊关卡）
            else if (isEnable && specialVision && CharactorManager.Instance.GetCurrentChar() == CharactorManager.Charactors.windplayer &&
                m_State.GetCurrentVisual() == VisualState.perspective)
            {
                transform.forward = Vector3.Slerp(transform.forward, desiredMove, 0.2f);
                m_MoveDir.x = -desiredMove.z * m_WalkSpeed;
                m_MoveDir.z = desiredMove.x * m_WalkSpeed;
            }
            //风的3d通常视角
            else if (isEnable)
            {
                Vector3 rejectY = new Vector3(desiredMove.x, 0, desiredMove.z);
                transform.forward = Vector3.Slerp(transform.forward, rejectY, 0.2f);
                m_MoveDir.x = desiredMove.x * m_WalkSpeed;
                m_MoveDir.z = desiredMove.z * m_WalkSpeed;
            }

            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    EventCenter.Instance.DispatchEvent(EventDefine.JUMP);
                    m_MoveDir.y = m_JumpSpeed;
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            if (!isEnable)
                m_MoveDir = new Vector3(0, m_MoveDir.y, 0);    
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
            //查看检测射线
            //Vector3 currentPos = transform.position;
            //Vector3 point_1 = new Vector3(12f, currentPos.y, currentPos.z);
            //Vector3 point_2 = new Vector3(-12f, currentPos.y, currentPos.z);
            //Debug.DrawLine(point_1, point_2, Color.red);
        }

        protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }
            if (body == null || body.isKinematic)
            {
                return;
            }
            Vector3 direction = m_CharacterController.transform.position - hit.transform.position;
            ////body.AddForceAtPosition(m_CharacterController.velocity * 7f, hit.point, ForceMode.Force);
            body.AddForceAtPosition(direction.normalized * 7f, hit.point, ForceMode.Force);
        }

        protected virtual void OnTriggerStay(Collider camCol)
        {
            //if (!isEnable) return;
            if (camCol.CompareTag("CamCol"))
            {
                int tempIndex = int.Parse(camCol.name.Substring(7,2));
                if (m_currentIndex != tempIndex)
                {
                    m_currentIndex = tempIndex;
                    EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_CAM, m_currentIndex);
                }

            }
        
        }
    }
}
