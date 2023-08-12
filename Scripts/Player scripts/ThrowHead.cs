using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHead : MonoBehaviour
{
    public GameObject impHead;
    private Vector3 startingPosition;

    public LayerMask throwHitLayer;
    public Transform throwHitbox;
    private Vector3 hitboxSize;

    [Header("Throw Settings")]
    public float speed;
    private float airTime;
    public float projectileSmoothness;
    private WaitForSeconds smallDelay;


    private void Start()
    {
        startingPosition = impHead.transform.position;
        hitboxSize = throwHitbox.localScale / 2;
        airTime = 5 / speed;
        smallDelay = new WaitForSeconds(airTime / projectileSmoothness);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ThrowHeadCheck();
        }

    }

    void ThrowHeadCheck()
    {
        Collider[] collisions = Physics.OverlapBox(throwHitbox.position, hitboxSize, Quaternion.identity, throwHitLayer);

        float closest = float.PositiveInfinity;
        GameObject target = null;

        foreach (var c in collisions)
        {
            if (Vector3.Distance(transform.position, c.transform.position) <= closest)
            {
                target = c.gameObject;
            }
        }
        StartCoroutine(Throw(target));
    }

    IEnumerator Throw(GameObject target)
    {
        Vector3 startPos = impHead.transform.position;
        float totalAirTime = 0;

        while(totalAirTime <= airTime)
        {
            impHead.transform.position = Vector3.Lerp(startPos, target.transform.position, totalAirTime * (1/airTime));

            Debug.Log($"current: {totalAirTime} max: {airTime}");

            yield return smallDelay;
            totalAirTime += (airTime / projectileSmoothness);
        }

        totalAirTime = 0;
        yield return new WaitForSeconds(0.5f);

        while (totalAirTime <= airTime)
        {
            impHead.transform.position = Vector3.Lerp(target.transform.position, startPos, totalAirTime * (1 / airTime));

            Debug.Log($"current: {totalAirTime} max: {airTime}");

            yield return smallDelay;
            totalAirTime += (airTime / projectileSmoothness);
        }

        yield return null;

    }



}
