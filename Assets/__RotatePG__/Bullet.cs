using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float movementSpeed = 3f;
    private float startTimeInSeconds;
    public float lifeTimeInSeconds = 5f;

    // Start is called before the first frame update
    void Awake ()
    {
        startTimeInSeconds = Time.realtimeSinceStartup;
        Debug.Log(" Awake");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;


        if(Time.realtimeSinceStartup > startTimeInSeconds + lifeTimeInSeconds){
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Terrain"){
            Destroy(this.gameObject);
        }    
    }

}
