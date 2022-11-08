using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.cursorUnLockUnPause();
        gameManager.instance.pauseMenu.SetActive(false);
        gameManager.instance.isPaused = false;
    }

    // Update is called once per frame
    public void restart()
    {
        gameManager.instance.cursorUnLockUnPause();
        SceneManager.LoadScene("Prison Room 1");
    }

    public void quit()
    {
        Application.Quit();
    }
    public void respawn()
    {
        gameManager.instance.playerScript.respawn();
        gameManager.instance.cursorUnLockUnPause();
    }

    public void startGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void optionsMenu() {
        MainMenu.instance.openOptionsMenu();
    }

    public void optionsBack() {
        MainMenu.instance.openMainMenu();
    }

    public void optionsApply() {
        MainMenu.instance.applyOptions();
    }

    public void sensUpdate() {
        MainMenu.instance.sensChange();
    }

    public void volUpdate() {
        MainMenu.instance.volChange();
    }
}
