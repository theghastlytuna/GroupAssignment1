using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] [Range(10.0f, 40.0f)] private float turnSpeed = 15.0f;
    [SerializeField] [Range(0.1f, 1.0f)] private float aimDuration = 0.3f;

    //[SerializeField] Transform followTarget;
    [SerializeField] Camera playerCam;
    [SerializeField] Transform playerObj;
    [SerializeField] CinemachineVirtualCamera zoomCam;

    bool isAiming = false;

    //public PlayerAction playerInput;

    private void OnEnable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            //If we are aiming, set the player object's rotation around the y-axis to that of the camera
            playerObj.transform.rotation = Quaternion.Euler(new Vector3(0f, playerCam.transform.rotation.eulerAngles.y, 0f));
        }
        
    }

    private void FixedUpdate()
    {
        //Old camera rotation system
        /*
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        followTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        */
    }

	
	void OnAim()
	{
        isAiming = !isAiming;


        if (isAiming)
        {
            //zoomCam.gameObject.transform.parent = null;

            //zoomCam.gameObject.transform.position = new Vector3()
            zoomCam.Priority += 10;
            //zoomCam.gameObject.transform.position = zoomCopy.gameObject.transform.position;
           // zoomCam.gameObject.transform.rotation = zoomCopy.gameObject.transform.rotation;
            //zoomCam.

        }
        else zoomCam.Priority -= 10;
	}
}
