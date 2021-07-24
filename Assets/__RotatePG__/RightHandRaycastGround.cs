using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RightHandRaycastGround : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    private InputActionReference actionReference;

    public bool triggerDown = false;
    private bool firstFrameTriggerDown = true;
    private bool firstFrameTriggerUp = false;

    public GameObject bulletPrefab;

    public GameObject gunBarrel;

    public GameObject player;
    void Start()
    {
        
    }

    public float cooldown = 0.5f;
    private float timeInSecondsSinceLastTrigger = 0f;
    private Vector3 lastRaycastHit;

    // Update is called once per frame
    void Update()
    {
        RaycastTowardsGround();


        if(GameSingleton.Instance.gameStarted == false)
            return;

        

        if (actionReference != null && actionReference.action != null)
        {
            float triggerValue = actionReference.action.ReadValue<float>();
            triggerDown = triggerValue > .5f ? true : false;
            
            if(triggerDown && firstFrameTriggerDown && IsOnCoolDown() == false){
                firstFrameTriggerDown = false;
                firstFrameTriggerUp = true;
                print("down");
                ShootBullet(gunBarrel.transform.position, lastRaycastHit);
                
                timeInSecondsSinceLastTrigger = Time.realtimeSinceStartup;
            } 
            if(!triggerDown && firstFrameTriggerUp){
                firstFrameTriggerUp = false;
                firstFrameTriggerDown = true;
                print("up");
                
            }

            if(triggerDown && IsOnCoolDown() == false){

            }
        }
    }

    private bool IsOnCoolDown()
    {
        return Time.realtimeSinceStartup < timeInSecondsSinceLastTrigger + cooldown;
    }

    public LayerMask terrainLayer;

    private void RaycastTowardsGround(){

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, terrainLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Raycast Hit" + hit.point);

            lastRaycastHit = hit.point;
            Vector3 newLookRot = hit.point - gunTurret.transform.position;
            gunTurret.transform.rotation = Quaternion.LookRotation(new Vector3(newLookRot.x,0,newLookRot.z));
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Raycast Did not Hit");
        }
    }

    public GameObject gunTurret;


    private void ShootBullet(Vector3 bulletSpawnPoint, Vector3 bulletLookAtPosition){
        GameObject newBullet = GameObject.Instantiate(bulletPrefab, bulletSpawnPoint,Quaternion.identity);
        //bulletLookAtPosition.y = bulletSpawnPoint.y;
        Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), player.GetComponent<Collider>());
        newBullet.transform.LookAt(bulletLookAtPosition);
    }
}
