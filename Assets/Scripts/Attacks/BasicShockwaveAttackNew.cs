using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShockwaveAttackNew : Attack
{
    protected const float ShockwaveLength = 5f;
    private const float ActiveTime = 3f;
    
    private bool shouldLerp = false;
    private float timeStartedLerping;
    private BoxCollider _boxCollider;

    // Start with initialCenter.z = 5
    // initialSize.z = 15
    Vector3 initialCenter;
    Vector3 initialSize;
    
    protected override void Start()
    {
        base.Start();
        startupTime = 0.25f;
        activeTime = ActiveTime;
        recoveryTime = 0.5f;
        damage = 25;
        stunTime = 1;
        range = 20f;
        
            
        Debug.Log("Shockwave basic attack set up: \n" +
                  "activeTime: " + activeTime + "\n" +
                  "range: " + range);
    }
    
    void Update()
    {   
        _boxCollider = GetComponent<BoxCollider>();
        
        
        if (shouldLerp)
        {
            Vector3 startCenter = _boxCollider.center;
            Vector3 startSize = _boxCollider.size;
            Vector3 targetCenter = new Vector3(initialCenter.x + ShockwaveLength, initialCenter.y, initialCenter.z);
            Vector3 targetSize = new Vector3(initialSize.x + 4 * ShockwaveLength, initialSize.y, initialSize.z);
            _boxCollider.center = ShockwaveLerp(startCenter, targetCenter, timeStartedLerping);
            _boxCollider.size = ShockwaveLerp(startSize, targetSize, timeStartedLerping);
            
            Debug.Log("box center: " + targetCenter + "\n" +
                      "activeTime: " + targetSize + "\n");
            
            _collider = _boxCollider;
        }
    }
    
    public override void StartAttack()
    {
        timeStartedLerping = Time.time;
        shouldLerp = true;
    }

    public Vector3 ShockwaveLerp(Vector3 start, Vector3 end, float timeStartedLerping)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / ActiveTime;

        var result = Vector3.Lerp(start, end, percentageComplete);
        

        return result;
    }
    
    public void ShockwaveLerp1()
        {
            // // Progressive attack range
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            initialCenter = boxCollider.center;
            initialSize = boxCollider.size;
            
            float timer = 0f;
            
            while (timer <= activeTime)
            {
                float t = timer / activeTime;
                float targetXCenter = Mathf.Lerp(initialCenter.x, ShockwaveLength, t);
                float targetXValue = Mathf.Lerp(initialSize.x, 2*ShockwaveLength, t);

                Vector3 targetCenter = new Vector3(targetXCenter, initialCenter.y, initialCenter.z);
                Vector3 targetSize = new Vector3(targetXValue, initialSize.y, initialSize.z);
                
                boxCollider.center = targetCenter;
                boxCollider.size = targetSize;
                
                timer += Time.deltaTime;
            }

            if (boxCollider.center != initialCenter && boxCollider.size != initialSize)
            {
                Debug.Log("Shockwave basic attack update");
            }
            
            // wait for ShockwaveLastTime = 3 seconds
            StartCoroutine(Freeze(activeTime));
            
            // reset the hit box to original states
            boxCollider.center = initialCenter;
            boxCollider.size = initialSize;
            
            if (boxCollider.center == initialCenter && boxCollider.size == initialSize)
            {
                Debug.Log("Shockwave basic attack reset");
            }
        }
        
        
        IEnumerator Freeze(float sec) {
            // Save the object's current position
            Vector3 originalPosition = transform.position;

            // Set the object's position to its current position, effectively "freezing" it
            transform.position = originalPosition;

            // Wait for ___ seconds
            yield return new WaitForSeconds(sec);
        }
    
    
    
    
}