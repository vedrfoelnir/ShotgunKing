﻿using UnityEngine;
using System.Collections;

public class PawnBehaviour : EnemyBehaviour
{
    public override IEnumerator MoveAction()
    {
        Debug.Log("Pawn Move");
        yield return new WaitForSeconds(1);
    }
}