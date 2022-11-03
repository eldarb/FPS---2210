using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Scene scene = SceneManager.GetActiveScene();
        //If you are in the pocket dimension you can tp only to the begging
        if (scene.name == "PocketDimension")
            SceneManager.LoadScene("Prison Room 1");
        else if (scene.name == "Prison Room 1")
            SceneManager.LoadScene("Courtyard Room 2");
        else if (scene.name == "Courtyard Room 2")
            SceneManager.LoadScene("Kitchen Room 3");
        else if (scene.name == "Kitchen Room 3")
            SceneManager.LoadScene("Throne Room Room 4");

        //gameManager.instance.inPocketDimension = false;
    }
}
