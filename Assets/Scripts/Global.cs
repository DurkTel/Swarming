using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming;

public class Global:MonoBehaviour
{

    public LevelFunc_1 obj;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            obj.PucimeTrap(this);
            //EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_VISUAL_TRIGGER, this);
            //EventCenter.Instance.DispatchEvent(EventDefine.CHANGE_CHAR_TRIGGER, this);

        }
        
    }


}
