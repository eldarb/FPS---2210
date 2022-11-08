using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityMenu : MonoBehaviour
{
    [SerializeField] public List<ability> abilities = new List<ability>();
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (gameManager.instance.playerScript.abilities.Contains(abilities[i]))
            {
                gameManager.instance.menuAbilities[i].SetActive(true);
            }
            else
            {
                gameManager.instance.menuAbilities[i].SetActive(false);
            }
        }
    }
}

