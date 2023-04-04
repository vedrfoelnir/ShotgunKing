using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUIComponent : MonoBehaviour
{


    public Text uiText;

    // Update is called once per frame
    void Update()
    {
        uiText.text =
            " HP: " + PlayerController.Instance.HP
            + "\n" +
            " " + GameStateManager.Instance.getCurrentLevel()
            + "\n" +
            " State: " + GameStateManager.Instance.State.ToString()
            ;
    }
}
