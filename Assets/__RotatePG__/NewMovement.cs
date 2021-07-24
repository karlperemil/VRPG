using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class NewMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference actionReference;


    [SerializeField]
    private InputActionReference rightHandPrimary;

    public float moveSpeed = 30;

    public GameObject player;
    private Rigidbody charRig;

    public Vector3 XRRigOffsetPos { get; private set; }

    public GameObject XRRig;
    private float boostCooldownInSeconds = 1.5f;
    private float lastBoostTimeInSeconds = 0f;
    private Vector3 boostVector;
    private float boostDuration = 0.25f;
    public float boostMovementSpeed = 3f;
    private float RotateSpeed = 1f;
    private Quaternion lookRotation;

    public Vector2 axisInput;

    void Start(){
        charRig = player.GetComponent<Rigidbody>();
        XRRigOffsetPos = player.transform.position - XRRig.transform.position;
    }

    private void Update() {
        Vector3 pos = player.transform.position - XRRigOffsetPos;
        Vector3 posNow = XRRig.transform.position;
        posNow.x = pos.x;
        posNow.z = pos.z;
        XRRig.transform.position = posNow;
    }

    private void FixedUpdate() {
        // Vector3 vel2 = charRig.velocity;
        // vel2.x = Mathf.Sin(Time.realtimeSinceStartup) * 2.5f;
        // charRig.velocity = vel2;
        //return;


        
        if (actionReference != null && actionReference.action != null && player != null)
        {
            Vector2 value = actionReference.action.ReadValue<Vector2>();
            float x = value.x;
            float z = value.y;
            axisInput = value;

            Vector3 direction = new Vector3(x,0f,z) * moveSpeed;
            direction.y = charRig.velocity.y;
            charRig.velocity = direction;

            
            lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            if (direction.x != 0 && direction.z != 0){
                player.transform.rotation = lookRotation;
            }
            
        }
        
        
        
        if (rightHandPrimary != null && rightHandPrimary.action != null && player != null)
        {
            bool value = rightHandPrimary.action.ReadValue<float>() > .5f;
            if(value && IsBoostOnCooldown() == false){
                Boost();
            }
        }

        if(Time.realtimeSinceStartup < lastBoostTimeInSeconds + boostDuration){
            Vector3 vel = charRig.velocity;
            vel += boostVector * boostMovementSpeed;
            charRig.velocity = vel;
        }
        return;
    }

    private void Boost()
    {
        Debug.Log("boost");
        lastBoostTimeInSeconds = Time.realtimeSinceStartup;
        boostVector = player.transform.forward;
    }

    private bool IsBoostOnCooldown()
    {
        return Time.realtimeSinceStartup < lastBoostTimeInSeconds + boostCooldownInSeconds;
    }
}
