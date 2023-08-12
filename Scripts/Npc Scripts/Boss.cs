using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IBoss
{
    public string bossName;

    public GameLogic gamelogicScript;
    public GameObject player;
    public float attackRadius;

    public GameObject attackPrefab;
    public LayerMask groundLayer;

    public float attackSpeed;
    public float attackSpacing; 
    private float prevAttackTime;

    public float health = 5;
    public HealthBar bossHealthBar;

    private void Start()
    {
        bossHealthBar.SetMaxHealth((int)health);
    }


    void Update()
    {
        if(Vector3.Distance(player.transform.position,transform.position) <= attackRadius)
        {
            if(Time.time - prevAttackTime >= attackSpeed)
            {
                StartCoroutine( AttackPlayer());
                prevAttackTime = Time.time;
            }

            transform.LookAt(player.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y,0);
        }


    }

    private Vector2 vec2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }

    IEnumerator AttackPlayer()
    {
        Vector2 bossPos = vec2(transform.position);
        Vector2 playerPos = vec2(player.transform.position);

        float dist = Vector2.Distance(bossPos, playerPos);
        if (dist <= attackSpacing)
        {
            SpawnBalde(player.transform.position);
            yield return null;
        }

        Vector2 dir = (playerPos - bossPos).normalized * attackSpacing;
        Vector3 step = new Vector3(dir.x, 1, dir.y);
        Vector3 attackPoint = transform.position;
        while(Vector2.Distance(vec2(attackPoint), playerPos) > attackSpacing * 1.5f)
        {
            attackPoint += step;
            SpawnBalde(attackPoint);
            yield return new WaitForSeconds(0.15f);
        }
        SpawnBalde(new Vector3(playerPos.x, 1, playerPos.y));

        yield return null;
    }

    void SpawnBalde(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, new Vector3(0, -1, 0), out hit, groundLayer, 1000))
        {
            Vector3 attackPoint = hit.point;
            GameObject atkCircle = Instantiate(attackPrefab, attackPoint + new Vector3(0, -0.75f, 0), Quaternion.identity);

        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        bossHealthBar.SetHealth((int)health);
        if (health <= 0)
        {
            gamelogicScript.slainBoss(BossType.Paladin);
            Destroy(gameObject);
        }
    }
}
