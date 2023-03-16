using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Attacks
{

    public class BasicShockwaveAttack: Attack
    {
        protected const float NormalZ = 20f;
        
        // [SerializeField] public Rigidbody shockwave;
        // [SerializeField] public Transform shockwaveOrigin;
        public static bool instantiateShockwave = false;
        
        // Start with initialCenter.z = 5
        // initialSize.z = 15
        Vector3 initialCenter;
        Vector3 initialSize;

        protected override void Start()
        {
            base.Start();
            startupTime = 0.25f;
            activeTime = 5f;
            recoveryTime = 1f;
            damage = 100;
            stunTime = 2;
            range = 20f;
            
            Debug.Log("Shockwave basic attack set up: \n" +
                      "range:" + range);
        }
        
        public void SetBasicShockwaveAttack()
        {
            // // Progressive attack range
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            initialCenter = boxCollider.center;
            initialSize = boxCollider.size;
            
            float timer = 0f;
            
            while (timer <= activeTime)
            {
                float t = timer / activeTime;
                float targetZCenter = Mathf.Lerp(initialCenter.z, NormalZ, t);
                float targetZValue = Mathf.Lerp(initialSize.z, 2*NormalZ, t);

                Vector3 targetCenter = new Vector3(initialCenter.x, initialCenter.y, targetZCenter);
                Vector3 targetSize = new Vector3(initialSize.x, initialSize.y, targetZValue);
                
                boxCollider.center = targetCenter;
                boxCollider.size = targetSize;
                
                timer += Time.deltaTime;
            }
            Debug.Log("Shockwave basic attack update");
            
            // Initialize the shockwave object. 
            instantiateShockwave = true;
            // Rigidbody shockwaveInstance;
            // Vector3 shockwaveO = transform.position + transform.forward * 2.0f;
            // shockwaveInstance = Instantiate(shockwave, shockwaveO, Quaternion.identity) as Rigidbody;
            // shockwaveInstance.AddForce(shockwaveOrigin.forward * 10f);
            

            // wait for ShockwaveLastTime = 3 seconds
            // System.Threading.Thread.Sleep(3000);
            
            // reset the hit box to original states
            boxCollider.center = initialCenter;
            boxCollider.size = initialSize;
            
            Debug.Log("Shockwave basic attack reset");
        }
    }
}
