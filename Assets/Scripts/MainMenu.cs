using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public Slider sensOption;
    public TMP_Text sensOptText;
    public Slider volOption;
    public TMP_Text volOptText;
    public AudioSource aud;

    public bool creditsOpen;

    private void Awake() {
        instance = this;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("sensitivity")) {
            sensOption.value = PlayerPrefs.GetFloat("sensitivity");
        } else {
            sensOption.value = 0.5f;
            PlayerPrefs.SetFloat("sensitivity", 0.5f);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("volume")) {
            volOption.value = PlayerPrefs.GetFloat("volume");
        } else {
            volOption.value = 0.5f;
            PlayerPrefs.SetFloat("volume", 0.5f);
            PlayerPrefs.Save();
        }

        aud.volume = PlayerPrefs.GetFloat("volume");
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (MainMenu.instance.creditsOpen)
            {
                creditsMenu.SetActive(false);
                creditsOpen = !creditsOpen;
            }
        }
    }

    public void openCredits()
    {
        creditsMenu.SetActive(true);
        creditsOpen = true;
    }

    public void closeCredits()
    {
        creditsMenu.SetActive(false);
        creditsOpen = false;
    }

    public void openOptionsMenu() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void closeOptionsMenu() {
        float vol = PlayerPrefs.GetFloat("volume");
        float sens = PlayerPrefs.GetFloat("sensitivity");
        aud.volume = vol;
        volOption.value = vol;
        sensOption.value = sens;
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void sensChange() {
        sensOptText.text = sensOption.value.ToString("F");
    }

    public void volChange() {
        aud.volume = volOption.value;
        volOptText.text = volOption.value.ToString("F");
    }

    public void applyOptions() {
        aud.volume = volOption.value;
        PlayerPrefs.SetFloat("sensitivity", sensOption.value);
        PlayerPrefs.SetFloat("volume", volOption.value);
        PlayerPrefs.Save();
    }
}
