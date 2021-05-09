using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : SingletonBase<AnimationManager>
{
    public void PlayAniByTrgger(string aniName, Animator animator)
    {
        animator.SetTrigger(aniName);
    }

    public void PlayAniByBool(string aniName, Animator animator, bool trigger)
    {
        animator.SetBool(aniName, trigger);
    }

    public void PlayAniByFloat(string aniName, Animator animator, float value)
    {
        animator.SetFloat(aniName, value);
    }

    public float GetAniFloat(string aniName, Animator animator)
    {
        return animator.GetFloat(aniName);

    }

    public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layIndex, Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(layIndex);
    }

}
