using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int Damage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyArrow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {

            other.gameObject.GetComponent<PlayerHealth>().TakeDmg(Damage);
            Destroy(this.gameObject);
            Debug.Log("I hit player");
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        
        
    }
}
