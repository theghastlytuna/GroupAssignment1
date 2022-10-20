using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;

public class speedPlugin : MonoBehaviour
{

    [DllImport("charModdingDLL")]
    private static extern void setSpeed(float inSpeed);

    [DllImport("charModdingDLL")]
    private static extern float getSpeed();
    
    [DllImport("charModdingDLL")]
    private static extern void setJump(float inJump);
    
    [DllImport("charModdingDLL")]
    private static extern float getJump();
    
    UserInput inputAction;

    private void Start()
    {
        inputAction = InputController.controller.inputAction;

        inputAction.Player.ModSpeed.performed += cntxt => OnModSpeed();

        inputAction.Player.ModJump.performed += cntxt => OnModJump();

    }

    void OnModSpeed()
    {
        foreach(GameObject o in GameObject.FindGameObjectsWithTag("Player"))
        {
            var moveScript = o.GetComponent<TpMovement>();

            if (moveScript != null)
                o.GetComponent<TpMovement>().SetMaxSpeed(getSpeed());
        }

        Debug.Log(getSpeed());

    }

    void OnModJump()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Player"))
        {
            var moveScript = o.GetComponent<TpMovement>();

            if (moveScript != null)
                o.GetComponent<TpMovement>().SetJumpForce(getJump());
        }

        Debug.Log(getJump());
    }
}
