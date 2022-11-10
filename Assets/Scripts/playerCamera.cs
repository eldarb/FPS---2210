using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamera : MonoBehaviour
{
    [SerializeField] float sensitivity;

    [SerializeField] int lockVerticalMin;
    [SerializeField] int lockVerticalMax;

    [SerializeField] bool invert;

    float xRotataion;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("sensitivity") < 0.1)
            sensitivity = 100;
        else
            sensitivity = PlayerPrefs.GetFloat("sensitivity") * 300;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; //makes the mouse cursor invisible 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //get input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;


        if(!invert)
        {
            xRotataion -= mouseY;
        }
        else
        {
            xRotataion += mouseY;
        }

        //clamp camera rotation
        xRotataion = Mathf.Clamp(xRotataion, lockVerticalMin, lockVerticalMax);

        //rotate the camera on the x-axis
        transform.localRotation = Quaternion.Euler(xRotataion, 0, 0);

        //rotates the player
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
