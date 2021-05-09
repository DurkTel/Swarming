using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollTest : MonoBehaviour
{
    public float RollSpeed;
    private Vector3 m_MoveDir = Vector3.zero;
    private bool m_Jump;
    private bool m_Jumping;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;

    protected CharacterController m_CharacterController;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_GravityMultiplier;



    Rigidbody m_rig;
    protected virtual void Start()
    {
        m_Jumping = false;
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        ////m_rig.velocity = new Vector3(0, moveY, RollSpeed);
    }
    private void LateUpdate()
    {
        if (!m_Jump)
        {
            m_Jump = Input.GetKeyDown(KeyCode.Space);
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            m_MoveDir.y = 0;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }

    private void FixedUpdate()
    {
        Vector3 desiredMove = Vector3.forward * 10;
        RaycastHit hitInfo;
        //获得平面法线向量
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                           m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        //获得投影向量
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
        m_MoveDir.x = desiredMove.x * RollSpeed;
        m_MoveDir.z = desiredMove.z * RollSpeed;
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
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
    }
}
