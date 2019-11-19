using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMe
{
    static public Color Color(GradientColorKey[] gradientKeys, Vector3 vertex, Vector3 normal) {
        Color color = new Color();

        float length = vertex.magnitude - 10;
        float lerp = Mathf.InverseLerp(0, 5, length);
        Debug.Log(length);
        for (int i = 0; i < gradientKeys.Length; i++) {
            if(gradientKeys[i].time > lerp) {
                color = gradientKeys[i - 1].color + (gradientKeys[i].color - gradientKeys[i-1].color) * lerp;
                break;
            }
        }

        return color;
    }
}
