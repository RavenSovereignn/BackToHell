using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadeout : MonoBehaviour
{
   
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private GameObject redFlash;
    public float Duration = 0.4f;
    private void Awake()
    {
        myUIGroup.alpha = 0;
    }

    private void Update()
    {
        if (myUIGroup.alpha >= 0)
        {
            myUIGroup.alpha += Time.deltaTime;
            if (myUIGroup.alpha >= 1)
            {
                myUIGroup.alpha = 0;
                redFlash.SetActive(false);
            }
        }
    }
  
}
