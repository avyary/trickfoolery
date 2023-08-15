using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*******************************************************************************************
// BasicShockwaveAttackNew
//*******************************************************************************************
/// <summary>
/// Attack subclass that implements a shockwave projectile that grows in size as it
/// moves via the object Transform.
/// </summary>
public class BasicShockwaveAttackNew : Attack
{
    [SerializeField]
    private float ShockwaveLength;
    [SerializeField]
    private float ShockwaveSize;

    private bool shouldLerp = false;
    private float timeStartedLerping;
    
    private BoxCollider boxCollider;

    // Start with initialCenter.z = 5
    // initialSize.z = 15
    Vector3 initialCenter = new Vector3(0, 0, 0);
    Vector3 initialSize = new Vector3(1, 1, 1);

    Vector3 targetCenter;
    Vector3 targetSize;

    protected override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider = (BoxCollider) _collider;
    }

    void Update()
    {   
        if (shouldLerp)
        {
            transform.localPosition = ShockwaveLerp(initialCenter, targetCenter, timeStartedLerping);
            transform.localScale = ShockwaveLerp(initialSize, targetSize, timeStartedLerping);
        }
    }

    public override void Activate() {
        base.Activate();
        _renderer.enabled = true;
        StartAttack();
    }
    
    public override void Deactivate() {
        shouldLerp = false;
        transform.localPosition = initialCenter;
        transform.localScale = initialSize;;
        _renderer.enabled = false;
        base.Deactivate();
    }
    
    public void StartAttack()
    {
        timeStartedLerping = Time.time;
        targetCenter = new Vector3(initialCenter.x, initialCenter.y, initialCenter.z + ShockwaveLength);
        targetSize = new Vector3(initialSize.x + ShockwaveSize, initialSize.y, initialSize.z);
        shouldLerp = true;
    }
    
    public void EndAttack()
    {
        _renderer.enabled = false;
        shouldLerp = false;
    }

    public Vector3 ShockwaveLerp(Vector3 start, Vector3 end, float timeStartedLerping)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / activeTime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;
    }
}