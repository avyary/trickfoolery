using Attacks;
using UnityEngine;

//*******************************************************************************************
// ShockwaveCreator
//*******************************************************************************************
/// <summary>
/// Instantiates a ShockwavePrefab according to the timer associated with the
/// BasicShockwaveAttack class.
/// </summary>
public class ShockwaveCreator : MonoBehaviour
{
    [SerializeField] public GameObject shockwavePrefab;
    
    private void Start()
    {
        
    }

    void Update()
    {
        if (BasicShockwaveAttack.instantiateShockwave)
        {
            // Debug.Log("Shockwave create 1");
            createShockwave();
            // Debug.Log("Shockwave create 2");
            BasicShockwaveAttack.instantiateShockwave = false;
        }
    }

    /// <summary>
    /// Instantiates a shockwave GameObject and sets its position to this GameObject's position.
    /// </summary>
    private void createShockwave()
    {
        GameObject node = Instantiate(shockwavePrefab, transform);
        node.transform.position = transform.position;
    }
    
}
