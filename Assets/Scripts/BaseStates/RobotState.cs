using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swarming.Controller
{
    public class RobotState : BaseStates
    {
        private void Start()
        {
            MoveSpeed = 15;
            JumpPower = 7;
        }

    }
}
