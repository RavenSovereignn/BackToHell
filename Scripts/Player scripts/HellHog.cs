using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellHog : MonoBehaviour
{
    public Transform vCamFollow;
    public Transform vCamLook;

    public Transform hogBody;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    public float mouseSensitivity;
    public float moveSpeed;
    private Rigidbody hogRB;

    private void Start()
    {
        hogRB = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        moveInput = vCamFollow.transform.forward * Input.GetAxisRaw("Vertical") + vCamFollow.transform.right * Input.GetAxisRaw("Horizontal");
        moveVelocity = moveInput.normalized * moveSpeed;
        //LookDirection(moveInput);
        hogRB.velocity = moveVelocity;

        hogBody.rotation = vCamFollow.rotation;

    }

    private void Update()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        if (Time.timeScale > 0)  //check if not paused
        {
            vCamFollow.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDestroyable>(out IDestroyable destroyableObj))
        {
            destroyableObj.DestroyObj();
        }
        if (other.gameObject.TryGetComponent<IBoss>(out IBoss bossInterface))
        {
            bossInterface.TakeDamage(5);
        }
    }

    public void AbilityTimer(float abilityTime, GameObject player)
    {
        StartCoroutine(abilityTimer(abilityTime, player));
    }

    private IEnumerator abilityTimer(float abilityTime, GameObject player)
    {
        yield return new WaitForSeconds(abilityTime);

        player.SetActive(true);
        player.GetComponent<HellHogRider>().EndAbility();
    }

}
