using UnityEngine;
using System.Collections;

public abstract class EnemyBehaviour : MonoBehaviour
{

    public abstract IEnumerator MoveAction();

    //public abstract (int, int) Move();

}