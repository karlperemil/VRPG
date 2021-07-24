using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class LeftHandStickMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference actionReference;


    [SerializeField]
    private InputActionReference rightHandPrimary;

    public float moveSpeed = 30;

    public GameObject character;

    public GameObject characterHolder;

    public GameObject lookPoint;

    public GameObject XRRig;
    private float boostCooldownInSeconds = 1.5f;
    private float lastBoostTimeInSeconds = 0f;
    private Vector3 boostVector;
    private float boostDuration = 0.25f;
    public float boostMovementSpeed = 3f;

    void Update()
    {
        if(GameSingleton.Instance.gameStarted == false)
            return;
        
        if (actionReference != null && actionReference.action != null && character != null)
        {
            Vector2 value = actionReference.action.ReadValue<Vector2>();
            Vector3 characterPos = characterHolder.transform.localPosition;

            characterPos.z += value.y * 0.0001f * moveSpeed;
            characterPos.x += value.x * 0.0001f * moveSpeed;

            characterHolder.transform.localPosition = characterPos;

            //XRRig.transform.localPosition = characterHolder.transform.localPosition;

            //Vector3.Lerp()

            //try lerping

            Vector3 localPos = new Vector3();
            localPos.z = value.y * .17f;
            localPos.x = value.x * .17f;
            lookPoint.transform.localPosition = localPos;


            Vector3 targetPostition = new Vector3( lookPoint.transform.position.x, 
                                        character.transform.position.y, 
                                        lookPoint.transform.position.z ) ;
            if(value.x != 0 && value.y != 0){
                //character.transform.LookAt
                //character.transform.LookAt( targetPostition,Vector3.back ) ;
                character.transform.LookAt( lookPoint.transform.position, Vector3.back ) ;
                //character.transform.rotation = Quaternion.Euler(character.transform.localRotation.eulerAngles.y, 0 , 0);
                character.transform.localRotation = Quaternion.Euler(0,character.transform.localRotation.eulerAngles.y,0);
            }
                
        }



        if (rightHandPrimary != null && rightHandPrimary.action != null && character != null)
        {
            bool value = rightHandPrimary.action.ReadValue<float>() > .5f;
            if(value && IsBoostOnCooldown() == false){
                Boost();
            }
        }

        if(Time.realtimeSinceStartup < lastBoostTimeInSeconds + boostDuration){
            characterHolder.transform.position += boostVector * Time.deltaTime * boostMovementSpeed;
        }

    }

    private void Boost()
    {
        Debug.Log("boost");
        lastBoostTimeInSeconds = Time.realtimeSinceStartup;
        boostVector = character.transform.forward;
    }

    private bool IsBoostOnCooldown()
    {
        return Time.realtimeSinceStartup < lastBoostTimeInSeconds + boostCooldownInSeconds;
    }
}
