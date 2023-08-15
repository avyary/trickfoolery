using System;
using System.Collections;
using System.Collections.Generic;
using Attacks;
using UnityEngine;

//*******************************************************************************************
// Shockwave
//*******************************************************************************************
/// <summary>
/// Implements a shockwave projectile that grows in length as it moves.
/// </summary>
public class Shockwave : MonoBehaviour
{
    protected float growthRate = 8f; // rate of growth per second
    protected float finalSize = 40f; // final size of the square
    protected const float ShockwaveLastTime = 7f;
    
    private void Start()
    {
        transform.localScale = new Vector3(15f, 20f, 0f);
        // Rigidbody shockwaveInstance;
        // Vector3 shockwaveO = transform.position + transform.forward * 2.0f;
        // shockwaveInstance = Instantiate(shockwave, shockwaveO, Quaternion.identity) as Rigidbody;
        // shockwaveInstance.AddForce(shockwaveOrigin.forward * 10f);
        StartCoroutine(CreateShockwave());
    }


    // private void Update()
    // {
    //     if (BasicShockwaveAttack.instantiateShockwave)
    //     {
    //         // // initialize the size of the shockwave
    //         // transform.localScale = new Vector3(1f, 2f, 0f);
    //         
    //         BasicShockwaveAttack.instantiateShockwave = false;
    //     }
    // }

    private IEnumerator CreateShockwave()
    {
        Debug.Log("Shockwave occurs");

        float currentLength = 0f;
        
        while (currentLength < finalSize)
        {
            Vector3 initialScale = transform.localScale;
            Vector3 initialPosition = transform.position;
            currentLength += growthRate * Time.deltaTime;
            
            transform.position = initialPosition + (transform.forward * currentLength);
            transform.localScale = new Vector3(initialScale.x, initialScale.y, currentLength);
            yield return null;
        }
        // Destroy(gameObject, ShockwaveLastTime); // destroy the square after __ seconds when it reaches its final size
        
    }
}
