using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *******************************************************************************************
// FieldOfView
//*******************************************************************************************
/// <summary>
/// Contains various methods to locate objects in a wedged area (from a sphere)
/// excluding obstacles. Additional methods find the player within the boundaries and
/// conduct player detection.
/// </summary>
public class FieldOfView : MonoBehaviour
{
    public float detectRadius;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    [HideInInspector]
    public GameObject player;
    public LayerMask obstacleMask;
    public LayerMask targetMask;

    public bool active = true;

    /// <summary>
    /// Continuously checks if the player GameObject has been sighted until the active flag is cleared
    /// externally or by sighting the player GameObject. Upon sighting the player GameObject, delays
    /// for a duration of time before invoking a provided function signature and clearing the active flag.
    /// </summary>
    /// <param name="delay"> The amount to deplete the player's health. </param>
    /// <param name="callback"> The Enemy that unleashed an attack on the player. </param>
    public IEnumerator FindPlayer(float delay, System.Action callback)
    {
        while (active)
        {
            yield return new WaitForSeconds(delay);
            if (PlayerIsVisible())
            {
                callback();
                active = false;
                yield break;
            }

            // Debug.Log("Player not visible");
            // else 
            // {
            //     Debug.Log("Player not visible");
            // }
        }
    }

    /// <summary>
    /// Finds all Colliders within a wedged area that do not belong to obstacle GameObjects. The wedged area
    /// is defined by the <i> viewRadius </i> for length of the sides and <i> viewAngle </i> as the arc extent.
    /// </summary>
    /// <returns> All non-obstacle colliders located within a wedged area. </returns>
    public List<Collider> FindVisibleTargets()
    {
        List<Collider> visibleTargets = new List<Collider>();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Collider target = targetsInViewRadius[i];

            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    visibleTargets.Add(target);
                }
            }
        }

        return visibleTargets;
    } 

    /// <summary>
    /// Checks if the player GameObject is found within a keyhole-shaped area defined by the <i> detectRadius
    /// </i> and <i> viewRadius </i> for length and the <i> viewAngle </i> as the arc extent.
    /// </summary>
    /// <returns> If the player position has successfully been located within a keyhole-shaped area. </returns>
    bool PlayerIsVisible() {
        float dstToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (dstToPlayer < detectRadius) {
            return true;
        }
        if (dstToPlayer > viewRadius)
        {
            return false;
        }
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Finds a unit vector from the provided angle. If the angle is specified in local space, adds the
    /// euler angle rotation along the y-axis to the provided angle before calculating the unit vector.
    /// </summary>
    /// <param name="angleInDegrees"> The angle used to calculate the unit vector in degrees. </param>
    /// <param name="angleIsGlobal"> Marks if the angle is in world space. </param>
    /// <returns> The unit vector calculated from the provided angle. </returns>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    public 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
}
