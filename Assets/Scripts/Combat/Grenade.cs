using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//   Người chơi và Boss dùng Grenade
public class Grenade : MonoBehaviour
{
    [SerializeField] HostileTag hostileTag;
    [SerializeField] GameObject explosion;  //  Vụ nổ được tạo
    [SerializeField] float explodeCountdown;    //  Hết thời gian sẽ phát nổ
    [SerializeField] bool inCountDownRange;     //  Thời gian phát nổ sẽ bị lệch

    float timer;
    void Start()
    {
        if (inCountDownRange)
        {
            explodeCountdown = Random.Range(explodeCountdown - 2f, explodeCountdown + 2f);
        }
        timer = explodeCountdown;
        Destroy(gameObject, 15);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == hostileTag.ToString())
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Destroy(gameObject);
    }

}
