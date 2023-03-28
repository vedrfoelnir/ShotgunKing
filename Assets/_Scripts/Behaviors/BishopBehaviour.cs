using UnityEngine;
using System.Collections;

public class BishopBehaviour : EnemyBehaviour
{
    public override IEnumerator MoveAction()
    {
        Debug.Log("Bishop Move");
        yield return new WaitForSeconds(1);
    }
}

