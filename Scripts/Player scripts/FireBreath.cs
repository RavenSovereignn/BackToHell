using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBreath : MonoBehaviour
{
    ParticleSystem ps;
    public float cooldownTime;
    private float nextAttackTime = 0;
    private bool isCooldown = false;
    public Image FireIcon;

    private KeyCode fireBreathKey;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        fireBreathKey = Controls.GetInstance().FireBreath;
    }

    void Update()
    {
        if(Input.GetKeyDown(fireBreathKey) && Time.time > nextAttackTime)
        {
            Fire();
            nextAttackTime = Time.time + cooldownTime;
            Debug.Log("Firebreath cooldown started");
        }
        fireBreathCooldown();
    }
    void fireBreathCooldown()
    {
        if (Input.GetKey(fireBreathKey) && isCooldown == false)
        {
            isCooldown = true;
            FireIcon.fillAmount = 1f;
        }

        if (isCooldown)
        {
            FireIcon.fillAmount -= 1 / cooldownTime * Time.deltaTime;

            if (FireIcon.fillAmount <= 0)
            {
                FireIcon.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
    void Fire()
    {
        Debug.Log("I Fire");
        ps.Play();
        gameObject.GetComponent<AudioComponent>().PlayRandomSound(transform,transform.position,0.1f);

    }
}
