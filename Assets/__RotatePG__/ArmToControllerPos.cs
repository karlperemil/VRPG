using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmToControllerPos : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject controller;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = controller.transform.localPosition * 2f;
    }
}
