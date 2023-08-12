using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour

{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    [SerializeField] 
    private GameObject YouDiedScreen;
    [SerializeField]
    private GameObject DmgFlash;
    public Fadeout fadeout;
    public bool youdiedbool = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {  
        if( currentHealth <= 0)
        {
            Time.timeScale = 0;
            YouDied();
            Debug.Log("You Died");
        }
    }
    
    public void TakeDmg(int amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        DmgFlash.SetActive(true);
        Debug.Log("I took dmg");
    }
    public void HealDmg(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        Debug.Log("I healed dmg");
    }
    public void YouDied ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        youdiedbool = true;
        DmgFlash.SetActive(false);
        YouDiedScreen.SetActive(true);

    }
  
}
