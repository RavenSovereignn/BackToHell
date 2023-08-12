using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBeamHit : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth pHealth))
        {
            //dont hit player when godmode is active. bit of a hack
            if (other.gameObject.layer == 8)
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDmg(damage);
            }
        }
    }


}
