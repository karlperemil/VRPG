using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RaycastTowardsGround();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag + " other.gameObject.tag");
        if(other.gameObject.tag == "Bullet"){
            Destroy(other.gameObject);
            //Destroy(this.gameObject);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public LayerMask terrainLayer;
    private void RaycastTowardsGround(){
        Vector3 pos = transform.position;
        pos.y += 3f;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, terrainLayer))
        {
            Debug.DrawRay(pos, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow,5f);
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y;
            transform.position = newPos;
            Debug.Log("Raycast hit");
        }
        else
        {
            Debug.DrawRay(pos, transform.TransformDirection(Vector3.down) * hit.distance, Color.red,5f);
            Debug.Log("Raycast Did not Hit");
        }
    }
}
