using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            giveItemToPlayer();
            Destroy(gameObject);
        }
    }

    protected virtual void giveItemToPlayer()
    {

    }
}
