using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class GrabPig : MonoBehaviour
{
    public Transform player;
    public Transform TheDest;
    bool hasPlayer = false;
    bool beingCarried = false;
    private Flee pigFlee;
    private BoxCollider pigCollider;
    private PlayerHealth PlayerHealth;
   
     void Start()
    {
        pigFlee = GetComponent<Flee>();
        pigCollider = GetComponent<BoxCollider>();
        PlayerHealth = player.GetComponent<PlayerHealth>();
    }
    void Update()
    {
        float dist = Vector3.Distance(gameObject.transform.position, player.position);
        if (dist <= 2.5f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }
        if (hasPlayer && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("I grab");
            transform.position = TheDest.position;
            pigFlee.enabled = false;
            pigCollider.enabled = false;
            /*GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().mass = 0;
            */
            GetComponent<NavMeshAgent>().enabled = false;
            transform.parent = TheDest;
            StartCoroutine(DestroyPig());
            beingCarried = true;
        }

        if (beingCarried)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //GetComponent<Rigidbody>().isKinematic = false;
                //transform.parent = null;
                //beingCarried = false;
            }
        }
    }
    IEnumerator DestroyPig()
    {
        
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        PlayerHealth.HealDmg(15);
    }
}