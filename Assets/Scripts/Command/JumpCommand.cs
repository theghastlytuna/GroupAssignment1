using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : CommandAbstract
{
    TpMovement player;
    private void Start()
    {
        Invoke("findPlayer", 1);
    }
    public override void execute()
    {
        Debug.Log("Jumping");
        //if (player == null) return;
        player.OnJump();
    }

    void findPlayer()
    {
        player = FindObjectOfType<TpMovement>();
    }
}