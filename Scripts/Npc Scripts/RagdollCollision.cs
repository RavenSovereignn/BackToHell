using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCollision : MonoBehaviour
{
    RagdollController controller;

    void Start()
    {
        controller = gameObject.GetComponentInParent<RagdollController>();       
    }

    private void OnTriggerEnter(Collider other)
    {
        controller.RagdollCollision(other);
    }

}
