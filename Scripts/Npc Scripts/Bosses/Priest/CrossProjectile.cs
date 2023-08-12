using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossProjectile : MonoBehaviour
{
    public float speed;
    public int damage;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnParticleCollision(GameObject other)
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth pHealth))
        {
            //dont hit player when godmode is active. bit of a hack
            if(collision.gameObject.layer == 8)
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDmg(damage);
            }
        }
        Destroy(gameObject);
    }

}
