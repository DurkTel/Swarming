using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Swarming
{
    public class LevelFunc_1 : SingletonMono<LevelFunc_1>
    {
        private List<GameObject> plats;
        public GameObject fallRock;
        public GameObject fujiman;
        public GameObject fujiman2;
        public GameObject wall;
        public GameObject tengman;
        public GameObject floatBoard;
        public GameObject brokenBridge;
        public GameObject rockDoor;
        public GameObject rockDoorEffect;
        public GameObject windTurbine;
        public GameObject brokenBridge2;
        public GameObject floatBoard2;
        public GameObject hays;
        public GameObject train;
        public GameObject platForm;
        public GameObject pumice;
        public GameObject platFormCam;
        void Start()
        {
            InitEvent();
        }

        void InitEvent()
        {
            plats = new List<GameObject>();
            EventCenter.Instance.AddEventListener(EventDefine.ENTER_BROKEN_BRIDGE, EnterBrokenBridge);
            EventCenter.Instance.AddEventListener(EventDefine.FALL_ROCK_TRIGGER, FallingRocks);
            EventCenter.Instance.AddEventListener(EventDefine.PRESSURE_PLATE_TRIGGER, RockDoorOpen);
            EventCenter.Instance.AddEventListener(EventDefine.PULL_ROD_TRIGGER, PullRod);
            EventCenter.Instance.AddEventListener(EventDefine.WIND_POWER_TRIGGER, WindPower);
            EventCenter.Instance.AddEventListener(EventDefine.FIRE_TRIGGER_1, FireTrigger_1);
            EventCenter.Instance.AddEventListener(EventDefine.TRAIN_START, TrainStart);
            EventCenter.Instance.AddEventListener(EventDefine.PLATFORM, PlatFormTrigger);
            EventCenter.Instance.AddEventListener(EventDefine.PLATFORMERROR, PlatFormTriggerError);
            EventCenter.Instance.AddEventListener(EventDefine.OPENPUMICE, OpenPucime);
            EventCenter.Instance.AddEventListener(EventDefine.PUMICETRAP, PucimeTrap);
            EventCenter.Instance.AddEventListener(EventDefine.ENTER_PLATFORM, EnterOrExitPlat);
            EventCenter.Instance.AddEventListener(EventDefine.DIALOG, OpenDialog);
            EventCenter.Instance.AddEventListener(EventDefine.FIRE_TIPS, FireTips);
        }

        void RemoveEvenet()
        {
            EventCenter.Instance.RemoveEventListener(EventDefine.ENTER_BROKEN_BRIDGE, EnterBrokenBridge);
            EventCenter.Instance.RemoveEventListener(EventDefine.FALL_ROCK_TRIGGER, FallingRocks);
            EventCenter.Instance.RemoveEventListener(EventDefine.PRESSURE_PLATE_TRIGGER, RockDoorOpen);
            EventCenter.Instance.RemoveEventListener(EventDefine.PULL_ROD_TRIGGER, PullRod);
            EventCenter.Instance.RemoveEventListener(EventDefine.WIND_POWER_TRIGGER, WindPower);
            EventCenter.Instance.RemoveEventListener(EventDefine.FIRE_TRIGGER_1, FireTrigger_1);
            EventCenter.Instance.RemoveEventListener(EventDefine.TRAIN_START, TrainStart);
            EventCenter.Instance.RemoveEventListener(EventDefine.PLATFORM, PlatFormTrigger);
            EventCenter.Instance.RemoveEventListener(EventDefine.PLATFORMERROR, PlatFormTriggerError);
            EventCenter.Instance.RemoveEventListener(EventDefine.OPENPUMICE, OpenPucime);
            EventCenter.Instance.RemoveEventListener(EventDefine.PUMICETRAP, PucimeTrap);
            EventCenter.Instance.RemoveEventListener(EventDefine.ENTER_PLATFORM, EnterOrExitPlat);
            EventCenter.Instance.RemoveEventListener(EventDefine.DIALOG, FireTips);


        }

        void OpenDialog(object obj)
        {
            GameObject game = obj as GameObject;
            UIManager.Instance.OpenView<DialogView>("Dialog/DialogBarView", UI_Layer.Mid, (DialogView p) => { p.order = game.name; });
        }

        void EnterBrokenBridge(object obj)
        {
            wall.SetActive(false);
            brokenBridge.GetComponent<Animator>().enabled = true;
            GameObject player = (GameObject)obj;
            player.transform.position = new Vector3(0, -11f, 26.4f);
            Physics.SyncTransforms();
            TimelineManager.Instance.StartCutscenes("碎桥动画", () => {
                brokenBridge.GetComponent<BoxCollider>().enabled = false;
                
            });
            
            EventCenter.Instance.AddEventListener(EventDefine.CHANGE_VISUAL_STATE, () => { wall.SetActive(!wall.activeSelf); });
        }

        void FallingRocks(object obj)
        {
            floatBoard.GetComponent<Animator>().enabled = true;
            UIManager.Instance.OpenView<DialogView>("Dialog/DialogBarView", UI_Layer.Mid, (p) => { p.order = "0-4"; });
            AudioSource frieSource = MusicManager.Instance.PlaySound("fire", false, 0.8f);
            StartCoroutine(FujimanFire(0, fujiman,()=> {
                MusicManager.Instance.StopSound(frieSource);
                fallRock.GetComponentInChildren<Animator>().enabled = true;
                tengman.SetActive(false);
                MusicManager.Instance.PlaySound("stoveFall", false, 0.5f);
            }));
        }

        void FireTips(object obj)
        { 
            UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Top, (p) => p.tipsText = "试试对着火把使用互动键！有意想不到的效果哦！");
        }
        
        IEnumerator FujimanFire(int index,GameObject game,UnityAction callBack = null)
        {
            int count = game.transform.childCount;
            if (index > count - 1)
            {
                if (callBack != null)
                    callBack();
                yield break;
            } 
            Animator tempAni = game.transform.GetChild(index).GetComponent<Animator>();
            tempAni.enabled = true;
            yield return new WaitForSeconds(tempAni.GetCurrentAnimatorStateInfo(0).length);
            StartCoroutine(FujimanFire(++index, game, callBack));
        }

        IEnumerator AniCallBack(Animator ani, UnityAction callBack)
        {
            yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);
            callBack();
        }

        void RockDoorOpen(object obj)
        {
            Animator rockDoorAni = rockDoor.GetComponent<Animator>();
            rockDoorAni.enabled = true;
            rockDoorEffect.SetActive(true);
            StartCoroutine(AniCallBack(rockDoorAni, () => { rockDoorEffect.SetActive(false); }));
            MusicManager.Instance.PlaySound("stoveDoorOpen", false);
            EventCenter.Instance.RemoveEventListener(EventDefine.CHANGE_VISUAL_STATE, () => { wall.SetActive(!wall.activeSelf); });

        }

        void PullRod(object obj)
        {
            brokenBridge2.GetComponent<Animator>().enabled = true;
        }

        void WindPower(object obj)
        {
            floatBoard2.GetComponent<Animator>().enabled = true;
            windTurbine.GetComponent<Animator>().enabled = true;

        }

        void FireTrigger_1(object obj)
        {
            hays.SetActive(false);
            AudioSource frieSource = MusicManager.Instance.PlaySound("fire", false, 0.8f);
            StartCoroutine(FujimanFire(0, fujiman2, () =>
             {
                 UIManager.Instance.OpenView<TipsView>("Tips/Tips_View", UI_Layer.Top, (p) => p.tipsText = "障碍物已被清理，可以通行！");

                 MusicManager.Instance.StopSound(frieSource);
             }));
        }

        void TrainStart()
        {
            train.SetActive(false);
            TimelineManager.Instance.StartCutscenes("火车动画",()=> {
                GameManager.Instance.RobotPlayer.transform.position = new Vector3(52, 4, 160);
                Physics.SyncTransforms();
            });
            
        }

        void PlatFormTrigger(object obj)
        {
            GameObject plat = obj as GameObject;
            switch (plats.Count)
            {
                case 0:
                    if (plat.name == "PlatForm_1")
                        plats.Add(plat);
                    break;
                case 1:
                    if (plat.name == "PlatForm_2")
                        plats.Add(plat);
                    break;
                case 2:
                    if (plat.name == "PlatForm_3")
                    {
                        plats.Add(plat);
                        platForm.GetComponent<Animator>().enabled = true;
                    }
                    break;
                default:
                    break;
            }
        }

        void EnterOrExitPlat(object obj)
        {
            GameObject gameObject = obj as GameObject;
            bool isActive = gameObject.name == "EnterOlatForn";
            platFormCam.SetActive(isActive);
            GameManager.Instance.windMotor.specialVision = isActive;
        }

        void PlatFormTriggerError(object obj)
        {
            GameObject plat = obj as GameObject;
            plat.GetComponent<Animator>().enabled = true;
            MusicManager.Instance.PlaySound("stoveDoorOpen", false);

        }

        void OpenPucime(object obj)
        {
            pumice.SetActive(true);
            pumice.GetComponent<Animator>().enabled = true;
            GameObject p = obj as GameObject;
            p.GetComponent<Animator>().enabled = true;
        }

        public void PucimeTrap(object obj)
        {
            EventCenter.Instance.EventListenerClear();
            //RemoveEvenet();
            PoolManager.Instance.Clear();
            pumice.GetComponent<Animator>().Play("浮石桥陷阱");
            UIManager.Instance.OpenView<LoadingView>("Loading/Loading_View", UI_Layer.System);
            ScenesManager.Instance.LoadSceneAsyn("Level_2", ()=> { 
                GameManager.Instance.Initialize();
                UIManager.Instance.CloseAllView();
                UIManager.Instance.OpenView<StatusView>("Status/Status_View", UI_Layer.Bot);

                //GameManager.Instance.RobotPlayer.transform.position = new Vector3(0, 10, 7.2f);
                //Physics.SyncTransforms();
            });
        }
    }
}
