using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBossHealth : MonoBehaviour
{
    public int maxHealth = 1000;
    public int currentHealth;
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }
     void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(200);
        }
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Pig boss took dmg:" + amount);
    }
}
