using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamera : MonoBehaviour
{
    [SerializeField] int hzntlSens;
    [SerializeField] int vrtclSens;

    [SerializeField] int lockVerticalMin;
    [SerializeField] int lockVerticalMax;

    [SerializeField] bool invert;

    float xRotataion;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; //makes the mouse cursor invisible 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //get input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * hzntlSens;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * vrtclSens;


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
