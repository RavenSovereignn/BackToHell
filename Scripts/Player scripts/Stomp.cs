using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stomp : MonoBehaviour
{
    [Header("HitBox Settings")]
    public Transform stompHitbox;
    public LayerMask stompHitLayer;
    private float stompHitBoxRadius;

    [Header("References")]
    public Transform bloodSpat;
    private PlayerHealth PlayerHealth;
    public Transform player;
    public Image stompIcon;
    public GameObject StompVFX;

    [Header("Ability Settings")]
    public float cooldownTime = 10;
    private float nextAttackTime = 0;
    private bool isCooldown = false;
    [Tooltip("(Intensity, Duration)")]public Vector2 cameraShakeSettings;
    public float vfxAppearTime; //how quickly the vfx appears

    private KeyCode stompKey;
    public Animator mAnimator;

    void Start()
    {
        stompHitbox.gameObject.SetActive(false);
        stompHitBoxRadius = stompHitbox.GetComponent<SphereCollider>().radius;
        PlayerHealth = player.GetComponent<PlayerHealth>();

        stompKey = Controls.GetInstance().Stomp;
        mAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(stompKey) && Time.time > nextAttackTime)
        {          
            nextAttackTime = Time.time + cooldownTime;
            StartCoroutine(stompAttack());
            mAnimator.SetTrigger("stompAttack");
            CinemachineShake.Instance.ShakeCamera(cameraShakeSettings.x, cameraShakeSettings.y);
            Debug.Log("Stomp cooldown started");
            
        }
        StompCooldown();
    }

    void StompCooldown()
    {
        if(Input.GetKey(stompKey) && isCooldown == false)
        {
            
            isCooldown = true;
            stompIcon.fillAmount = 1f;
        }

        if (isCooldown)
        {
            stompIcon.fillAmount -= 1 / cooldownTime * Time.deltaTime;

            if (stompIcon.fillAmount <= 0)
            {
                stompIcon.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

    IEnumerator stompAttack()
    {
        StartCoroutine(StompVfx());
        gameObject.GetComponent<AudioComponent>().PlaySound(1, null, transform.position, 1);

        Collider[] collisions = Physics.OverlapSphere(stompHitbox.position, stompHitBoxRadius, stompHitLayer);
        foreach (Collider c in collisions)
        {
            if (c.gameObject.TryGetComponent<House>(out House houseScript))
            {
                houseScript.BreakHouse();
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
                PlayerHealth.HealDmg(5);
            }
            if (c.CompareTag("Boss"))
            {
                c.GetComponent<PigBossHealth>().TakeDamage(100);
                Debug.Log("I deal dmg to boss with stomp");
            }
        }

        yield return new WaitForSeconds(0.1f);
        stompHitbox.gameObject.SetActive(false);
    }

    private IEnumerator StompVfx()
    {
        Vector3 spawnPos = stompHitbox.position + new Vector3(1,0,0);
        spawnPos.y = 0;
        Transform effect = (Instantiate(StompVFX, spawnPos, Quaternion.identity)).transform;

        Debug.Log(stompHitBoxRadius);

        Vector3 scale = new Vector3();
        Vector3 scaleStepSize = new Vector3(stompHitBoxRadius * 0.4f, stompHitBoxRadius * 0.4f, stompHitBoxRadius * 0.4f) / 10f;

        for (int i = 0; i < 10; i++)
        {
            effect.localScale = scale;
            scale += scaleStepSize;
            yield return new WaitForSeconds(vfxAppearTime / 10f);
        }

        yield return new WaitForSeconds(5f);

        for (int i = 0; i < 10; i++)
        {
            effect.localScale = scale;
            scale -= scaleStepSize;
            yield return new WaitForSeconds(vfxAppearTime / 10f);
        }

        Destroy(effect.gameObject);

        yield return null;

    }

}
