using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.GetComponent<PlayerHealth>().TakeDmg(10);
        }

    }

    public void Rise(float speed, float finalHeight, float deleteTime)
    {
        StartCoroutine(Rising(speed, finalHeight, deleteTime));
    }

    IEnumerator Rising(float speed, float finalHeight, float deleteTime)
    {
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        while(transform.position.y <= finalHeight)
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));

            yield return delay;
        }

        yield return new WaitForSeconds(deleteTime);
        Destroy(gameObject);
    }

}
