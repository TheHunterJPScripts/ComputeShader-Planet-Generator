using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public Gradient gradient;
    static public Gradient gGradient;

    private void Awake() {
        gGradient = gradient;
    }
    public void Recalculate() {
        gGradient = gradient;
    }
}
