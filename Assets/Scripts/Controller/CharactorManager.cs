using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarming.Controller
{
    public class CharactorManager : SingletonBase<CharactorManager>
    {
        public enum Charactors
        { 
            robotPlayer,
            windplayer,
        }
        [SerializeField]
        private Charactors m_charactor = Charactors.windplayer;
        public GameObject CurrentPlayer;

        public bool canChangeChar = false;
        public bool canChangeVisual = false;
        private bool motor_1 = false;
        private bool motor_2 = false;

        public bool isChanging;

        public CharactorManager()
        {
            
            MonoManager.Instance.AddUpdateListener(Update, "CharactorManager");
        }
        private void Update()
        {
            if (InputManager.Instance.CharactorChangerTrigger && canChangeChar && !isChanging) ChangeChar();
            if (InputManager.Instance.ChangeTrigger && m_charactor == Charactors.windplayer && canChangeVisual)
                EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_VISUAL_STATE);
        }

        public void SetAllCharMotor(bool isActive)
        {
            isChanging = true;
            motor_1 = GameManager.Instance.robotMotor.isEnable;
            motor_2 = GameManager.Instance.windMotor.isEnable;
            GameManager.Instance.robotMotor.isEnable = isActive;
            GameManager.Instance.windMotor.isEnable = isActive;
        }

        public void ReSetCharMotor()
        {
            isChanging = false;
            GameManager.Instance.robotMotor.isEnable = motor_1;
            GameManager.Instance.windMotor.isEnable = motor_2;
        }

        public void ChangeCharTrigger(object obj)
        {
            canChangeChar = true;
            UIManager.Instance.OpenView("Dialog/DialogBarView", UI_Layer.Mid, (DialogView p) => { p.order = "0-1"; p.callBack = GetCharAble; });
            void GetCharAble()
            {
                UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Mid, (i) => { i.tipsText = "在机器人附近按K键可以切换角色！"; });
            }
        }

        public void ChangeVisualTrigger(object obj)
        {
            canChangeVisual = true;
            MusicManager.Instance.PlaySound("getAble", false, 0.6f);
            Transform effect = GameManager.Instance.WindPlayer.transform.GetChild(0).Find("effect");
            effect.Find("Energy").gameObject.SetActive(true);
            //GameManager.Instance.windMotor.m_animator.SetLayerWeight(2, 1);
            UIManager.Instance.OpenView("Dialog/DialogBarView", UI_Layer.Mid, (DialogView p) => { p.order = "0-3"; p.callBack = GetVisualAble; });
            void GetVisualAble()
            {
                UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Mid, (i) => { i.tipsText = "按J键可以切换空间维度！"; });
            }
        }

        public void CharInit()
        {

            EventCenter.Instance.AddEventListener(EventDefine.CHANGE_CHAR_TRIGGER, ChangeCharTrigger);
            EventCenter.Instance.AddEventListener(EventDefine.CHANGE_VISUAL_TRIGGER, ChangeVisualTrigger);
            //EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_CHAR_TRIGGER, this);
            GameManager.Instance.windMotor.InitEvent();
            GameManager.Instance.windState.InitState();
            CurrentPlayer = GameManager.Instance.WindPlayer;
            SetComponentActive(Charactors.windplayer);
            EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_CHAR);
        }

        private void SetComponentActive(Charactors m_char)
        {
            switch (m_char)
            {
                case Charactors.robotPlayer:
                    CurrentPlayer = GameManager.Instance.RobotPlayer;
                    ToRobot();
                    break;
                case Charactors.windplayer:
                    CurrentPlayer = GameManager.Instance.WindPlayer;
                    ToWind();
                    break;
                default:
                    break;
            }
        }

        private void ToRobot()
        {
            if (GameManager.Instance.windState.GetCurrentVisual() == VisualState.perspective)
                EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_VISUAL_STATE);
            GameManager.Instance.WindPlayer.SetActive(false);
            GameManager.Instance.robotMotor.isEnable = true;
            GameManager.Instance.windMotor.isEnable = false;
            GameManager.Instance.robotMotor.m_IsKinematic = true;
            GameManager.Instance.robotAudioListener.enabled = true;
            GameManager.Instance.windAudioListener.enabled = false;
            EventCenter.Instance.DispatchEvent(EventDefine.ROBOT_OPEN, true);
        }

        private void ToWind()
        {
            GameManager.Instance.WindPlayer.SetActive(true);
            GameManager.Instance.WindPlayer.transform.position = GameManager.Instance.RobotPlayer.transform.position + new Vector3(0, 2f, 0);
            GameManager.Instance.robotMotor.isEnable = false;
            GameManager.Instance.windMotor.isEnable = true;
            GameManager.Instance.robotMotor.m_IsKinematic = false;
            GameManager.Instance.robotAudioListener.enabled = false;
            GameManager.Instance.windAudioListener.enabled = true;
            Physics.SyncTransforms();
            EventCenter.Instance.DispatchEvent(EventDefine.ROBOT_OPEN, false);

        }

        public Charactors GetCurrentChar()
        {
            return m_charactor;
        }

        public void ChangeChar()
        {
            if (m_charactor == Charactors.windplayer && GameManager.Instance.Distance > 2) return;
            m_charactor = m_charactor == 0 ? Charactors.windplayer : Charactors.robotPlayer;
            SetComponentActive(m_charactor);
            EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_CHAR);
            
        }

        public void ChangeChar(Charactors charactors)
        {
            if (m_charactor == charactors) return;
            SetComponentActive(charactors);
            EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_CHAR);
        }
    }
}
