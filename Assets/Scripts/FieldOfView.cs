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
        }
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
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
}
