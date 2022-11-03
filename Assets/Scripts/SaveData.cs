using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    static bool inPocketDimension;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        inPocketDimension = false;
    }
}
