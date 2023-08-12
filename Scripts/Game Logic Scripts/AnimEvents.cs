using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvents : MonoBehaviour
{
    public static UnityAction swipeAttack;

    public void ClawAttackStart()
    {
        swipeAttack.Invoke();
    }

    
}
