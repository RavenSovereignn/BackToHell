using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pig : MonoBehaviour, IEdible, IThrowable
{
    private NavMeshAgent _agent;

    public int healAmount;

    private bool thrown = false;
    private bool grabed = false;
    private Vector3 thrownStartPos;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (thrown && Vector3.Distance(thrownStartPos, transform.position) > 30)
        {
            gameObject.GetComponent<IDestroyable>().DestroyObj();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the demon has thrown the human
        if (thrown)
        {
            //if it collides with something that can be broken
            if (collision.gameObject.TryGetComponent(out IDestroyable destroyableObj))
            {
                destroyableObj.DestroyObj();
                gameObject.GetComponent<IDestroyable>().DestroyObj();
            }
        }

    }

    public int Eat()
    {
        return healAmount;
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

        gameObject.GetComponent<AudioComponent>().PlayRandomSound(null, transform.position,1);
    }
}
