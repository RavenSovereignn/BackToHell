using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    public GameObject swipeTrail;
    public List<Transform> startPoints;

    private List<GameObject> particleSystems;

    [Range(0f, 360)]
    public float swipeLength = 90;
    public float swipeTime = 0.5f;

    //how long inbetween refreshes in the coroutine
    private float delayLength = 0.01f;   

    void Start()
    {
        particleSystems = new List<GameObject>();
        foreach(Transform t in startPoints)
        {
            GameObject particleSys = Instantiate(swipeTrail, t.position, Quaternion.identity);
            particleSystems.Add(particleSys);
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
        }
    }

    public void StartVFX()
    {
        StartCoroutine(ActivateSwipe());
    }

    private IEnumerator ActivateSwipe()
    {
        for(int i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].GetComponent<ParticleSystem>().Play();
            particleSystems[i].GetComponent<Transform>().position = startPoints[i].position;
        }

        WaitForSeconds delay = new WaitForSeconds(0.01f);

        float swipeAmount = (swipeLength / swipeTime) * delayLength;

        float timepassed = 0f;
        while(timepassed < swipeTime)
        {
            foreach (GameObject ps in particleSystems)
            {
                ps.transform.RotateAround(transform.position, Vector3.down, swipeAmount);
            }
            timepassed += delayLength;
            yield return delay;
        }


        int count = 0;
        foreach (GameObject ps in particleSystems)
        {
            ps.GetComponent<ParticleSystem>().Pause();
            ps.transform.position = startPoints[count++].position;
        }

        foreach (GameObject ps in particleSystems)
        {
            ps.GetComponent<ParticleSystem>().Play();
        }

    }

}
