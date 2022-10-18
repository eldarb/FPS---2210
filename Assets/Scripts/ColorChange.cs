using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField] Light lightColor;
    [SerializeField] float lerp;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(randomColor());
    }

    IEnumerator randomColor()
    {
        while(true)
        {
            Color startColor = lightColor.color;
            Color endColor = new Color32(System.Convert.ToByte(Random.Range(0, 255)), System.Convert.ToByte(Random.Range(0, 255)), System.Convert.ToByte(Random.Range(0, 255)), 255);

            float t = 0;
            while (t < 1)
            {
                t = Mathf.Min(1, lerp + Time.deltaTime); // Multiply Time.deltaTime by some constant to speed/slow the transition.
                lightColor.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
