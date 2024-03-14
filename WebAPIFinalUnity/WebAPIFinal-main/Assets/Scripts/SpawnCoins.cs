using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    public GameObject coinPrefab;
    public int maxCoins;
    public float zMax = 23;
    public float zMin = -6;
    public float xMax = 15;
    public float xMin = -15;
    public int numCoins;
    public List<GameObject> coinList = new List<GameObject>();

    public bool endGame = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < maxCoins; i++)
        {
            coinList.Add(Instantiate(coinPrefab, new Vector3(Random.Range(xMin, xMax), 0, Random.Range(zMin, zMax)), Quaternion.identity));
           
        }
       numCoins = coinList.Count;
    }

    // Update is called once per frame
    void Update()
    {
        int j = 0;
        for(int i = 0; i < maxCoins; i++)
        {
            if (coinList[i] != null)
            {
                j++;
            }
        }
        numCoins = j;

        if(numCoins <= 0)
        {
            endGame = true;
        }
    }
}
