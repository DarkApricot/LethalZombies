using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutcrackerScript : Ab_BaseMonsterScript, IUpdateMonsterStartPosition
{
    private new void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        // 1 = 1 tile
        CheckForPlayer(7f);
    }
}
