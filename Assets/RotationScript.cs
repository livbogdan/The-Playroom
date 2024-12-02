using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDemoRotateTransform : MonoBehaviour
{
    public GameObject PivotCenter;
    public float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PivotCenter.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

}
