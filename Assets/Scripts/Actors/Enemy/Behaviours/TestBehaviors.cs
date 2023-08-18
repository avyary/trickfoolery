using UnityEngine;

//*******************************************************************************************
// TestBehaviors
//*******************************************************************************************
/// <summary>
/// Contains various static methods to handle the movement and rotation of an object.
/// </summary>
public class TestBehaviors
{
    /// <summary>
    /// Rotates a GameObject along the y-axis.
    /// </summary>
    /// <param name="target"> The GameObject to be rotated. </param>
    /// <param name="speed"> The angle amount to rotate the GameObject. </param>
    public static void Rotate(GameObject target, float speed) {
        target.transform.Rotate(0.0f, speed, 0.0f);
    }

    /// <summary>
    /// Moves a GameObject along the z-axis.
    /// </summary>
    /// <param name="target"> The GameObject to be translated. </param>
    /// <param name="speed"> The speed at which the GameObject will move. </param>
    public static void MoveForward(GameObject target, float speed) {
        target.transform.position += target.transform.forward * Time.deltaTime * speed;
    }

    /// <summary>
    /// Moves a GameObject towards the other's position with respect to rotation.
    /// </summary>
    /// <param name="target"> The GameObject to be moved. </param>
    /// <param name="player"> The GameObject the target will be moved towards. </param>
    /// <param name="speed"> The intensity of the move towards the player GameObject. </param>
    public static void MoveToPlayer(GameObject target, GameObject player, float speed) {
        // change to setDestination
        Vector3 toPlayer = player.transform.position - target.transform.position;
        toPlayer.y = 0;
        target.transform.position += (toPlayer.normalized * speed);
        target.transform.rotation = Quaternion.LookRotation(toPlayer, Vector3.up);
    }
}