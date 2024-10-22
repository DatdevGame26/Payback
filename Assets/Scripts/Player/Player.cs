using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LiveEntity
{
    [SerializeField] float baseSpeed = 12f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] PlayerWeapon playerWeapon;


    Rigidbody rb;
    bool isGrounded;
    float realSpeed;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        realSpeed = baseSpeed;
    }

    protected override void Update()
    {
        base.Update();
        Movement();
        InputHandleler();
    }


    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isMoving = x != 0 || z != 0;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            realSpeed = baseSpeed / 4;
        }
        else realSpeed = baseSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        rb.MovePosition(rb.position + move * realSpeed * Time.deltaTime);
    }

    void InputHandleler()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (Input.GetMouseButton(0))
        {
            playerWeapon.shootBullet();
        }else if(Input.GetMouseButton(1))
        {
            playerWeapon.shootGrenade();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerWeapon.Reload();  
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = false;
        }
    }

    public override void doSomethingThenDie()
    {
        base.doSomethingThenDie();
        Application.Quit();
    }

}
