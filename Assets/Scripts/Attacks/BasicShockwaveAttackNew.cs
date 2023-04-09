using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShockwaveAttackNew : Attack
{
    protected const float ShockwaveLength = 5f;
    protected const float ShockwaveSize = 10f;
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
        damage = 10;
        stunTime = 1;
        range = 4f;
        

        Debug.Log("Shockwave basic attack set up: \n" +
                  "activeTime: " + activeTime + "\n" +
                  "range: " + range);
    }
    
    void Update()
    {   
        _boxCollider = GetComponent<BoxCollider>();
        Debug.Log("box center: " + _boxCollider.center + "\n" +
                  "box size: " + _boxCollider.size + "\n");

        if (shouldLerp)
        {
            Vector3 targetCenter = new Vector3(initialCenter.x, initialCenter.y, initialCenter.z + ShockwaveLength);
            Vector3 targetSize = new Vector3(initialSize.x, initialSize.y, initialSize.z + ShockwaveSize);
            _boxCollider.center = ShockwaveLerp(initialCenter, targetCenter, timeStartedLerping);
            _boxCollider.size = ShockwaveLerp(initialSize, targetSize, timeStartedLerping);
            
            Debug.Log("box center: " + _boxCollider.center + "\n" +
                      "box size: " + _boxCollider.size + "\n");
            
            _collider = _boxCollider;

            //
            StartCoroutine(Reset(activeTime));
            
        }

    }
    
    public void StartAttack()
    {
        initialCenter = _boxCollider.center;
        initialSize = _boxCollider.size;
        timeStartedLerping = Time.time;
        shouldLerp = true;
    }
    
    public void EndAttack()
    {
        shouldLerp = false;
    }

    public Vector3 ShockwaveLerp(Vector3 start, Vector3 end, float timeStartedLerping)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / (ActiveTime - 1);

        var result = Vector3.Lerp(start, end, percentageComplete);
        
        return result;
    }
    
        
    IEnumerator Reset(float sec) {
        Debug.Log("Reset");

        // Wait for ___ seconds
        yield return new WaitForSeconds(sec);
        shouldLerp = false;
        _boxCollider.center = initialCenter;
        _boxCollider.size = initialSize;;
    }
    
    
}