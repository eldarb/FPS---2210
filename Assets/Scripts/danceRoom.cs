using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class danceRoom : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip clip;
    float musicVol;

    // Start is called before the first frame update
    void Awake()
    {
        aud.loop = true;
        aud.clip = clip;
        aud.volume = PlayerPrefs.GetFloat("volume");
        aud.Play();
    }
}
