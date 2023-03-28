using UnityEngine;
using System.Collections;

public class QueenBehaviour : EnemyBehaviour
{
    public override IEnumerator MoveAction()
    {
        Debug.Log("Queen Move");
        yield return new WaitForSeconds(1);
    }
}

