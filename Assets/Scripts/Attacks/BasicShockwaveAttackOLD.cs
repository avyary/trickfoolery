using System.Collections;
using UnityEngine;

namespace Attacks
{
    // Initial Status:
    // Center: -0.001 0 0.03
    // Size: 0.083 0.07 0.05
    
    //*******************************************************************************************
    // BasicShockwaveAttackOld
    //*******************************************************************************************
    /// <summary>
    /// Attack subclass that implements a shockwave projectile that grows in size as it
    /// moves via a BoxCollider. Creates a single shockwave that permeates the area and
    /// temporarily persists at max size until it ceases to exist.
    /// </summary>
    public class BasicShockwaveAttackOld: Attack
    {
        protected const float NormalZ = 0.1f;
        
        // [SerializeField] public Rigidbody shockwave;
        // [SerializeField] public Transform shockwaveOrigin;
        public static bool instantiateShockwave = false;
        
        // Start with initialCenter.z = 5
        // initialSize.z = 15
        Vector3 initialCenter;
        Vector3 initialSize;

        /// <summary>
        /// Extends the parent class initialization of bookkeeping structures with adjustments to the
        /// <b> startupTime </b>, <b> activeTime </b>, <b> recoveryTime </b>, <b> damage </b>,
        /// <b> stunTime </b>, and <b> range </b> attributes associated with this specific Attack.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            startupTime = 0.25f;
            activeTime = 5f;
            recoveryTime = 1f;
            damage = 100;
            stunTime = 2;
            range = 5f;
            
            Debug.Log("Shockwave basic attack set up: \n" +
                      "range:" + range);
        }
        
        /// <summary>
        /// Interpolates the position and size of the BoxCollider associated with this GameObject
        /// until the <i> activeTime </i> duration of time passes, then freezes this Attack for
        /// <i> activeTime </i> duration while resetting the BoxCollider size and position.
        /// </summary>
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

            if (boxCollider.center != initialCenter && boxCollider.size != initialSize)
            {
                Debug.Log("Shockwave basic attack update");
            }
            

            // Initialize the shockwave object. 
            // instantiateShockwave = true;

            // Rigidbody shockwaveInstance;
            // Vector3 shockwaveO = transform.position + transform.forward * 2.0f;
            // shockwaveInstance = Instantiate(shockwave, shockwaveO, Quaternion.identity) as Rigidbody;
            // shockwaveInstance.AddForce(shockwaveOrigin.forward * 10f);
            
            // wait for ShockwaveLastTime = 3 seconds
            // System.Threading.Thread.Sleep(3000);
            StartCoroutine(Freeze(activeTime));
            
            // reset the hit box to original states
            boxCollider.center = initialCenter;
            boxCollider.size = initialSize;
            
            if (boxCollider.center == initialCenter && boxCollider.size == initialSize)
            {
                Debug.Log("Shockwave basic attack reset");
            }
        }
        
        /// <summary>
        /// Sets this GameObject's position to its current position and waits for a duration of time.
        /// </summary>
        /// <param name="sec"> The duration of time to wait after resetting this GameObject's position in
        /// seconds. </param>
        IEnumerator Freeze(float sec) {
            // Save the object's current position
            Vector3 originalPosition = transform.position;

            // Set the object's position to its current position, effectively "freezing" it
            transform.position = originalPosition;

            // Wait for ___ seconds
            yield return new WaitForSeconds(sec);
        }
    }
    
}
