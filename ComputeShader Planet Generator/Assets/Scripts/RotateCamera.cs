using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public void RotateX(float angle) {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    public void RotateY(float angle) {
        transform.rotation = Quaternion.Euler(angle, 0, 0);
    }
}
