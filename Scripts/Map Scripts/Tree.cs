using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [HideInInspector]
    public bool onFire = false;

    public float burnTime;

    ParticleSystem firePS;

    private void Start()
    {
        firePS = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("tree lit");

        if (!onFire)
        {
            SetAlight();
        }
    }

    public void SetAlight()
    {
        StartCoroutine(SetFire());
    }

    private IEnumerator SetFire()
    {
        onFire = true;

        firePS.Play();

        yield return new WaitForSeconds(burnTime);

        firePS.Stop();
    }

    private void BurnCloseTrees()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        foreach(var c in colliders)
        {
            if (c.TryGetComponent<Tree>(out Tree tree))
            {
                tree.SetAlight();
            }
        }

    }

}
