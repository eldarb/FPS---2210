using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player -----")]
    public GameObject player;
    public PlayerController playerScript;

    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public GameObject currentMenu;

    public bool isPaused;

    // Start is called before the first frame update
    // Creates the instance that holds the playerController
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
            if (isPaused)
                cursorLockPause();
            else
                cursorUnlockUnpause();
        }
    }
    public void cursorLockPause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void cursorUnlockUnpause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
