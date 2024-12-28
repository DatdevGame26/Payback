using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    /*  Index của Vật phẩm (xếp theo phần trăm rơi ra giảm dần)
     *      0: Health 
     *      1: Ammo
     *      2: Fuel
     *      3: Grenade
     *      4: Fuel (Bigger)
     */
    [SerializeField] int dropItemNumber;
    [SerializeField] float minDropForce;
    [SerializeField] float maxDropForce;
    [SerializeField] DropItem[] allDropItems;
    Vector3 offsetSpawn;

    public void Spawn()
    {
        float percent;
        for (int i = 0; i < dropItemNumber; i++)
        {
            percent = Random.value;
            int randItemIndex = Random.Range(0, allDropItems.Length);
            if (percent <= allDropItems[randItemIndex].dropPercent)
            {
                Rigidbody rb = Instantiate(allDropItems[randItemIndex].item,
                    transform.position + offsetSpawn, Quaternion.identity).GetComponent<Rigidbody>();

                Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
                float dropForce = Random.Range(minDropForce, maxDropForce);

                rb.AddForce(dropDirection * dropForce);
            }
        }

    }

    public void setSpawnOffset(Vector3 offsetSpawn)
    {
        this.offsetSpawn = offsetSpawn;
    }

}

[System.Serializable]
class DropItem
{
    public GameObject item;
    public float dropPercent;
}