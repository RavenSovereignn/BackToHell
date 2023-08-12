using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float delayTime;
    public float hitboxActivetime;
    public float swordSpeed;

    public GameObject sword;

    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(delayTime);

        sword.GetComponent<Sword>().Rise(swordSpeed, 1, hitboxActivetime);
        sword.transform.parent = null;

        Destroy(gameObject);

    }

}
