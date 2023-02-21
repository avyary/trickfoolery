using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviors
{
    public static void Rotate(GameObject target, float speed) {
        target.transform.Rotate(0.0f, speed, 0.0f);
    }

    public static void MoveForward(GameObject target, float speed) {
        target.transform.position += target.transform.forward * Time.deltaTime * speed;
    }

    public static void MoveToPlayer(GameObject target, GameObject player, float speed) {
        // change to setDestination
        Vector3 toPlayer = player.transform.position - target.transform.position;
        toPlayer.y = 0;
        target.transform.position += (toPlayer.normalized * speed);
        target.transform.rotation = Quaternion.LookRotation(toPlayer, Vector3.up);
    }
}