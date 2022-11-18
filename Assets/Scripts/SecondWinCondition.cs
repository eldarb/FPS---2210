using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondWinCondition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.hiddenWinConditionPanel.SetActive(true);
        gameManager.instance.cursorLockPause();
        gameManager.instance.controlHints.SetActive(false);
        gameManager.instance.abilityBar.gameObject.SetActive(false);
    }
}
