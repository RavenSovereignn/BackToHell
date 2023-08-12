using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKill : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("I collide");

        Destroy(this.gameObject);

    }
}
