using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming.Controller;
using UnityEngine.Events;

namespace Swarming
{
    public class LevelFunc_2 : MonoBehaviour
    {
        public GameObject board;
        public GameObject sokoban;
        public GameObject RollRobot;
        public GameObject draw;
        public GameObject block;
        public GameObject cage;
        public GameObject[] sokobanHider;
        public bool[] Pillar = { true, true, true, false, false };
        public GameObject[] PillarFire;
        public GameObject Letter;
        public GameObject thrustCam;
        public Transform Pillars;
        public Transform FinalSwitch;

        void Start()
        {
            InitEvent();
        }


        void InitEvent()
        {
            EventCenter.Instance.AddEventListener(EventDefine.FLOAT_BOARD, BoardMove);
            EventCenter.Instance.AddEventListener(EventDefine.BOARD_STOP, BoardStop);
            EventCenter.Instance.AddEventListener(EventDefine.ROTPOWER, Rotblock);
            EventCenter.Instance.AddEventListener(EventDefine.ROLL_1, Roll_1);
            EventCenter.Instance.AddEventListener(EventDefine.ROLL_2, Roll_2);
            EventCenter.Instance.AddEventListener(EventDefine.ROLL_STOP_1, RollStop_1);
            EventCenter.Instance.AddEventListener(EventDefine.ROLL_STOP_2, RollStop_2);
            EventCenter.Instance.AddEventListener(EventDefine.CONDUIT, Conduit);
            EventCenter.Instance.AddEventListener(EventDefine.CONDUITEX, ConduitEx);
            EventCenter.Instance.AddEventListener(EventDefine.SOKOBAN, Sokoban);
            EventCenter.Instance.AddEventListener(EventDefine.SOKOBANEX, SokobanEx);
            EventCenter.Instance.AddEventListener(EventDefine.DRAWWORDENTER, DrawWordEnter);
            EventCenter.Instance.AddEventListener(EventDefine.DRAWWORDEND, DrawWordEnd);
            EventCenter.Instance.AddEventListener(EventDefine.PILLARPUZZLE, PillarPuzzle);
            EventCenter.Instance.AddEventListener(EventDefine.STARTPILLARPUZZLE, StartPillar);
            EventCenter.Instance.AddEventListener(EventDefine.FINALSWITCHUP, FinalSwitchUp);
            EventCenter.Instance.AddEventListener(EventDefine.LETTER, LetterUp);
            EventCenter.Instance.AddEventListener(EventDefine.ENTER_SOKOBAN, EnterSokoban);
            EventCenter.Instance.AddEventListener(EventDefine.GAME_END, GameOver);
        }

        void RemoveEvenet()
        {

        }

        void GameOver(object obj)
        {
            UIManager.Instance.OpenView<SuccessView>("Success/Success_View", UI_Layer.System);
        }

        void BoardMove(object obj)
        {
            Destroy(board.GetComponent<Mechanism>());
            board.GetComponent<FloatBoard>().enabled = true;
        }

        void BoardStop(object obj)
        {
            Destroy(board.GetComponent<FloatBoard>());
            MusicManager.Instance.PlaySound("fallWater", false);
        }

        void Rotblock(object obj)
        {
            GameObject gameObject = obj as GameObject;
            gameObject.GetComponent<Animator>().enabled = true;
            for (int i = 0; i < block.transform.childCount; i++)
            {
                Animator temp = block.transform.GetChild(i).GetComponent<Animator>();
                temp.enabled = true;
                StartCoroutine(AniCallBack(temp, () => { temp.transform.GetChild(0).gameObject.SetActive(true); }));

            }
        }

        void Roll_1(object obj)
        {

            GameManager.Instance.robotMotor.isEnable = false;
            //GameManager.Instance.RobotPlayer.SetActive(false);
            GameManager.Instance.RobotPlayer.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            RollRobot.SetActive(true);
            CameraManager.Instance.ChangeChar(RollRobot.transform);
        }

        void Roll_2(object obj)
        {
            GameManager.Instance.RobotPlayer.SetActive(false);
            RollRobot.SetActive(true);
            RollRobot.transform.position = new Vector3(0, -33.2f, 373.2f);
            CameraManager.Instance.ChangeChar(RollRobot.transform);
        }


