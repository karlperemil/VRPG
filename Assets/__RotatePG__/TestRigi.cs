using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRigi : MonoBehaviour
{
    private Rigidbody charRig;

    // Start is called before the first frame update
    void Start()
    {
        charRig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector3 vel2 = charRig.velocity;
        vel2.x = Mathf.Sin(Time.realtimeSinceStartup) * 2.5f;
        charRig.velocity = vel2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
