using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : Room   // subclass of Room
{
// -------------------------- Private variables --------------------------
    // room size: 8 (y) * 16 (x)
    // 1: coin, 2: weed
    private int[,] layout = new int[20, 20];
    private const int coinNumber = 10, trapNumber = 10;
    private List<SpikeWeed> spikeWeeds = new List<SpikeWeed>();
    private List<Coin> coins = new List<Coin>();

// -------------------------- Start --------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Initialize layout to 0
        for(int i = 0; i < 8; i ++)
            for(int j = 0; j < 16; j ++)
                layout[i, j] = 0;
    }

// -------------------------- Update --------------------------
    // Update is called once per frame
    void Update()
    {
        
    }

// -------------------------- Other functions --------------------------
    // IMPORTANT!!
    // Implementation of GenerateLayout
    public override void GenerateLayout(GameObject coin, GameObject spikeWeed)
    {
        int x, y;

        // coin
        /* for(int i = 0; i < coinNumber; i ++)
        {
            // Debug.Log(i);
            do{
                x = UnityEngine.Random.Range(0, xRange);
                y = UnityEngine.Random.Range(0, yRange);
            } while(layout[x, y] > 0);
            coins.Add(Instantiate(coin, this.position + new Vector3(x, y, 0), Quaternion.identity).GetComponent<Coin>());
            layout[x, y] = 1;
            // if(i == 0)
        }
        */

        // weed
        for(int i = 0; i < coinNumber; i ++)
        {
            // Randomly get position (may update)
            do{
                x = UnityEngine.Random.Range(0, xRange);
                y = UnityEngine.Random.Range(0, yRange);
            } while(layout[x, y] > 0);

            // Generate new weed
            spikeWeeds.Add(Instantiate(spikeWeed, this.position + new Vector3(x, -y, 0), Quaternion.identity).GetComponent<SpikeWeed>());

            // Set layout
            layout[x, y] = 2;
        }
    }
}
