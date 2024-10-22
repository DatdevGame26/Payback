using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    GameObject playerObject;
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerObject = other.gameObject;
            giveItemToPlayer();
            Destroy(gameObject);
        }
    }

    protected virtual void giveItemToPlayer()
    {

    }
}
