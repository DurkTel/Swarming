using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swarming.Controller
{
    public class RobotMotor : BaseMotor
    {
        //private RobotState m_RobotState;
        private Animator m_animator;
        public bool m_IsKinematic = true;
        public Vector3 PhyVect;
        private bool isRoll = false;


        protected override void Start()
        {
            base.Start();
            m_animator = GetComponentInChildren<Animator>();
            m_State = GetComponent<RobotState>();
        }

        protected override void LateUpdate()
        {
            if (m_IsKinematic)
            {
                m_CharacterController.radius = 0.8f;
                m_CharacterController.height = 1f;
                m_CharacterController.center = new Vector3(0, 0, 0);
                base.LateUpdate();
            }
            else
            {
                m_CharacterController.radius = 0.7f;
                m_CharacterController.height = 1.4f;
                m_CharacterController.center = new Vector3(0, -0.28f, 0);
            }
        }

        protected override void FixedUpdate()
        {
            if (m_IsKinematic)
                base.FixedUpdate();
            else
            {
                if (PhyVect != Vector3.zero)
                {
                    m_CharacterController.Move(PhyVect * Time.fixedDeltaTime);
                    transform.forward = Vector3.Slerp(transform.forward, PhyVect, 0.2f);

                }
                else
                { 
                    m_CharacterController.Move(Physics.gravity * 3 * Time.fixedDeltaTime);
                }
            }
        }

        protected override void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (m_IsKinematic)
                base.OnControllerColliderHit(hit);
            else
            {
                if (hit.collider.tag == "Sokoban")
                {
                    EventCenter.Instance.DispatchEvent(EventDefine.ROLL, false);
                }
                //if (hit.collider.name == "Thrust")
                //{
                //    isThrust = true;
                //PhyVect = Vector3.zero;
                //}
            }
        }


    }
}
