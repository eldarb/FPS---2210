using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityMenu : MonoBehaviour
{
    [SerializeField] ability pd;
    [SerializeField] ability fb;
    [SerializeField] ability ib;
    [SerializeField] ability sl;
    [SerializeField] ability rt;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager.instance.playerScript.abilities.Contains(pd))
        {
            gameManager.instance.abilityMenu[0].SetActive(true);
        }
        else
        {
            gameManager.instance.abilityMenu[0].SetActive(false);
        }

        if (gameManager.instance.playerScript.abilities.Contains(fb))
        {
            gameManager.instance.abilityMenu[2].SetActive(true);
        }
        else
        {
            gameManager.instance.abilityMenu[2].SetActive(false);
        }

        if (gameManager.instance.playerScript.abilities.Contains(ib))
        {
            gameManager.instance.abilityMenu[1].SetActive(true);
        }
        else
        {
            gameManager.instance.abilityMenu[1].SetActive(false);
        }

        if (gameManager.instance.playerScript.abilities.Contains(sl))
        {
            gameManager.instance.abilityMenu[3].SetActive(true);
        }
        else
        {
            gameManager.instance.abilityMenu[3].SetActive(false);
        }

        if (gameManager.instance.playerScript.abilities.Contains(rt))
        {
            gameManager.instance.abilityMenu[4].SetActive(true);
        }
        else
        {
            gameManager.instance.abilityMenu[4].SetActive(false);
        }
    }
}
}
