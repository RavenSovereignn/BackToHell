using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public GameObject attackHitbox;
    public LayerMask attackHitLayer;
    private Vector3 HitboxSize;

    public Transform bloodSpat;
    public Transform player;
    private PlayerHealth PlayerHealth;

    [Header("Attack Stats")]
    public float attackSpeed = 0.1f;  //time between attacks
    private float prevAttackTime = 0;
    public int healthGainedOnkill = 15;

    [Header("Effects and Anim")]
    public Swipe attackEffect;
    private KeyCode attackKey;
    public Animator mAnimator;
    public GameObject demonModel;
    
    void Start()
    {
        mAnimator = demonModel.GetComponent<Animator>();

        HitboxSize = attackHitbox.GetComponent<BoxCollider>().bounds.size /2.0f;
        attackHitbox.gameObject.SetActive(false);

        PlayerHealth = player.GetComponent<PlayerHealth>();
        attackKey = Controls.GetInstance().BasicAttack;
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && Time.time - prevAttackTime >= attackSpeed )
        {
            StartCoroutine(PrimaryAttack());
            prevAttackTime = Time.time;
            mAnimator.SetTrigger("attackTrigger");
           
        }
    }

    IEnumerator PrimaryAttack()
    {
        yield return new WaitForSeconds(0.2f);


        attackEffect.StartVFX();
        gameObject.GetComponent<AudioComponent>().PlaySound(0, null, transform.position, 1);

        //small delay for timing with vfx
        yield return new WaitForSeconds(0.15f);

        //finds all objects in the hitbox
        Collider[] collisions = Physics.OverlapBox(attackHitbox.transform.position, HitboxSize,Quaternion.identity,attackHitLayer);
        foreach(Collider c in collisions)
        {
            if(c.gameObject.TryGetComponent<IDestroyable>(out IDestroyable destroyableObj))
            {
                destroyableObj.DestroyObj();
            }
            if (c.gameObject.TryGetComponent<IBoss>(out IBoss bossInterface))
            {
                bossInterface.TakeDamage(1);
            }
            if (c.CompareTag("Enemy"))
            {
                Destroy(c.gameObject);
                Vector3 position = new Vector3(c.gameObject.transform.position.x, 0.1f, gameObject.transform.position.z);
                Instantiate(bloodSpat, position, Quaternion.identity);
                PlayerHealth.HealDmg(healthGainedOnkill);
            }
            if (c.CompareTag("Boss"))
            {
                c.GetComponent<PigBossHealth>().TakeDamage(50);
                Debug.Log("I deal dmg to boss");
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

}
