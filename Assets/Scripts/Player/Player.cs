using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : LiveEntity
{
    [SerializeField] float baseSpeed = 12f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int initFuel;
    [SerializeField] PlayerWeapon playerWeapon;
    [SerializeField] CameraFPS camFPS;
    [SerializeField] Image hurtIndicator;
    [SerializeField] FillBar healthBar;
    [Header("Toggle Map")]
    [SerializeField] GameObject miniMap;
    [SerializeField] GameObject wholeMap;
    int currentFuel;

    Rigidbody rb;
    bool isGrounded;
    bool isControllerStop;
    float realSpeed;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        currentFuel = initFuel;
        isControllerStop = false;
        realSpeed = baseSpeed;
        PlayerPrefs.SetString("Game Result", "");
    }

    protected override void Update()
    {
        base.Update();
        healthBar.updateFillBar(currentHealth, maxHealth);

        ToggleMap();

        if (isControllerStop) return;

        Movement();
        InputHandleler();
        IndicateHurt();
    }

    private void ToggleMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            wholeMap.SetActive(true);
            miniMap.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            wholeMap.SetActive(false);
            miniMap.SetActive(true);
        }
    }

    public void plusHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth) currentHealth = maxHealth;
    }

    public override void Damage(int damage)
    {
        if (PlayerPrefs.GetString("Game Over") == "Win") return;
        base.Damage(damage);
        if (isDead)
        {
            PlayerPrefs.SetString("Game Result", "Lose");
            SceneManager.LoadScene("Game Over");
            return;
        }

        camFPS.applyRecoil(1);
        hurtIndicator.color = new Color(1, 1, 1, 0.08f * damage / 5);
    }

    void IndicateHurt()
    {
        hurtIndicator.color = Color.Lerp(hurtIndicator.color, new Color(1, 1, 1, 0), 5 * Time.deltaTime);
    }

    public void playPickUpItemSound()
    {
        AudioManager.Instance.PlaySound("item_pick_up", gameObject, false);
    }

    #region Controller
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

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

    public void stopController(bool check)
    {
        isControllerStop = check;
    }
    public bool controllerIsStop() { return isControllerStop; }
    #endregion

    #region Collision Functions
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
    #endregion

    #region Fuel Functions
    public void plusFuel(int amount) { currentFuel += amount; }
    public void minusFuel(int amount)
    {
        currentFuel -= amount;
        if(currentFuel <= 0) currentFuel = 0;
    }
    public int getCurrentFuel() { return currentFuel; }
    #endregion


}
