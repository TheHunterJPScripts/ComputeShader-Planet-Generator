using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traker : MonoBehaviour
{
    public Transform planet;
    GameObject obj;
    Camera cam;
    Vector3 center;

    void Start()
    {
        cam = Camera.main;
        center = planet.position;
        obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint = new RaycastHit();
        if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity)) {
            obj.SetActive(true);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, (hitPoint.point - center).normalized);
            obj.transform.position = hitPoint.point;
            obj.transform.rotation = rotation;
        }else
            obj.SetActive(false);
    }
}
