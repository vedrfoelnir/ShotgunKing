using UnityEngine;
using System.Collections;

public class KnightBehaviour : EnemyBehaviour
{
    public override IEnumerator MoveAction()
    {
        Debug.Log("Knight Move");
        yield return new WaitForSeconds(1);
    }
}

