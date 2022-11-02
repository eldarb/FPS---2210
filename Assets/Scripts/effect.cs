using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class effect : ScriptableObject
{
    [SerializeField] public int efctdmg;
    [SerializeField] public int efctdur;
    [SerializeField] public bool lastingdmg;
    [SerializeField] public int wait;
    [SerializeField] public int lowspeed;
}