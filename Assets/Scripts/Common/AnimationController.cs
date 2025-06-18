using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static Action OnAttackAnimationEnd;

    public void AttackEnd()
    {
        OnAttackAnimationEnd?.Invoke();
    }
}
