using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSupervisor : MonoBehaviour
{
    CommandAbstract[] commands = new CommandAbstract[4];
    public JumpCommand jumpCommand;

    private void Start()
    {
        commands[1] = jumpCommand;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            //Debug.Log("U Key Pressed");
            if(commands[0] == null) return;
            commands[0].execute();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Debug.Log("I Key Pressed");
            if(commands[1] == null) return;
            commands[1].execute();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            //Debug.Log("O Key Pressed");
            if(commands[2] == null) return;
            commands[2].execute();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Debug.Log("P Key Pressed");
            if(commands[3] == null) return;
            commands[3].execute();
        }
    }
}