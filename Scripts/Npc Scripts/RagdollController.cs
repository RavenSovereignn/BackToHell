using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class RagdollController : MonoBehaviour, IThrowable
{
    public GameObject RagDoll;
    public GameObject VillagerModel;

    public NavMeshAgent _agent;

    private bool thrown = false;
    private bool grabed = false;
    private Vector3 thrownStartPos;

    //only collide with one house
    private bool collided = false;

    //called when the ragdoll colides
    public void RagdollCollision(Collider collision)
    {
        //if the demon has thrown the human
        if (thrown && !collided)
        {
            //dont collide with self
            if(collision.GetComponent<RagdollController>() == this)
            {
                return;
            }

            //if it collides with something that can be broken
            if (collision.gameObject.TryGetComponent(out IDestroyable destroyableObj))
            {
                destroyableObj.DestroyObj();
                //gameObject.GetComponent<IDestroyable>().DestroyObj();

                if(collision.gameObject.TryGetComponent<HellSpreadObj>(out HellSpreadObj obj))
                {
                    if(obj.destructableObjectType == GameLogic.DestructableObjects.House)
                    {
                        collided = true;
                    }
                }
            }
        }
    }


    public void Grabed()
    {
        Destroy(_agent);
        _agent = null;
        grabed = true;
    }

    public void Thrown()
    {
        thrown = true;
        thrownStartPos = transform.position;
        Destroy(VillagerModel);
        if(RagDoll != null)RagDoll.SetActive(true);

        gameObject.GetComponent<AudioComponent>().PlayRandomSound(transform, transform.position,1);

        Destroy(gameObject, 10);
    }
}
