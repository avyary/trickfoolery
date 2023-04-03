// using System.Collections;
// using System.Collections.Generic;
// using Attacks;
// using UnityEngine;

// public class ShockwaveCreator : MonoBehaviour
// {
//     [SerializeField] public GameObject shockwavePrefab;
    
//     private void Start()
//     {
        
//     }

//     void Update()
//     {
//         if (BasicShockwaveAttack.instantiateShockwave)
//         {

//             Debug.Log("Shockwave create 1");
//             createShockwave();
//             Debug.Log("Shockwave create 2");

//             // Debug.Log("Shockwave create 1");
//             createShockwave();
//             // Debug.Log("Shockwave create 2");

//             BasicShockwaveAttack.instantiateShockwave = false;
//         }
//     }

//     private void createShockwave()
//     {
//         GameObject node = Instantiate(shockwavePrefab, transform);
//         node.transform.position = transform.position;
//     }
    
// }
