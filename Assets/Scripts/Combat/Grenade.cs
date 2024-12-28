using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] HostileTag hostileTag;
    [SerializeField] GameObject explosion;
    [SerializeField] float explodeCountdown;
    [SerializeField] bool inCountDownRange;

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
