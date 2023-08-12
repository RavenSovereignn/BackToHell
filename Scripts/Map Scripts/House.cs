using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, IDestroyable
{
    public bool createHellNode;
    public float maxForce = 5f;

    public bool keepParts;
    [Range(0,100)]public int percentPartsStay;
    public float shrinkAmount;
    private float handlePartDelay = 0.35f;

    public float timeToDestroy;
    public bool shakeCamera = false;
    public float shakeIntensity;
    public float shakeDuration;


    public void BreakHouse()
    {
        Rigidbody[] bricks = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody objPart in bricks)
        {
            objPart.isKinematic = false;

            objPart.AddExplosionForce(maxForce, transform.position, 20f);
            objPart.AddForce(Random.insideUnitSphere * maxForce);

            //sets the shader to glow red when passing through the ground
            if (objPart.GetComponent<Renderer>().material.HasProperty("Boolean_35be091dcba1460092dee0af010a6863")){
                objPart.GetComponent<Renderer>().material.SetInt("Boolean_35be091dcba1460092dee0af010a6863", 1);
            }
        }

        if (shakeCamera) { CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeDuration); }    

        if (keepParts) { StartCoroutine(HandleParts(bricks)); }

        Destroy(gameObject.GetComponent<Collider>());
        Destroy(gameObject, timeToDestroy);
    }

    Vector3 RandomForce()
    {
        Vector3 rndForce = Random.onUnitSphere * maxForce;
        return rndForce;
    }

    private IEnumerator HandleParts(Rigidbody[] _parts)
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);
        List<Rigidbody> parts = new List<Rigidbody>(_parts);

        yield return new WaitForSeconds(handlePartDelay);

        //destory a few random parts, can alter the amount 
        for (int i = 0; i < parts.Count; i++)
        {
            Rigidbody part = parts[i];
            if (Random.Range(0.0f, 1.0f) > percentPartsStay / 100.0)
            {
                parts.Remove(part);
                Destroy(part.gameObject);
            }
            yield return delay;
        }

        //shrink the remaining parts in increments
        float shrinkSteps = ((1 -shrinkAmount) * 10) * 4;
        for (int i= 0; i < shrinkSteps; i++)
        {
            foreach (Rigidbody part in parts)
            {
                part.transform.localScale = part.transform.localScale * 0.95f;
            }
            yield return delay;
        }

        yield return new WaitForSeconds(0.4f);

        //destroy the rest of the parts
        int count = parts.Count;
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, parts.Count);
            Rigidbody part = parts[index];
            parts.Remove(part);
            Destroy(part.gameObject);
            yield return delay;
        }

        yield return null;
    }

    public void DestroyObj()
    {
        if (gameObject.TryGetComponent<AudioComponent>(out AudioComponent audio))
        {
            //play random audio from gameobject at this position but no parent
            audio.PlayRandomSound(null, transform.position,1);
        }
        BreakHouse();  
    }

}

