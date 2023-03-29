using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    [HideInInspector]
    public GameObject player;
    public LayerMask obstacleMask;
    public LayerMask targetMask;

    public bool active = false;

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

            Debug.Log("Player not visible");
            // else 
            // {
            //     Debug.Log("Player not visible");
            // }
        }
    }

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

    bool PlayerIsVisible() {
        float dstToPlayer = Vector3.Distance(transform.position, player.transform.position);
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
