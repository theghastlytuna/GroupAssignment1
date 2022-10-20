using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSupervisor : MonoBehaviour
{
    CommandAbstract[] commands = new CommandAbstract[100];
    public JumpCommand jumpCommand;

    private void Start()
    {
        commands[0] = jumpCommand;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Debug.Log("P Key Pressed");
            commands[0].execute();
        }
        //Debug.Log("Loop");
        //commands[0].execute();
    }
}