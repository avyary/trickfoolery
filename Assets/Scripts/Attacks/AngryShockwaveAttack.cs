using UnityEngine;

namespace Attacks
{
    //*******************************************************************************************
    // AngryShockwaveAttack
    //*******************************************************************************************
    /// <summary>
    /// Attack subclass that implements a shockwave projectile that grows in size as it
    /// moves via a BoxCollider. Moves at a faster rate than a normal shockwave attack.
    /// </summary>
    public class AngryShockwaveAttack : Attack
    {
        protected const float AngryZ = 70f;

        [SerializeField] public Rigidbody shockwave;
        [SerializeField] public Transform shockwaveOrigin;
        
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
            startupTime = 0.5f;
            activeTime = 7f;
            recoveryTime = 3f;
            damage = 150;
            stunTime = 2;
            range = 20f;
            
            Debug.Log("Shockwave angry attack set up: \n" +
                      "range:" + range);
        }
        
        /// <summary>
        /// Increases the size of the BoxCollider associated with this GameObject gradually until the
        /// <i> activeTime </i> duration of time passes, resetting the BoxCollider size upon completion.
        /// </summary>
        public void SetAngryShockwaveAttack()
        {
            // Progressive attack range
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            initialCenter = boxCollider.center;
            initialSize = boxCollider.size;
            
            float timer = 0f;

            while (timer <= activeTime)
            {
                float t = timer / activeTime;
                float targetZCenter = Mathf.Lerp(initialCenter.z, AngryZ, t);
                float targetZValue = Mathf.Lerp(initialSize.z, 2*AngryZ, t);

                Vector3 targetCenter = new Vector3(initialCenter.x, initialCenter.y, targetZCenter);
                Vector3 targetSize = new Vector3(initialSize.x, initialSize.y, targetZValue);
                
                boxCollider.center = targetCenter;
                boxCollider.size = targetSize;
                
                timer += Time.deltaTime;
            }
            Debug.Log("Shockwave angry attack update");
            
            // wait for ShockwaveLastTime = 4 seconds
            // System.Threading.Thread.Sleep(4000);
            
            // reset the hit box to original states
            boxCollider.center = initialCenter;
            boxCollider.size = initialSize;
            
            Debug.Log("Shockwave angry attack reset");
        }
    }
}
