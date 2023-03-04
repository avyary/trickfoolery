using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerMovement player;

    public GameObject[] hearts;

    public int numberOfHearts;
    public int[] heartChunks;

    public int health;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        numberOfHearts = hearts.Length;
        health =  Convert.ToInt32(player.MAX_HEALTH);

        heartChunks = new int[numberOfHearts];
        int chunkSize = health / numberOfHearts;
        for (int i = 0; i < numberOfHearts; i++)
        {
            heartChunks[i] = chunkSize * (i + 1); // Assign each part with the same size

            if (i < health % numberOfHearts)
            {
                // Add one to the current part if the remainder is not zero
                heartChunks[i]++;
            }
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        health = Convert.ToInt32(player.health);
        if (health < heartChunks[3])
        {
            Destroy(hearts[3].gameObject);
        }
        if (health < heartChunks[2])
            Destroy(hearts[2].gameObject);
        if (health < heartChunks[1])
            Destroy(hearts[1].gameObject);
        if (health < heartChunks[0])
            Destroy(hearts[0].gameObject);
    }
}
