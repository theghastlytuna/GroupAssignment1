using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : CommandAbstract
{
    TpMovement player;
    private void Start()
    {
        player = FindObjectOfType<TpMovement>();
    }
    public override void execute()
    {
        player.OnJump();
    }
}