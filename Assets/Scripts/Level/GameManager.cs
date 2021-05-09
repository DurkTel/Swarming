using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming.Controller;

namespace Swarming
{
    public enum Level
    {
        level_1,
        level_2,
        level_3
    }
    public class GameManager : SingletonBase<GameManager>
    {
        public GameObject RobotPlayer;
        public GameObject WindPlayer;
        public RobotMotor robotMotor;
        public WindMotor windMotor;
        public RobotState robotState;
        public WindState windState;
        public AudioListener robotAudioListener;
        public AudioListener windAudioListener;
        public int GoldNum;
        private GameObject Hinder_2D;
        private GameObject Hinder_3D;
        private Level m_level = Level.level_1;
        private float m_distance;
        private Vector3 brithPos;
        private int m_life = 3;

        public int Life { set { m_life = value; } get { return m_life; } }

        public float Distance { get { return m_distance; } }

        public Level CurrentLevel { get { return m_level; } }

        public GameManager()
        {
            MonoManager.Instance.AddUpdateListener(Update, "GameManager");
        }
        //protected override void Awake()
        //{
        //    base.Awake();
        //    Initialize();
        //    DontDestroyOnLoad(this);
        //}
        //private void Start()
        //{
        //    EventCenter.Instance.AddEventListener(EventDefine.CHANGE_VISUAL_STATE, SetHinder);
        //}

        public void Initialize()
        {
            var root = GameObject.Find("SceneColl");
            //var hinders = GameObject.FindGameObjectsWithTag("Hinder");
            Hinder_2D = root.transform.Find("Hinder_2D").gameObject;
            Hinder_3D = root.transform.Find("Hinder_3D").gameObject;
            Hinder_2D.SetActive(true);
            Hinder_3D.SetActive(false);//拿到后把它激死
            //进行第一次获取
            if (RobotPlayer == null) RobotPlayer = ResManager.Instance.Load<GameObject>("RobotPlayer");
            if (WindPlayer == null) WindPlayer = ResManager.Instance.Load<GameObject>("WindPlayer");
            if (robotMotor == null) robotMotor = RobotPlayer.GetComponent<RobotMotor>();
            if (windMotor == null) windMotor = WindPlayer.GetComponent<WindMotor>();
            if (robotState == null) robotState = RobotPlayer.GetComponent<RobotState>();
            if (windState == null) windState = WindPlayer.GetComponent<WindState>();
            if (robotAudioListener == null) robotAudioListener = RobotPlayer.GetComponentInChildren<AudioListener>();
            if (windAudioListener == null) windAudioListener = WindPlayer.GetComponentInChildren<AudioListener>();
            //可重新获取出生点
            brithPos = GameObject.FindGameObjectWithTag("Birth").transform.position;
            RobotPlayer.transform.position = brithPos;
            WindPlayer.transform.position = brithPos;
            Physics.SyncTransforms();
            GameObject.DontDestroyOnLoad(RobotPlayer);
            GameObject.DontDestroyOnLoad(WindPlayer);
            //初始化摄像机和角色相关
            RobotPlayer.GetComponentInChildren<BaseAni>().InitEvent();

            CameraManager.Instance.Initialize();

            CharactorManager.Instance.CharInit();
            //ChatManager.Instance.Initialize();
            SaveManager.Instance.AddEvent();
            EventCenter.Instance.AddEventListener(EventDefine.CHANGE_VISUAL_STATE, SetHinder);
            //WindPlayer.GetComponent<BaseAni>().InitEvent();
        }

        public void GetHurt()
        {
            m_life--;
            EventCenter.Instance.DispatchEvent(EventDefine.LIFEUPDATE);
            if (m_life <= 0)
            {
                UIManager.Instance.OpenView<FailView>("Fail/Fail_View", UI_Layer.System);
            }
        }
        public void Update()
        {
            m_distance = Vector3.Distance(RobotPlayer.transform.position, WindPlayer.transform.position);
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    CharactorManager.Instance.canChangeChar = true;
            //    CharactorManager.Instance.canChangeVisual = true;
            //}
            if (Input.GetKeyDown(KeyCode.Escape) && !UIManager.Instance.IsShowing("MenuEx/MenuEx_View") && !UIManager.Instance.IsShowing("Menu/MainMenu_View"))
            {
                UIManager.Instance.OpenView<MenuExView>("MenuEx/MenuEx_View", UI_Layer.System);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                UIManager.Instance.CloseView("Loading/Loading_View");
            }
        }

        public void ChangeLevel(int num)
        {
            m_level = (Level)num;
        }

        private void SetHinder()
        {
            bool act = Hinder_2D.activeSelf;
            Hinder_2D.SetActive(!act);
            Hinder_3D.SetActive(act);
        }

    }
}
