using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TpMovement : MonoBehaviour
{
    [Header("Movement")]
    //[SerializeField] [Range(1.0f, 10.0f)] private float speed = 5.0f;
    [SerializeField] [Range(3.0f, 20.0f)] private float jumpForce = 10.0f;
    //[SerializeField] [Range(0.0f, 10.0f)] private float groundDrag = 5.0f;
    //[SerializeField] [Range(0.1f, 1.0f)] private float airSlowdown = 0.3f;
    [SerializeField] [Range(5.0f, 30.0f)] private float rotSpeed = 10.0f;
	[SerializeField] [Range(1.0f, 100.0f)] private float maxSpeed = 10.0f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Transform feetTransform;

    [Header("Rotation")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObj;
	[SerializeField] private Camera playerCam;

    float horizontalInput;
    float verticalInput;

    //bool isJumping;

    Vector3 moveDir;

    Rigidbody rBody;

    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
       
        //Freeze the rotation of the rigid body, ensuring it doesn't fall over
        rBody.freezeRotation = true;

        //player = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
		//MyInput();

		/*
        //Rotate orientation
        Vector3 viewDir = transform.position - new Vector3(playerCam.transform.position.x, transform.position.y, playerCam.transform.position.z);
        orientation.forward = viewDir.normalized;

        //Rotate playerObj
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //If the player has input a movement, spherically lerp between the current forward and the new direction
        if (inputDir != Vector3.zero) playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotSpeed);
		*/
		RotatePlayer();
		
	}

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(feetTransform.position, 0.1f, floorMask);

        MovePlayer();

		AddHorizontalDrag();
    }

    private void MyInput()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");

        //Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //If the feet object is touching the ground, then jump
            rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
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
		//Debug.Log(rBody.velocity * -0.5f);
		float dragX = 0.05f + (rBody.velocity.x * rBody.velocity.x - maxSpeed) / 10 * 1;
		float dragZ = 0.05f + (rBody.velocity.z * rBody.velocity.z - maxSpeed) / 10 * 1;

		rBody.velocity = new Vector3(rBody.velocity.x * (1 - Time.deltaTime * dragX), rBody.velocity.y, rBody.velocity.z * (1 - Time.deltaTime * dragZ));

		//rBody.AddForce(-rBody.velocity, ForceMode.Force);
	}

	/*
	void OnLook(InputValue playerInput)
	{
		
		Vector2 mouseMovement = playerInput.Get<Vector2>();

		//Rotate orientation
		Vector3 viewDir = transform.position - new Vector3(playerCam.transform.position.x, transform.position.y, playerCam.transform.position.z);
		orientation.forward = viewDir.normalized;

		Vector3 inputDir = orientation.forward * mouseMovement.y + orientation.right * mouseMovement.x;

		//If the player has input a movement, spherically lerp between the current forward and the new direction
		if (inputDir != Vector3.zero) playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotSpeed);
		
	}
	*/

	void OnMove(InputValue playerInput)
	{
		Vector2 playerMovement = playerInput.Get<Vector2>();

		horizontalInput = playerMovement.x;
		verticalInput = playerMovement.y;

	}
}
