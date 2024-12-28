using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int amount;
    Transform playerT;
    Rigidbody rb;
    Collider col;

    private void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null) playerT = playerGO.transform;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        Destroy(gameObject, 60);
    }

    private void Update()
    {
        if (playerT == null) return;

        if (Vector3.Distance(transform.position, playerT.position) <= 10)
        {
            Vector3 followDir = (playerT.position - transform.position).normalized;
            rb.velocity = 25 * followDir;
            col.isTrigger = true;
        }
        else col.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            switch (itemType)
            {
                case ItemType.Health:
                    {
                        if (player != null) player.plusHealth(amount);
                        break;
                    }
                case ItemType.Fuel:
                    {
                        if (player != null) player.plusFuel(amount);
                        break;
                    }
                case ItemType.Ammo:
                    {
                        PlayerWeapon weapon = other.gameObject.GetComponent<PlayerWeapon>();
                        if (weapon != null) weapon.plusAmmo(amount);
                        break;
                    }
                case ItemType.Grenade:
                    {
                        PlayerWeapon weapon = other.gameObject.GetComponent<PlayerWeapon>();
                        if (weapon != null) weapon.plusGrenade(amount);
                        break;
                    }
            }
            player.playPickUpItemSound();
            Destroy(gameObject);
        }
    }

    public enum ItemType
    {
        Health,
        Grenade,
        Ammo,
        Fuel
    }
}