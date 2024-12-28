using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   //  Có thể chỉnh được ở Inspector
//  Lớp DropItem sẽ tham chiếu đến Prefab Item và tỉ lệ rơi ra nó
class DropItem
{
    public GameObject item;
    public float dropPercent;
}

//  Gán vào kẻ thù muốn rơi ra vật phẩm
public class ItemSpawner : MonoBehaviour
{
    /*  Index của Vật phẩm 
     *      0: Health 
     *      1: Ammo
     *      2: Fuel
     *      3: Grenade
     *      4: Fuel (Bigger)
     */

    [SerializeField] int dropItemNumber;    //  Số lượng vật phẩm có thể rơi ra
    [SerializeField] float minDropForce;    //  Lực rơi vật phẩm ra tối thiểu
    [SerializeField] float maxDropForce;    //  Lực rơi vật phẩm cực đại
    [SerializeField] DropItem[] allDropItems;   //  Toàn bộ Item có thể rơi ra
    Vector3 offsetSpawn;

    public void Spawn()
    {
        float percent;

        //  Duyệt số lượng rơi ra item
        for (int i = 0; i < dropItemNumber; i++)
        {
            percent = Random.value;     //  Tạo giá trị phần trăm ngẫu nhiên
            int randItemIndex = Random.Range(0, allDropItems.Length);       //  Chọn ngẫu nhiên Vật phẩm trong danh sách allDropItems

            //  Nếu phần trăm được tạo mà nhỏ hơn phần trăm rơi của Vật phẩm được chọn ngẫu nhiên, Sinh ra item đó
            if (percent <= allDropItems[randItemIndex].dropPercent)
            {
                //  Thêm lực rơi cho Vật phẩm
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
