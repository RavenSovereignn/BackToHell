using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingForMovement : MonoBehaviour
{
    private Animator mAnimator;
    private Vector3 LastFramePos;
    private Vector3 LiveFramePos;
    private Vector3 difference;
    public Transform player;
    public GameObject demonModel;
    void Start()
    {
        mAnimator = demonModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        LastFramePos = player.position;
        difference = LastFramePos - LiveFramePos;
        if(difference.magnitude > 0)
        {
            mAnimator.SetBool("isMoving", true);
        }
        else 
        { 
            mAnimator.SetBool("isMoving", false); 
        }
    }
    private void LateUpdate()
    {
        LiveFramePos = player.position;
    }
}
