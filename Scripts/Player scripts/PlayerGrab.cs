using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("References")]
    public Transform grabHitbox;
    public Transform grabDestination;
    private Vector3 hitboxSize;
    public Transform demonBody;
    private PlayerHealth health;

    [Header("Grab Settings")]
    public LayerMask grabLayer;
    private GameObject heldObj;
    private IThrowable throwObj;
    private bool holdingSomething = false;

    //controls
    private KeyCode grabKey;
    private KeyCode throwKey;

    void Start()
    {
        hitboxSize = grabHitbox.localScale / 2;
        health = gameObject.GetComponent<PlayerHealth>();

        grabKey = Controls.GetInstance().Grab;
        throwKey = Controls.GetInstance().Throw;
    }

    void Update()
    {
        if (Input.GetKeyDown(grabKey) && !holdingSomething)
        {
            StartCoroutine(Grab());
        }
        else if (Input.GetKeyDown(throwKey) && holdingSomething)
        {
            StartCoroutine(Throw());
        }
    }

    private IEnumerator Grab()
    {
        float closest = -1;
        Collider closestObj = null;

        Collider[] collisions = Physics.OverlapBox(grabHitbox.position, hitboxSize, Quaternion.identity, grabLayer);
        foreach (Collider c in collisions)
        {
           if(closest < 0 ||  Vector3.Distance(transform.position, c.transform.position) < closest )
           {
                if (c.gameObject.TryGetComponent<IThrowable>(out throwObj))
                {
                    closestObj = c;
                }
           }
        }
        if(closestObj != null)
        {
            closestObj.GetComponent<Collider>().enabled = false;
            closestObj.transform.position = grabDestination.position;
            closestObj.transform.parent = demonBody;

            holdingSomething = true;
            heldObj = closestObj.gameObject;
            throwObj.Grabed();

            //if its a pig
            if(heldObj.TryGetComponent<IEdible>(out IEdible pig))
            {
                //call the eat function on the pig and heal the player
                health.HealDmg(pig.Eat()); 
            }
        }



        yield return null;
    }

    private IEnumerator Throw()
    {
        heldObj.GetComponent<Collider>().enabled = true;
        heldObj.transform.parent = null;
        heldObj.layer = 13;

        if (!heldObj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb = heldObj.AddComponent<Rigidbody>();
        }

        if(throwObj != null) { 
            throwObj.Thrown();
            throwObj = null;
        }

        if(heldObj.TryGetComponent<RagdollController>(out RagdollController _)){

            Rigidbody[] rbs = heldObj.GetComponentsInChildren<Rigidbody>();

            Vector3 pos = grabDestination.position - grabDestination.forward + new Vector3(0, 1, 0);
            foreach(Rigidbody r in rbs)
            {
                r.AddExplosionForce(5000, pos, 5.0f);
            }
        }
        else
        {
            rb.AddForce(grabDestination.forward * 2000);
        }

        holdingSomething = false;


        yield return null;
    }


}
