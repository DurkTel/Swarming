using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using Swarming.Controller;
using UnityEngine.Events;

namespace Swarming
{
    public class TimelineManager : SingletonBase<TimelineManager>
    {

        public void StartCutscenes(string cutName,UnityAction callback) 
        {
            Dictionary<string, PlayableBinding> bindingDict = new Dictionary<string, PlayableBinding>();
            CharactorManager.Instance.SetAllCharMotor(false);
            ResManager.Instance.LoadAsync<GameObject>("Cutscenes/" + cutName, (p) =>
            {
                PlayableDirector pd = p.GetComponent<PlayableDirector>();
                foreach (var bind in pd.playableAsset.outputs)
                {
                    if (!bindingDict.ContainsKey(bind.streamName))
                    {
                        bindingDict.Add(bind.streamName, bind);
                    }
                }
                if (bindingDict.TryGetValue("CutscenesCam", out PlayableBinding pb))
                    pd.SetGenericBinding(pb.sourceObject, Camera.main.GetComponent<CinemachineBrain>());
                Camera.main.cullingMask = ~((1 << 12) + (1 << 15));
                pd.stopped += (parm) =>
                {
                    Camera.main.cullingMask = ~(1 << 14);
                    CharactorManager.Instance.ReSetCharMotor();
                    GameObject.Destroy(p);
                    callback();

                };
            });
        }
    }
}
