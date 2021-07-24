using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightBaseOnTerrain : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject raycastGameObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastTowardsGround();
    }

    public Vector3 offset;
    public LayerMask terrainLayer;
    private void RaycastTowardsGround(){

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(raycastGameObject.transform.position, raycastGameObject.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, terrainLayer))
        {
            Debug.DrawRay(raycastGameObject.transform.position, raycastGameObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Vector3 newPos = transform.transform.position;
            newPos.y = hit.point.y;
            transform.position = newPos  + offset;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }
}
