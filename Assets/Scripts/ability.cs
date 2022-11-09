using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ability : ScriptableObject
{
    public float cooldown;
    public int HPcost;
    public GameObject bullet;
    public GameObject fireEffect;
    public AudioClip sound;
    public Color color;
}