        void RollStop_1(object obj)
        {
            GameObject temp = obj as GameObject;
            Animator robot = temp.GetComponent<Animator>();
            robot.SetBool("End", false);
            StartCoroutine(AniCallBack(robot, () =>
             {
                 GameManager.Instance.RobotPlayer.transform.position = new Vector3(0, -37f, 352.18f);
                 robot.gameObject.SetActive(false);
                 GameManager.Instance.RobotPlayer.SetActive(true);
                 EventCenter.Instance.DispatchEvent(EventDefine.ROBOT_OPEN, true);
                 Physics.SyncTransforms();
                 CameraManager.Instance.ChangeChar();
             }));
        }

        void RollStop_2(object obj)
        {
            GameObject temp = obj as GameObject;
            Animator robot = temp.GetComponent<Animator>();
            GameManager.Instance.RobotPlayer.transform.position = new Vector3(0, -126.8f, 795.6f);

            robot.SetBool("End", false);
            StartCoroutine(AniCallBack(robot, () =>
            {
                robot.gameObject.SetActive(false);
                //GameManager.Instance.RobotPlayer.SetActive(true);
                GameManager.Instance.RobotPlayer.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                EventCenter.Instance.DispatchEvent(EventDefine.ROBOT_OPEN, true);

                Physics.SyncTransforms();
                CameraManager.Instance.ChangeChar();
            }));
        }

        void Conduit(object obj)
        {
            GameObject temp = obj as GameObject;
            Transform pos = temp.transform.GetChild(0);
            GameManager.Instance.RobotPlayer.transform.position = pos.position;
            GameManager.Instance.robotMotor.PhyVect = Vector3.zero;
            EventCenter.Instance.DispatchEvent(EventDefine.ROLL, false);
            Physics.SyncTransforms();
        }

        void ConduitEx(object obj)
        {
            GameObject temp = obj as GameObject;
            Transform pos = temp.transform.GetChild(0);
            GameManager.Instance.WindPlayer.transform.position = pos.position;
            Physics.SyncTransforms();
        }

        void EnterSokoban(object obj)
        {
            bool temp = !thrustCam.activeSelf;
            thrustCam.SetActive(temp);
            CharactorManager.Instance.canChangeChar = !temp;
            if (temp)
                UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Top, (p) => p.tipsText = "此关卡暂时失去切换角色能力，请利用风核心的能力来驱动机器人进入管道哦！");

        }

        void Sokoban(object obj)
        {
            GameObject game = obj as GameObject;
            int index = 0;
            switch ((obj as GameObject).name)
            {
                case "ThrustForWard":
                    GameManager.Instance.robotMotor.PhyVect = Vector3.forward * 5f;
                    index = 0;
                    break;
                case "ThrustBack":
                    GameManager.Instance.robotMotor.PhyVect = Vector3.back * 5f;
                    index = 1;
                    break;
                case "ThrustLeft":
                    GameManager.Instance.robotMotor.PhyVect = Vector3.left * 5f;
                    index = 2;
                    break;
                case "ThrustRight":
                    GameManager.Instance.robotMotor.PhyVect = Vector3.right * 5f;
                    index = 3;
                    break;
                default:
                    break;
            }
            EventCenter.Instance.DispatchEvent(EventDefine.ROLL, true);
            game.GetComponentInChildren<Animator>().enabled = true;
            foreach (var item in sokoban.GetComponentsInChildren<SokobanTips>())
            {
                item.PlayTipsAni(index);
            }
            StartCoroutine(DelyFunc(2.5f, () => game.GetComponentInChildren<Animator>().enabled = false));
        }

        IEnumerator DelyFunc(float time, UnityAction callBack)
        {
            yield return new WaitForSeconds(time);
            callBack();
        }

