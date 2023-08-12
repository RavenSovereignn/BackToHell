using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HellHogRider : MonoBehaviour
{
    public float hogRidingTime;

    public GameObject hellHogPrefab;
    public Transform HogSpawnPoint;
    public CinemachineVirtualCamera vCam;
    public Transform playerCamFollow;

    //usual Y position for the player
    private float startYPos;
    
    private GameObject hog;

    void Start()
    {
        startYPos = transform.position.y;   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartAbility();
        }
    }


    private void StartAbility()
    {
        hog = Instantiate(hellHogPrefab, HogSpawnPoint.position, HogSpawnPoint.rotation);

        HellHog hogScript = hog.GetComponent<HellHog>();

        vCam.Follow = hogScript.vCamFollow;
        vCam.LookAt = hogScript.vCamFollow;

        hogScript.AbilityTimer(hogRidingTime, gameObject);

        gameObject.SetActive(false);
    }

    public void EndAbility()
    {
        gameObject. SetActive(true);
        gameObject.transform.position = hog.transform.position;
        vCam.Follow = playerCamFollow;
        vCam.LookAt = playerCamFollow;

        Destroy(hog);
    }



}
