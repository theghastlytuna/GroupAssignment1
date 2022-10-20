using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Cinemachine;

public class TpMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Range(3.0f, 20.0f)] private float jumpForce = 10.0f;
    [SerializeField] [Range(5.0f, 30.0f)] private float rotSpeed = 10.0f;
	[SerializeField] [Range(1.0f, 1000.0f)] private float maxSpeed = 100.0f;
    [SerializeField] [Range(0.01f, 1f)] private float dragVariable = 1.0f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Transform feetTransform;

    [Header("Rotation")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObj;

    [Header("Camera")]
	[SerializeField] private Camera playerCam;

    Vector2 moveInput;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rBody;

    PhotonView view;

    bool isGrounded;

    RaycastHit rayHit;

    bool editing = false;

    UserInput inputAction;

    // Start is called before the first frame update
    void Start()
    {
        inputAction = InputController.controller.inputAction;

        inputAction.Player.Move.performed += cntxt => moveInput = cntxt.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += cntxt => moveInput = Vector2.zero;

        inputAction.Player.Jump.performed += cntxt => OnJump();

        inputAction.Player.EnableUI.performed += cntxt => OnEnableUI();

        rBody = GetComponent<Rigidbody>();
       
        //Freeze the rotation of the rigid body, ensuring it doesn't fall over
        rBody.freezeRotation = true;

        //Photon component attached to player
        view = GetComponent<PhotonView>();

        GameObject.Find("EditorCanvas").GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //if you control THAT character
        if (view.IsMine)        {
            horizontalInput = moveInput.x;
            verticalInput = moveInput.y;

            isGrounded = Physics.CheckSphere(feetTransform.position, 0.1f, floorMask);

            RotatePlayer();
            MovePlayer();
            AddHorizontalDrag();
        }
        /*
        else
        {
            GetComponent<TpMovement>().enabled = false;
        }
        */
    }

    //For rotating the player object when the player inputs a direction
	private void RotatePlayer()
	{
		//Rotate orientation
		Vector3 viewDir = transform.position - new Vector3(playerCam.transform.position.x, transform.position.y, playerCam.transform.position.z);
		orientation.forward = viewDir.normalized;

        //Determine the input direction based on the current orientation of the player model
		Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

		//If the player has input a movement, spherically lerp between the current forward and the new direction
		if (inputDir != Vector3.zero) playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotSpeed);
	}

    //For moving the player object when the player inputs a direction
    private void MovePlayer()
    {
       
        //Calculate direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rBody.AddForce(moveDir.normalized * maxSpeed, ForceMode.Force);

        //This code is for keeping the player on a ramp while they are doing down it.
        //Raycast downwards to see what the player is standing on.
        if (Physics.Raycast(feetTransform.position, -transform.up, out rayHit, 0.1f))
        {
            //Create a quaternion that holds the rotation from up to along the ramp
            Quaternion groundRot = Quaternion.FromToRotation(Vector3.up, rayHit.normal);

            //Create a new velocity by multiplying the roation quaternion with the current velocity
            Vector3 newVelocity = groundRot * rBody.velocity;

            //If the y component of the velocity is less than 0, meaning the player is going down a ramp,
            //then set the velocity to the new velocity
            if (newVelocity.y < 0) rBody.velocity = newVelocity;
        }
    }

    //For removing slipperiness
	private void AddHorizontalDrag()
	{
        //The lower the drag variable, the lower the drag
        float dragForce = Mathf.Pow(Mathf.Sqrt(rBody.velocity.x * rBody.velocity.x + rBody.velocity.z * rBody.velocity.z), 2) * Mathf.Pow(dragVariable, 3); 

        //Multiply the drag force by the current velocity (x and z) and make it negative to find the drag vector
        Vector3 dragVec = dragForce * -new Vector3(rBody.velocity.x, 0f, rBody.velocity.z);

        //Add the drag to the current velocity
        rBody.velocity = rBody.velocity + dragVec;
    }

    //New input system
	void OnMove(InputValue playerInput)
	{
		Vector2 playerMovement = playerInput.Get<Vector2>();

		horizontalInput = playerMovement.x;
		verticalInput = playerMovement.y;
	}

    //New input system
    void OnJump()
    {
        if (isGrounded) rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    //For setting the jump force
    public void SetJumpForce(float newForce)
    {
        jumpForce = newForce;
    }

    //For getting the jump force
    public float GetJumpForce()
    {
        return jumpForce;
    }

    public void SetMaxSpeed(float newSpeed)
    {
        maxSpeed = newSpeed;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    void OnEnableUI()
    {
        GameObject.Find("EditorCanvas").GetComponent<Canvas>().enabled = !GameObject.Find("EditorCanvas").GetComponent<Canvas>().enabled;

        editing = !editing;

        playerCam.GetComponent<CinemachineBrain>().enabled = !playerCam.GetComponent<CinemachineBrain>().enabled;

        Cursor.visible = !Cursor.visible;

        if (editing) Cursor.lockState = CursorLockMode.None;

        else Cursor.lockState = CursorLockMode.Locked;

        //spawnerUI.enabled = !spawnerUI.enabled;
    }
}
