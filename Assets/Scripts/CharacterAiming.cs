using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] [Range(10.0f, 40.0f)] private float turnSpeed = 15.0f;
    [SerializeField] [Range(0.1f, 1.0f)] private float aimDuration = 0.3f;

    [SerializeField] Transform followTarget;
    [SerializeField] Camera playerCam;
    [SerializeField] Transform playerObj;
    [SerializeField] CinemachineVirtualCamera zoomCam;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    Animator animator;
    int isAimingVariable = Animator.StringToHash("isAiming");

    bool isAiming = false;

    private void OnEnable()
    {
       // aim
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(isAimingVariable, isAiming);
        /*
        if (isAiming)
        {
            //startedAiming = false;
            followTarget.transform.forward = transform.forward;
        }
        */
        
        if (isAiming)
        {
            playerObj.transform.rotation = Quaternion.Euler(new Vector3(0f, followTarget.transform.rotation.eulerAngles.y, 0f));
        }
        
    }

    private void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        followTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
    }

	
	void OnAim()
	{
        isAiming = !isAiming;

        if (isAiming) zoomCam.Priority += 10;
        else zoomCam.Priority -= 10;
	}
}
