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

    /// <summary>
    /// Extends the parent class initialization of bookkeeping structures with additional caching of BoxCollider
    /// data pertaining to this GameObject.
    /// </summary>
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

    /// <summary>
    /// Extends the parent class Attack activation with enabling the MeshRenderer and triggering the
    /// shockwave expansion logic.
    /// </summary>
    public override void Activate() {
        base.Activate();
        _renderer.enabled = true;
        StartAttack();
    }
    
    /// <summary>
    /// Extends the parent class Attack deactivation with disabling the MeshRenderer and resetting the shockwave
    /// position and size.
    /// </summary>
    public override void Deactivate() {
        shouldLerp = false;
        transform.localPosition = initialCenter;
        transform.localScale = initialSize;;
        _renderer.enabled = false;
        base.Deactivate();
    }
    
    /// <summary>
    /// Toggles the flag to begin interpolating the size and position of the shockwave Attack on update,
    /// setting the necessary data for the target position and size to interpolate to.
    /// </summary>
    public void StartAttack()
    {
        timeStartedLerping = Time.time;
        targetCenter = new Vector3(initialCenter.x, initialCenter.y, initialCenter.z + ShockwaveLength);
        targetSize = new Vector3(initialSize.x + ShockwaveSize, initialSize.y, initialSize.z);
        shouldLerp = true;
    }
    
    /// <summary>
    /// Disables the MeshRenderer and clears the flag to interpolate on update.
    /// </summary>
    public void EndAttack()
    {
        _renderer.enabled = false;
        shouldLerp = false;
    }

    /// <summary>
    /// Interpolates a vector from a provided starting point to a provided end point for a single frame.
    /// Interpolation progress is tracked through <i> timeStartedLerping </i> and Attack <i> activeTime </i>.
    /// </summary>
    /// <param name="start"> The starting point of the Interpolation. </param>
    /// <param name="end"> The end point of the Interpolation. </param>
    /// <param name="timeStartedLerping"> The starting time of the interpolation. </param>
    /// <returns> The vector value of the interpolation result for this frame. </returns>
    public Vector3 ShockwaveLerp(Vector3 start, Vector3 end, float timeStartedLerping)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / activeTime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;
    }
}