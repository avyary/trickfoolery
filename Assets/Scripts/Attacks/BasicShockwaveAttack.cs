using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShockwaveAttack : Attack
{
    private bool shouldLerp = false;

    // [SerializeField]
    
    // Vector3 startPosition;
    // [SerializeField]
    // Vector3 endPosition;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float timeStartedLerping;
    private float lerpTime;



    private void StartLerping() 
    {
        timeStartedLerping = Time.time;
        shouldLerp = true;
    }

    public Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1) 
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;
    }

    protected override void Start() 
    {
        base.Start();
        startPosition = _collider.transform.position;
        endPosition = new Vector3(startPosition.x + range, startPosition.y, startPosition.z);
        lerpTime = activeTime;

    }

    void Update() 
    {   
        if (shouldLerp) 
        {
            _collider.transform.position = Lerp(startPosition, endPosition, timeStartedLerping, lerpTime);
            _renderer.transform.position = _collider.transform.position;
        }
        
    }

    public override void StartAttack()
    {
        StartLerping();
    }


        


}
