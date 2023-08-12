using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody PlayerRb;
    public GameObject playerBody;
    public Transform cameraFollowPoint;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    public float mouseSensitivity;
    public float rotationSpeed = 0.1f;



    // Start is called before the first frame update
    void Start()
    {
        PlayerRb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        moveInput = cameraFollowPoint.transform.forward * Input.GetAxisRaw("Vertical") + cameraFollowPoint.transform.right * Input.GetAxisRaw("Horizontal");
        moveVelocity = moveInput.normalized * moveSpeed;
        //LookDirection(moveInput);
        PlayerRb.velocity = moveVelocity;

        playerBody.transform.rotation = cameraFollowPoint.rotation;


    }

    private void Update()
    {
        RotatePlayer();

        //godmode cheat code
        if(Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.G))
        {
            gameObject.layer = 0;
            moveSpeed = 40; 
        }
    }

    private void RotatePlayer()
    {
        if(Time.timeScale > 0)  //check if not paused
        {
            cameraFollowPoint.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
        }
    }

    private void LookUpDown()
    {
        float mouseInput = Input.GetAxis("Mouse Y") * mouseSensitivity * 0.1f;
        cameraFollowPoint.localPosition += new Vector3(0, mouseInput, 0);

        //clamp height withn range
        Vector3 prevHeight = cameraFollowPoint.localPosition;
        prevHeight.y = Mathf.Clamp(prevHeight.y,0 , 1.2f);
        cameraFollowPoint.localPosition = prevHeight;
    }

    private void LookDirection(Vector3 CurrentMovementDirection)
    {
        if (CurrentMovementDirection.magnitude <= 0.1) { return; }   //not changing direction

        float direction = (180 / Mathf.PI) * Mathf.Atan2(CurrentMovementDirection.x, CurrentMovementDirection.z);

        //smoothes the rotation
        Quaternion newDirection = Quaternion.Lerp(playerBody.transform.rotation, Quaternion.Euler(new Vector3(0, direction, 0)), rotationSpeed * Time.deltaTime);

        playerBody.transform.rotation = newDirection;
    }


}
