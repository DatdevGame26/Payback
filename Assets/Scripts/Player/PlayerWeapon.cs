using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] Animator animator;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform convergencePoint;
    [SerializeField] CameraFPS camFPS;
    [SerializeField] GameObject flashLight;
    [Header("Gun Info")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject grenade;
    [SerializeField] float fireRate;
    [SerializeField] float projectileSpeed;
    [Range(0f, 1f)][SerializeField] float accuracy;
    [SerializeField] float recoil;
    [Header("Ammo Info")]
    [SerializeField] int initialAmmo = 90;
    [SerializeField] int initialGrenade = 10;
    [SerializeField] int ammoCapacity;
    [Header("Gun SFX")]
    [SerializeField] AudioClip shootBulletSFX;
    [SerializeField] AudioClip shootGrenadeSFX;

    int currentAmmo;
    int currentGrenade;
    int totalAmmo;
    int bulletDamageLevel;

    float timer;
    float stuckReloadTimer;
    float baseAccuracy;

    bool isReloading;
    bool isFlashlighOn;


    void Start()
    {
        firePoint.LookAt(convergencePoint);

        baseAccuracy = accuracy;
        totalAmmo = initialAmmo;
        currentAmmo = ammoCapacity;
        currentGrenade = initialGrenade;

        bulletDamageLevel = 0;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (currentAmmo <= 0) Reload();

        if (Input.GetKeyDown(KeyCode.LeftControl))  accuracy = 1; 
        else if (Input.GetKeyUp(KeyCode.LeftControl)) accuracy = baseAccuracy;

        if (isReloading)
        {
            stuckReloadTimer += Time.deltaTime;
            if(stuckReloadTimer >= 3)
            {
                isReloading = false;
                stuckReloadTimer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashlighOn = !isFlashlighOn;
            flashLight.SetActive(isFlashlighOn);
        }
    }

    public void shootBullet()
    {
        if (timer <= 0 && !isReloading && currentAmmo > 0)
        {
            Vector3 shootDir = getRandomShotDir();

            Rigidbody bulletRB = Instantiate(bullet, firePoint.position, Quaternion.LookRotation(shootDir)).GetComponent<Rigidbody>();
            if (bulletRB != null)
            {
                bulletRB.velocity = shootDir * projectileSpeed;
            }
            camFPS.applyRecoil(recoil);
            timer = fireRate;
            currentAmmo--;
            animator.Play("player_gun_shoot", -1, 0);

            AudioManager.Instance.PlaySound("player_shoot_bullet", gameObject);
        }
    }

    public void shootGrenade()
    {
        if (timer <= 0 && !isReloading && currentGrenade > 0)
        {
            Rigidbody grenadeRB = Instantiate(grenade, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            if (grenadeRB != null)
            {
                grenadeRB.velocity = getRandomShotDir() * projectileSpeed / 3.5f;
                Vector3 torque = new Vector3(10, 10, 0);
                grenadeRB.AddTorque(torque, ForceMode.Impulse);
            }

            camFPS.applyRecoil(recoil * 2);
            timer = fireRate * 3;
            currentGrenade--;
            animator.Play("player_gun_shoot", -1, 0);
            AudioManager.Instance.PlaySound("player_shoot_grenade", gameObject);
        }
    }

    public void Reload()
    {
        if (isReloading || currentAmmo == ammoCapacity || totalAmmo <= 0) return;
        AudioManager.Instance.PlaySound("player_reload", gameObject);
        isReloading = true;
        animator.SetTrigger("Reload");
        stuckReloadTimer = 0;
    }

    public void finishReload()
    {
        if (totalAmmo > 0)
        {
            int ammoToReload = ammoCapacity - currentAmmo;
            if (totalAmmo < ammoToReload)
            {
                ammoToReload = totalAmmo;
            }

            currentAmmo += ammoToReload;
            totalAmmo -= ammoToReload;
        }
        isReloading = false;
    }

    Vector3 getRandomShotDir()
    {
        float spreadAngle = (float)((1 - accuracy) * 10);
        spreadAngle *= 0.01f;

        float randomX = Random.Range(-spreadAngle, spreadAngle);
        float randomY = Random.Range(-spreadAngle, spreadAngle);
        float randomZ = Random.Range(-spreadAngle, spreadAngle);

        Vector3 shootDir = firePoint.transform.forward + new Vector3(randomX, randomY, randomZ);
        shootDir.Normalize();
        return shootDir;
    }

    #region Get Funcs
    public float getAccuracy() { return accuracy; }
    public string getAmmoSlashTotal() { return $"{currentAmmo} / {totalAmmo}"; }
    public string getGrenadeLeft() { return currentGrenade.ToString(); }
    #endregion

    #region Plus Bullet Funcs
    public void plusAmmo(int amount) { totalAmmo += amount; }
    public void plusGrenade(int amount) { currentGrenade += amount; }
    #endregion

    #region Upgrade
    public void increaseFireRate() { fireRate -= 0.03f; }
    public void increaseProjectileSpeed() { projectileSpeed += 33; }
    public void increaseAmmoCapacity() { ammoCapacity += 10; }
    public void increaseBulletDamage()
    {
        bulletDamageLevel++;
        bullet = Resources.Load<GameObject>($"Player Bullet/Bullet_Lvl_{bulletDamageLevel}");
    }
    #endregion
}
