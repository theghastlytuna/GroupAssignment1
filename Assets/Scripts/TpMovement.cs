using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class TpMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Range(3.0f, 20.0f)] private float jumpForce = 10.0f;
    [SerializeField] [Range(5.0f, 30.0f)] private float rotSpeed = 10.0f;
	[SerializeField] [Range(1.0f, 100.0f)] private float maxSpeed = 10.0f;
    [SerializeField] [Range(0.01f, 1f)] private float dragVariable = 1.0f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Transform feetTransform;

    [Header("Rotation")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObj;
	[SerializeField] private Camera playerCam;

    float horizontalInput;
    float verticalInput;

    bool isJumping;

    Vector3 moveDir;

    Rigidbody rBody;

    PhotonView view;

    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
       
        //Freeze the rotation of the rigid body, ensuring it doesn't fall over
        rBody.freezeRotation = true;
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is your character you can control that one only
        //if (view.IsMine)
        {
            //MyInput();
        }
    }

    private void FixedUpdate()
    {
        /*
        if (view.IsMine)        {
            //isGrounded = Physics.CheckSphere(feetTransform.position, 0.1f, floorMask);            //MovePlayer();            //if (isGrounded) rBody.drag = groundDrag;            //else rBody.drag = 0;
        }
        */
        isGrounded = Physics.CheckSphere(feetTransform.position, 0.1f, floorMask);

        RotatePlayer();
        MovePlayer();
        AddHorizontalDrag();
    }

	private void RotatePlayer()
	{
		//Rotate orientation
		Vector3 viewDir = transform.position - new Vector3(playerCam.transform.position.x, transform.position.y, playerCam.transform.position.z);
		orientation.forward = viewDir.normalized;

		Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

		//If the player has input a movement, spherically lerp between the current forward and the new direction
		if (inputDir != Vector3.zero) playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotSpeed);
	}


    private void MovePlayer()
    {
        //Calculate direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //If grounded, normal movement
        //if (isGrounded) 
		rBody.AddForce(moveDir.normalized * maxSpeed * 10f, ForceMode.Force);

    }

	private void AddHorizontalDrag()
	{
        //The lower the drag variable, the lower the drag
        float dragForce = Mathf.Pow(Mathf.Sqrt(rBody.velocity.x * rBody.velocity.x + rBody.velocity.z * rBody.velocity.z), 2) * Mathf.Pow(dragVariable, 3); 

        Vector3 dragVec = dragForce * -new Vector3(rBody.velocity.x, 0f, rBody.velocity.z);

        rBody.velocity = rBody.velocity + dragVec;
    }

	void OnMove(InputValue playerInput)
	{
		Vector2 playerMovement = playerInput.Get<Vector2>();

		horizontalInput = playerMovement.x;
		verticalInput = playerMovement.y;

	}

    void OnJump()
    {
        if (isGrounded) rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void SetJumpForce(float newForce)
    {
        jumpForce = newForce;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }
}
