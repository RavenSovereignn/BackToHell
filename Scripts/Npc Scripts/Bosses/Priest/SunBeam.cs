using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBeam : MonoBehaviour
{
    public GameObject beam;


    void Start()
    {
        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect()
    {
        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Renderer>().enabled = false;
        beam.SetActive(true);


        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    } 


}
