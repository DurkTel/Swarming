using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swarming.Controller
{
    public class InputManager : SingletonBase<InputManager>
    {
        private float input_h;
        public float Input_h
        {
            get { return input_h; }
        }

        private float input_v;
        public float Input_v
        {
            get { return input_v; }
        }

        private bool isOnGround;
        public bool IsOnGround
        {
            get { return isOnGround; }
        }

        public bool JumpTrigger = false;
        private bool lastJumpTrigger;

        public bool ChangeTrigger;
        private bool lastChangeTrigger;

        public bool CharactorChangerTrigger;
        private bool LastCharactorChangerTrigger;

        public bool InteractTrigger;
        private bool lastInteractTrigger;


        private float rayOffset = 0;

        //方向偏移量
        private float degree;
        public float Degree
        {
            get { return degree; }
        }
        //输入方向
        private Vector2 iputVec;
        public Vector2 IPutVec
        {
            get { return iputVec; }
        }


        private GameObject m_currentChar;
        private CapsuleCollider capcol;

        public InputManager()
        {
            MonoManager.Instance.AddUpdateListener(Update, "InputManager");
            MonoManager.Instance.AddFixedUpdateListener(InteractTriggerSignal);
        }

        private void Update()
        {
            //获取水平输入
            input_h = Input.GetAxis("Horizontal");
            //获取垂直输入
            input_v = Input.GetAxis("Vertical");
            JumpSignal();
            CalculateVec();
            ChangeSignal();
            ChangeCharactorSignal();
            //InteractTriggerSignal();
        }

        //跳跃信号单次获取
        void JumpSignal()
        {
            bool newjump = Input.GetAxisRaw("Jump") == 1;
            if (newjump != lastJumpTrigger && newjump == true)
            { 
                JumpTrigger = true;
                EventCenter.Instance.DispatchEvent(EventDefine.JUMP_KEY);
            }
            else
                JumpTrigger = false;
            lastJumpTrigger = newjump;
        }

        void ChangeSignal()
        {
            bool newchange = Input.GetKeyDown(KeyCode.J);
            if (newchange != lastChangeTrigger && newchange == true)
                ChangeTrigger = true;
            else
                ChangeTrigger = false;
            lastChangeTrigger = newchange;
        }

        void ChangeCharactorSignal()
        {
            bool newchange = Input.GetKeyDown(KeyCode.K);
            if (newchange != LastCharactorChangerTrigger && newchange == true)
                CharactorChangerTrigger = true;
            else
                CharactorChangerTrigger = false;
            LastCharactorChangerTrigger = newchange;
        }

        void InteractTriggerSignal()
        {
            bool newchange = Input.GetKeyDown(KeyCode.L);
            if (newchange != lastInteractTrigger && newchange == true)
            { 
                InteractTrigger = true;
                EventCenter.Instance.DispatchEvent(EventDefine.INTERACTION_KEY);
            }
            else
                InteractTrigger = false;
            lastInteractTrigger = newchange;
        }

        //计算移动方向
        void CalculateVec()
        {
            iputVec = SquareToCircle(new Vector2(Input_h, Input_v));
            degree = Mathf.Sqrt(Mathf.Pow(iputVec.x, 2) + Mathf.Pow(iputVec.y, 2));
            //iputVec = tempAxis.x * Vector3.right + tempAxis.y * Vector3.forward;
        }

        //获得圆形输入向量
        Vector2 SquareToCircle(Vector2 input)
        {
            Vector2 output = Vector2.zero;
            output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
            output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
            return output;
        }

        //地面输入（接触地面）
        void CalculateOnGround()
        {
            //特殊处理 不检测来自侧面的检测 把检测放在脚底
            //float details = capcol.height / 2 - capcol.radius;
            //point_1 = transform.position + transform.up * details;
            //point_2 = transform.position - transform.up * details;
            //Vector3 point = transform.position - transform.up*0.5f;
            //Collider[] outPutCols = Physics.OverlapCapsule(point_1, point_2, capcol.radius, LayerMask.GetMask("Ground"));
            //Collider[] outPutCols = Physics.OverlapSphere(point, 0.4f, LayerMask.GetMask("Ground"));
            //isOnGround = outPutCols.Length > 0;
            //foreach (var item in outPutCols)
            //{
            //  print(item.name);
            //}

            m_currentChar = CharactorManager.Instance.CurrentPlayer;
            capcol = m_currentChar.GetComponent<CapsuleCollider>();
            rayOffset = CharactorManager.Instance.GetCurrentChar() == CharactorManager.Charactors.robotPlayer ? 0.5f : 0.1f;
            isOnGround = Physics.Raycast(m_currentChar.transform.position, m_currentChar.transform.position - (Vector3.up * (capcol.height / 2 + rayOffset)), LayerMask.GetMask("Ground"));
            Debug.DrawLine(m_currentChar.transform.position, m_currentChar.transform.position - (Vector3.up * (capcol.height / 2 + rayOffset)), Color.red);
            Debug.Log(CharactorManager.Charactors.robotPlayer + isOnGround.ToString());
        }

    }
}
