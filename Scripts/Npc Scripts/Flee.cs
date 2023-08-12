using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee : MonoBehaviour, IDestroyable
{
    private NavMeshAgent _agent;
    public GameObject Player;
    public float FleeDistance;

    public Transform bloodSplat;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        //Debug.Log("distance: " + distance);
        if(_agent != null)
        {
            if (distance < FleeDistance)
            {
                Vector3 dirtoPlayer = transform.position - Player.transform.position;
                Vector3 newPos = transform.position + dirtoPlayer;
                _agent.SetDestination(newPos);
            }
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
        Vector3 position = new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z);
        Instantiate(bloodSplat, position, Quaternion.identity);
    }

}
