using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportFromPocketToFirstLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Scene scene = SceneManager.GetActiveScene();
        //If you are in the pocket dimension you can tp only to the begging
        if (scene.name == "PocketDimension")
            SceneManager.LoadScene("Prison Room 1");
        //gameManager.instance.inPocketDimension = false;
    }
}
