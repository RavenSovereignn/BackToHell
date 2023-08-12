using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static Controls instance;
    

    [Range(0,1)]
    public float volume = 0;

    public KeyCode BasicAttack;
    public KeyCode FireBreath;
    public KeyCode Stomp;
    public KeyCode Grab;
    public KeyCode Throw;

    public static Controls GetInstance()
    {
        return instance;
    }


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


}
