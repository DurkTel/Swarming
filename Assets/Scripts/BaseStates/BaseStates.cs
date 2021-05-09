using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swarming.Controller
{
    public class BaseStates : MonoBehaviour
    {
        private int hp;
        public int HP
        {
            get { return hp; }
        }

        private float moveSpeed;
        public float MoveSpeed
        {
            set { moveSpeed = value; }
            get { return moveSpeed; }
        }

        private float jumpPower;
        public float JumpPower
        {
            set { jumpPower = value; }
            get { return jumpPower; }
        }

        private int MaxHP = 5;

        public void GetHurt(int damge)
        {
            hp -= damge;
        }

        public void GetCure(int treat)
        {
            hp += treat;
            if (hp > 5)
                hp = 5;
        }

        public virtual void Death()
        {
            if (hp <= 0)
                print("死亡");
        }

        public virtual VisualState GetCurrentVisual() { return VisualState.orthographic; }

    }
}
