using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public Slider sensOption;
    public TMP_Text sensOptText;
    public Slider volOption;
    public TMP_Text volOptText;

    private void Awake() {
        instance = this;
    }

    public void openOptionsMenu() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void openMainMenu() {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void sensChange() {
        sensOptText.text = sensOption.value.ToString("F");
    }

    public void volChange() {
        volOptText.text = volOption.value.ToString("F");
    }

    public void applyOptions() {
        PlayerPrefs.SetFloat("sensitivity", sensOption.value);
        PlayerPrefs.SetFloat("volume", volOption.value);
    }
}
