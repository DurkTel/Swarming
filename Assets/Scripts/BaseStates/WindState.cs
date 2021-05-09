using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swarming.Controller
{
    public enum VisualState
    {
        perspective = 0,
        orthographic = 1
    }
    public class WindState : BaseStates
    {
        [SerializeField]
        private VisualState m_visual;
        private void Start()
        {
            
        }

        public void InitState()
        { 
            EventCenter.Instance.AddEventListener(EventDefine.CHANGE_VISUAL_STATE, ChangeVisualState);
            m_visual = VisualState.orthographic;
            MoveSpeed = 15;
            JumpPower = 7;
        }

        public void ChangeVisualState()
        {
            m_visual = m_visual == VisualState.orthographic ? VisualState.perspective : VisualState.orthographic;
        }

        public override VisualState GetCurrentVisual()
        {
            return m_visual;
        }
    }
}
