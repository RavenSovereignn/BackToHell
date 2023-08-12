using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PigBossAi : MonoBehaviour
{
    public string bossName;
    public float attackRadius;
    public NavMeshAgent agent;

    public Transform player;
    public GameLogic gameLogicScript;

    public LayerMask whatIsGround, whatIsPlayer;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public LayerMask groundLayer;

    public HealthBar bossHealthBar;
    public int currentBossHealth;
    //Abilites
    private Vector3 ramPosition;
    private bool ramming = false;
    private bool spawning = false;
    public float ramRange;
    public float spawnRange;
    public float meleeRange;
    public GameObject StompVFX;

    public Transform holystompHitbox;
    public LayerMask holystompHitLayer;
    private Vector3 holystompHitboxSize;
    public float maxforce = 1000f;
    public Rigidbody playerRb;

    private Vector3 HitboxSize;
    public Transform attackHitbox;
    public LayerMask attackHitLayer;

    public GameObject minionPrefab;
    //States
    public float sightRange, attackRange;
    private bool playerInRamRange;
    private bool playerInSpawnRange;
    private bool playerInMeleeRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool colliderdmg = false;
    private object collision;
    public PlayerHealth playerHealth;

    private void Awake()
    {
        currentBossHealth = GetComponent<PigBossHealth>().currentHealth;
        playerHealth = GetComponent<PlayerHealth>();
        //bossHealthBar.SetMaxHealth((int)currentBossHealth);
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        attackHitbox.gameObject.SetActive(false);
        HitboxSize = attackHitbox.localScale / 2.5f;
    }

    private void OnDestroy()
    {
        //when killed send info to game logic
        gameLogicScript.slainBoss(BossType.Pig);
    }

    private void Update()
    {
        currentBossHealth = GetComponent<PigBossHealth>().currentHealth;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInRamRange = Physics.CheckSphere(transform.position, ramRange, whatIsPlayer);
        playerInSpawnRange = Physics.CheckSphere(transform.position, spawnRange, whatIsPlayer);
        playerInMeleeRange = Physics.CheckSphere(transform.position, meleeRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (ramming == false && playerInSightRange && !playerInAttackRange && spawning == false)
        {
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            
            AttackPlayer();
        }

        Debug.Log(currentBossHealth);
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
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
            StartCoroutine(AttackPattern());
            Debug.Log("I Attack");
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
   
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    IEnumerator AttackPattern()
    {

       
        if (currentBossHealth>= 800 && playerInMeleeRange)
        {
            StartCoroutine(NormalAttack());
            
        }
        else if(currentBossHealth <= 800 && currentBossHealth >= 600 && playerInRamRange && playerInSightRange)
        {
            StartCoroutine(RamAttack());
            yield return new WaitForSeconds(3f);

        }
        else if(currentBossHealth <= 600 && currentBossHealth >= 400)
        {
            agent.speed = 3.5f;
            agent.acceleration = 8;
            ramming = false;
            StartCoroutine(holySmash());
        }
        else if (currentBossHealth <= 400 && currentBossHealth >= 0)
        {
            ramming = false;
            StartCoroutine(spawnPigMinions());
            yield return new WaitForSeconds(3f);
        }
        yield return null;
    }
    IEnumerator NormalAttack()
    {
        Debug.Log("I use normal attack");
        //shows flash of red where it box is
        attackHitbox.gameObject.SetActive(true);

        //finds all objects in the hitbox
        Collider[] collisions = Physics.OverlapBox(attackHitbox.position, HitboxSize, Quaternion.identity, attackHitLayer);
        foreach (Collider c in collisions)
        {

            if (c.CompareTag("Player"))
            {
                c.gameObject.GetComponent<PlayerHealth>().TakeDmg(25);
                Debug.Log("I attacked player");
            }
        }

        yield return new WaitForSeconds(1);
        attackHitbox.gameObject.SetActive(false);
    }
    IEnumerator RamAttack()
    {
        Debug.Log("I ram");
        ramming = true;
        while (ramming == true)
        {
            ramPosition = player.position;
            agent.SetDestination(ramPosition);
            agent.speed = 40;
            agent.acceleration = 40;
            colliderdmg = true;
            if(Vector3.Distance(transform.position, ramPosition) <= 1)
            {
               ramming = false;
            }
            yield return new WaitForSeconds(3);
        }
        if (ramming == false)
        {
            agent.speed = 3.5f;
            agent.acceleration = 8;
            colliderdmg = false;
        }
        yield return new WaitForSeconds(5f);
    }
    IEnumerator holySmash()
    {
        holystompHitbox.gameObject.SetActive(true);
        StartCoroutine(HolyStompVfx());
        Collider[] collisions = Physics.OverlapSphere(holystompHitbox.position, holystompHitbox.GetComponent<CapsuleCollider>().bounds.size.x, attackHitLayer);
        foreach (Collider c in collisions)
        {
            Debug.Log($"foreach {c.gameObject.name}");
            if (c.CompareTag("Player"))
            {
                
                c.gameObject.GetComponent<PlayerHealth>().TakeDmg(25);
                 playerRb.AddExplosionForce(maxforce, transform.position, 120f);
                Debug.Log("PlayerRb");
            }
        }
        holystompHitbox.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator HolyStompVfx()
    {
        Vector3 spawnPos = transform.position + new Vector3(1, 0, 1);
        spawnPos.y = 0;
        Transform effect = (Instantiate(StompVFX, spawnPos, Quaternion.identity)).transform;

        Vector3 scale = new Vector3();

        for (int i = 0; i < 10; i++)
        {
            effect.localScale = scale;
            scale += new Vector3(2.5f, 2.5f, 2.5f) / 10f;
            yield return new WaitForSeconds(0.4f / 10f);
        }

        yield return new WaitForSeconds(5f);

        for (int i = 0; i < 10; i++)
        {
            effect.localScale = scale;
            scale -= new Vector3(2.5f, 2.5f, 2.5f) / 10f;
            yield return new WaitForSeconds(0.4f / 10f);
        }

        Destroy(effect.gameObject);

        yield return null;

    }
    private IEnumerator spawnPigMinions()
    {
        Debug.Log("I spawn");
        int numMinions = 3;
        spawning = true;
        while (spawning == true)
        {
            
            for (int i = 0; i < numMinions; i++)
            {
                Vector3 spawn = transform.position + new Vector3(Random.Range(-5f, 5f), 0 , Random.Range(-5f, 5f));
                GameObject g = Instantiate(minionPrefab, spawn, Quaternion.identity);
                g.GetComponent<PigMinionAi>().player = player;
                if(i == numMinions)
                {
                    spawning = false;
                }
            }
            yield return new WaitForSeconds(4f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (colliderdmg == true)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDmg(25);
            Debug.Log("I rammed on player");
        }
    }
}