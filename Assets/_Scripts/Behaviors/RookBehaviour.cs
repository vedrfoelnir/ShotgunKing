using UnityEngine;
using System.Collections;

public class RookBehaviour : EnemyBehaviour
{
    public override IEnumerator MoveAction()
    {
        Debug.Log("Rook Move");
        yield return new WaitForSeconds(1);
    }
}

