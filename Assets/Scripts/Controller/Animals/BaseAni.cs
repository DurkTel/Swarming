using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Swarming.Controller
{
    public class BaseAni : MonoBehaviour
    {
        public BaseMotor m_motor;
        Transform m_effectRoot;
        Animator m_animator;
        ParticleSystem m_runParticle;
        private int counter = 0;
        private AudioSource m_walkSource;
        private float m_walkInterval = 0.5f;
        private float m_walkTimer = 0.6f;
        private bool m_jumping = false;

        void Start()
        {
            m_animator = GetComponentInChildren<Animator>();

            m_effectRoot = transform.parent.Find("effect");
            m_runParticle = m_effectRoot.Find("Raise1").GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!m_motor.isEnable)
                m_runParticle.gameObject.SetActive(false);

        }

        public void InitEvent()
        {
            EventCenter.Instance.AddEventListener(EventDefine.WALK, WalkAni);
            EventCenter.Instance.AddEventListener(EventDefine.JUMP, JumpAniBegin);
            EventCenter.Instance.AddEventListener(EventDefine.JUMP_END, JumpAniEnd);
            EventCenter.Instance.AddEventListener(EventDefine.ROLL, RollAni);
            EventCenter.Instance.AddEventListener(EventDefine.ROBOT_OPEN, OpenAni);
        }

        void WalkAni(object value)
        {
            bool isWalk = (bool)value;
            if (!m_motor.isEnable || !m_animator) return;
            m_animator.SetBool("Walk_Anim", isWalk);
            m_runParticle.gameObject.SetActive(isWalk);
            if (isWalk && m_walkTimer >= m_walkInterval)
            {
                m_walkSource = MusicManager.Instance.PlaySound("robotWalk", false, 0.3f);
                m_walkTimer = 0;
            }
            else if (!isWalk || m_jumping)
            {
                MusicManager.Instance.StopSound(m_walkSource);  
            }
            m_walkTimer += Time.deltaTime;
        }

        void JumpAniBegin()
        {
            if (!m_motor.isEnable || !m_animator) return;
            m_jumping = true;
            m_animator.SetTrigger("Jump_Anim");
            m_animator.SetBool("JumpEnd_Anim", false);
            MusicManager.Instance.PlaySound("robotJump", false, 0.3f);
        }

        void JumpAniEnd()
        {
            if (!m_motor.isEnable || !m_animator) return;
            m_jumping = false;
            m_animator.SetBool("JumpEnd_Anim", true);
        }

        void RollAni(object value)
        {
            //if (!m_motor.isEnable || !m_animator) return;
            m_animator.SetBool("Roll_Anim", (bool)value);
        }

        void OpenAni(object value)
        {
            if (!m_animator) return;
            CharactorManager.Instance.isChanging = true;
            bool isOpen = (bool)value;
            string sourceName = isOpen ? "rotbotOpen" : "robotClose";
            m_runParticle.gameObject.SetActive(false);
            MusicManager.Instance.PlaySound(sourceName, false, 0.8f);
            m_animator.SetBool("Open_Anim", isOpen);
            if (isOpen)
            {
                GameManager.Instance.robotMotor.isEnable = false;
                StartCoroutine(AniCallBack(m_animator, () =>
                {
                    GameManager.Instance.robotMotor.isEnable = true;
                    CharactorManager.Instance.isChanging = false;

                    if (counter++ == 0)
                    {
                        UIManager.Instance.OpenView("Dialog/DialogBarView", UI_Layer.Mid, (DialogView p) => { p.order = "0-2"; });
                    }
                }));
            }
            else
            { 
                StartCoroutine(AniCallBack(m_animator,()=> CharactorManager.Instance.isChanging = false));
            }
        }

        IEnumerator AniCallBack(Animator ani, UnityAction callBack)
        {
            yield return new WaitForSeconds(3.5f);
            callBack();
        }

    }
}
