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
    [Header("Shoot Info")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject grenade;
    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [Range(0f, 1f)][SerializeField] float accuracy;
    [SerializeField] float recoil;
    [Header("Ammo Info")]
    [SerializeField] int initialAmmo = 90;
    [SerializeField] int initialGrenade = 10;
    [SerializeField] int ammoCapacity;

    int currentAmmo;
    int currentGrenade;
    int totalAmmo;

    float timer;
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
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (currentAmmo <= 0) Reload();
        handleCrouch();
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashlighOn = !isFlashlighOn;
            flashLight.SetActive(isFlashlighOn);
        }
    }

    private void handleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            accuracy = 1;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            accuracy = baseAccuracy;
        }
    }

    public void shootBullet()
    {
        if (timer <= 0 && !isReloading && currentAmmo > 0)
        {
            float spreadAngle = (float)((1 - accuracy) * 10);
            Vector3 shootDir = getRandomShotDir(spreadAngle * 0.01f);

            Rigidbody bulletRB = Instantiate(bullet, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            if (bulletRB != null)
            {
                bulletRB.transform.forward = shootDir;
                bulletRB.velocity = shootDir * bulletSpeed;
            }
            camFPS.applyRecoil(recoil);
            timer = fireRate;
            currentAmmo--;
            animator.Play("player_gun_shoot", -1, 0);
        }
    }

    public void shootGrenade()
    {
        if (timer <= 0 && !isReloading && currentGrenade > 0)
        {
            Rigidbody grenadeRB = Instantiate(grenade, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            if (grenadeRB != null)
            {
                // Thiết lập vận tốc cho lựu đạn
                grenadeRB.velocity = firePoint.forward * bulletSpeed / 4;

                // Áp dụng lực quay
                Vector3 torque = new Vector3(10, 10, 0); // Thay đổi giá trị này để điều chỉnh độ mạnh của lực quay
                grenadeRB.AddTorque(torque, ForceMode.Impulse);
            }

            camFPS.applyRecoil(recoil * 2);
            timer = fireRate * 3;
            currentGrenade--;
            animator.Play("player_gun_shoot", -1, 0);
        }
    }


    public void Reload()
    {
        if (isReloading || currentAmmo == ammoCapacity || totalAmmo <= 0) return;
        isReloading = true;
        animator.SetTrigger("Reload");
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

    Vector3 getRandomShotDir(float spreadAmount)
    {
        float randomX = Random.Range(-spreadAmount, spreadAmount);
        float randomY = Random.Range(-spreadAmount, spreadAmount);
        float randomZ = Random.Range(-spreadAmount, spreadAmount);

        Vector3 shootDir = firePoint.transform.forward + new Vector3(randomX, randomY, randomZ);
        shootDir.Normalize();
        return shootDir;
    }

    public float getAccuracy()
    {
        return accuracy;
    }

    public string getAmmoSlashTotal()
    {
        return $"{currentAmmo} / {totalAmmo}";
    }

    public string getGrenadeLeft()
    {
        return currentGrenade.ToString() ;
    }
}
