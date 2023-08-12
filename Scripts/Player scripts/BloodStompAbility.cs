using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodStompAbility : MonoBehaviour
{
    public Transform bloodSpat;

    public float chargeUpTime;
    public float activeTime;
    private float prevAcitvateTime;

    public KeyCode activateKey;
    public Text chargeUpPercentUI;

    public LayerMask hitLayers;

    private bool bloodstompActive;

    void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown(activateKey) && Time.time - prevAcitvateTime >= chargeUpTime)
        {
            StartCoroutine(Activate());
            prevAcitvateTime = Time.time;
        }

        
    }

    private void UpdateUI()
    {
        float percent = 100 * (Time.time - prevAcitvateTime) / chargeUpTime;
        percent = Mathf.Clamp(percent, 0, 100);

        if(percent >= 100){
            chargeUpPercentUI.text = "BLOODSTOMP: " + percent.ToString("F0");   //emphasize when full 
        }
        else{
            chargeUpPercentUI.text = "BloodStomp: " + percent.ToString("F0");
        }
    }

    IEnumerator Activate()
    {
        bloodstompActive = true;

        yield return new WaitForSeconds(activeTime);

        bloodstompActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bloodstompActive)
        {
            //checks if the collider's layer is within the layer mask
            if(hitLayers.value == (hitLayers.value | 1 << collision.gameObject.layer))
            {
                if(collision.gameObject.TryGetComponent(out IDestroyable script))
                {
                    script.DestroyObj();
                }
            }
        }
    }



}
