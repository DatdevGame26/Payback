using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Loại vật phẩm
public enum ItemType
{
    Health,
    Grenade,
    Ammo,
    Fuel
}

public class Item : MonoBehaviour
{
    [SerializeField] ItemType itemType;     //  Loại vật phẩn
    [SerializeField] int amount;        //  Số lượng
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

        //  Nếu người chơi đến đủ gần, vật phẩm sẽ chuyển collider từ Non-Trigger (tương tác với vật lý) sang Trigger để đi xuyên vật thể đến với người chơi
        if (Vector3.Distance(transform.position, playerT.position) <= 10)
        {
            Vector3 followDir = (playerT.position - transform.position).normalized;
            rb.velocity = 25 * followDir;
            col.isTrigger = true;
        }
        else col.isTrigger = false;
    }

    //   Nếu va phải người chơi, dựa vào loại vật phẩm để tăng đúng số lượng cần thiết, sau đó tự xoá nó khỏi game
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

            //  Chạy âm thanh bên phía người chơi khi thu thập vật phẩm
            player.playPickUpItemSound();
            Destroy(gameObject);
        }
    }

}