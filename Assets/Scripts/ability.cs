using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ability : ScriptableObject
{
    public float cooldown;
    // public Effect effect;
    public GameObject bullet;
    public GameObject activateEffect;
    public AudioClip sound;
}
