using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAi : MonoBehaviour, IDestroyable
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    private Vector3 HitboxSize;
    public Transform attackHitbox;
    public LayerMask attackHitLayer;
    public int Damage;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Transform bloodSplat;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        attackHitbox.gameObject.SetActive(false);
        HitboxSize = attackHitbox.localScale / 2.5f;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }


        //Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (Vector3.Distance(transform.position, walkPoint) <= 2)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

       // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        Vector3 rotNew = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(rotNew);

        if (!alreadyAttacked)
        {
            ///Attack code here
            StartCoroutine(PrimaryAttack());
            Debug.Log("I Attack");
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    IEnumerator PrimaryAttack()
    {
        //shows flash of red where it box is
        attackHitbox.gameObject.SetActive(true);

        //finds all objects in the hitbox
        Collider[] collisions = Physics.OverlapBox(attackHitbox.position, HitboxSize, Quaternion.identity, attackHitLayer);
        foreach (Collider c in collisions)
        {

            if (c.CompareTag("Player"))
            {
                c.gameObject.GetComponent<PlayerHealth>().TakeDmg(Damage);
                Debug.Log("I attacked player");
            }
        }

        yield return new WaitForSeconds(1);
        attackHitbox.gameObject.SetActive(false);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public void DestroyObj()
    {
        Destroy(gameObject);
        Vector3 position = new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z);
        Instantiate(bloodSplat, position, Quaternion.identity);
    }
}
