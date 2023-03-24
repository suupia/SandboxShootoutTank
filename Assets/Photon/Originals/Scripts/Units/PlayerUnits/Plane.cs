using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : NetworkPlayerUnit
{
    NetworkCharacterControllerPrototype cc;

    public override void Spawned()
    {
        base.Spawned();
        cc = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void Move(Vector3 direction)
    {
        cc.Move(direction);

    }

    public override void Action(NetworkButtons buttons, NetworkButtons preButtons)
    {
        
    }
}

