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

    public bool creditsOpen;

    private void Awake() {
        instance = this;
    }
    private void Start()
    {
        sensOption.value = PlayerPrefs.GetFloat("sensitivity");
        volOption.value = PlayerPrefs.GetFloat("volume");
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
            //else
            //{
            //    isPaused = !isPaused;
            //    pauseMenu.SetActive(isPaused);
            //    if (isPaused)
            //    {
            //        cursorLockPause();
            //    }
            //    else
            //    {
            //        cursorUnLockUnPause();
            //    }
            //}
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
        PlayerPrefs.Save();
    }
}
