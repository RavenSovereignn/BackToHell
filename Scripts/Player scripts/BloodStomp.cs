using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStomp : MonoBehaviour
{
    

    public Transform blood;
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            ContactPoint contact = col.contacts[0];
            Quaternion rotation = Quaternion.identity;
            Vector3 position = new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z);
            Instantiate(blood, position, rotation);
            Destroy(gameObject);
        }
    }
}