using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    SpawnCoins coins;
    public int points;
    public GameObject gameManager;
    public GameObject jsonSerial;
    void Movement()
    {
        if(isLocalPlayer)
        {
            if(transform.position.x < -17) 
            {
                transform.position = new Vector3(transform.position.x + 1, 0, transform.position.z);
            }
            else if(transform.position.z < -15)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z + 1);
            }
            else if(transform.position.x > 17)
            {
                transform.position = new Vector3(transform.position.x - 1, 0, transform.position.z);
            }
            else if (transform.position.z > 25)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z - 1);
            }
            else
            {
                float moveH = Input.GetAxis("Horizontal");
                float moveV = Input.GetAxis("Vertical");
                Vector3 move = new Vector3(moveH / 5, 0, moveV / 5);
                transform.position = transform.position + move;
            }
        }
    }

    private void Update()
    {
        Movement();

        gameManager = GameObject.Find("GameManager");
        Debug.Log(gameManager.GetComponent<SpawnCoins>().endGame);
        if (gameManager.GetComponent<SpawnCoins>().endGame)
            {
            
            jsonSerial = GameObject.Find("JsonStuff");
            jsonSerial.GetComponent<JsonSerializer>().currentlySelectedEntry.score += points;
            jsonSerial.GetComponent<JsonSerializer>().currentlySelectedEntry.wins++;

            if (jsonSerial.GetComponent<JsonSerializer>().entryExists)
            {
                jsonSerial.GetComponent<JsonSerializer>().UpdateEntryButton();
            }
            else
            {
                jsonSerial.GetComponent<JsonSerializer>().SendButton();
            }
        }
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isLocalPlayer)
        {
            if(collision.gameObject.tag == "coin")
            {
                //gameManager.GetComponent<SpawnCoins>().numCoins--;
                Destroy(collision.gameObject);
                points++;
               // Debug.Log("Hello");
            }
        }
    }
}
