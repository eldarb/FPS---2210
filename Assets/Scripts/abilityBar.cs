using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class abilityBar : MonoBehaviour
{
    [SerializeField] List<Image> A = new List<Image>();
    [SerializeField] List<Image> Abckgrnd = new List<Image>();
    [SerializeField] Color selectedColor;
    [SerializeField] Color defaultColor;

    
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        updateAbilities();
    }

    public void cooldown(int selected)
    {
        timer = gameManager.instance.playerScript.abilities[selected].cooldown / 8;
        StartCoroutine(Refill(selected));
    }

    IEnumerator Refill(int selected)
    {
        A[selected].fillAmount = 0;
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(timer);
            A[selected].fillAmount += 0.125f;
            Debug.Log("Refill abilityBar " + selected);
        }
    }
    public void updateAbilities()
    {
        for(int i = 0; i < A.Count; i++)
        {
            A[i].color = gameManager.instance.playerScript.abilities[i].color;
            if (i == gameManager.instance.playerScript.selected)
            {
                Abckgrnd[i].color = selectedColor;
            }
            else
            {
                Abckgrnd[i].color = defaultColor;
            }
        }
    }
}
