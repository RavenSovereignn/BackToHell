using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PigMinionAi : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDmg(25);
            Debug.Log("I minion dmged player");
        Destroy(this.gameObject);
    }
}