        void StartPillar(object obj)
        {

            TimelineManager.Instance.StartCutscenes("机器人被囚禁", () =>
            {
                GameObject player = (GameObject)obj;
                player.transform.position = new Vector3(0, -127.2f, 1068f);
                Physics.SyncTransforms();
                CharactorManager.Instance.ChangeChar();
                cage.SetActive(true);
                MusicManager.Instance.PlaySound("stoveFall", false);
                StartCoroutine(MoveByValue
                    (Pillars, new Vector3(Pillars.transform.position.x, -128.25f, Pillars.transform.position.z), 3f, (p) =>
                    {
                        CharactorManager.Instance.canChangeChar = false;
                        UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Top, (i) => i.tipsText = "有陷阱！出口就在前方，加油破解谜题吧！");

                    }));
            });
        }

        void FinalSwitchUp(object obj)
        {
            StartCoroutine(MoveByValue(FinalSwitch, new Vector3(FinalSwitch.position.x, -126.889f, FinalSwitch.position.z), 3f, (p) => { }));
        }

        void LetterUp(object obj)
        {
            Letter.SetActive(true);
        }

        void PillarPuzzle(object obj)
        {
            string name = (obj as GameObject).name;
            int index = (int.Parse(name.Substring(7, 1)) - 1);
            if (Pillar[index]) return;
            int next = index + 1 >= Pillar.Length ? 0 : index + 1;
            int front = index - 1 <= -1 ? Pillar.Length - 1 : index - 1;
            Pillar[index] = !Pillar[index];
            Pillar[next] = !Pillar[next];
            Pillar[front] = !Pillar[front];
            PillarFire[index].SetActive(Pillar[index]);
            PillarFire[next].SetActive(Pillar[next]);
            PillarFire[front].SetActive(Pillar[front]);
            if (CheckPillar())
                GameEnd();
        }



        bool CheckPillar()
        {
            foreach (var item in Pillar)
            {
                if (!item) return false;
            }
            return true;
        }

        void SokobanEx(object obj)
        {
            GameObject game = obj as GameObject;
            Animator animator = game.GetComponentInChildren<Animator>();
            animator.enabled = !animator.enabled;
            string name = (obj as GameObject).name;
            int index = int.Parse(name.Substring(10, 1));
            Vector3 sokoban = sokobanHider[index - 1].transform.localPosition;
            Vector3 pos = sokoban.y < -0.5f ? new Vector3(sokoban.x, 0, sokoban.z) : new Vector3(sokoban.x, -2.0f, sokoban.z);
            StartCoroutine(MoveByValue(sokobanHider[index - 1].transform, pos, 2f, (p) => { }));
        }



        void DrawWordEnter(object obj)
        {
            StartCoroutine(ScaleByValue(GameManager.Instance.WindPlayer.transform, 0.5f, 5f, (p) =>
             {
                 p.position = new Vector3(0, -120f, 970);
                 Physics.SyncTransforms();
                 EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_VISUAL_STATE);
                 draw.SetActive(true);
             }));
        }

        void DrawWordEnd(object obj)
        {
            StartCoroutine(ScaleByValue(GameManager.Instance.WindPlayer.transform, 1f, 5f, (p) =>
            {
                draw.SetActive(false);
            }));
        }

        void GameEnd()
        {
            StartCoroutine(MoveByValue(cage.transform, new Vector3(cage.transform.position.x, -120f, cage.transform.position.z), 3.5f, (p) =>{ CharactorManager.Instance.canChangeChar = true; }));
        }

        IEnumerator AniCallBack(Animator ani, UnityAction callBack)
        {
            yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);
            callBack();
        }

        IEnumerator ScaleByValue(Transform tran, float scale, float time, UnityAction<Transform> callback)
        {
            float currentTime = 0;
            while (tran.transform.localScale != scale * Vector3.one)
            {
                yield return new WaitForFixedUpdate();
                currentTime += Time.deltaTime / time;
                Vector3 currentScale = tran.transform.localScale;
                tran.transform.localScale = Vector3.Lerp(currentScale, scale * Vector3.one, currentTime);
            }
            callback(tran);
        }

        IEnumerator MoveByValue(Transform tran, Vector3 pos, float time, UnityAction<Transform> callback)
        {
            float currentTime = 0;
            Vector3 currentPos = tran.localPosition;
            while (Vector3.Distance(currentPos, pos) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                tran.localPosition = Vector3.Lerp(tran.localPosition, pos, currentTime);
                currentPos = tran.localPosition;
                currentTime += Time.deltaTime / time;
            }
            callback(tran);
        }
    }
}