using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFPS : MonoBehaviour
{

    [SerializeField] float offsetY;
    [SerializeField] float crouchY;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float recoilRecoverySpeed = 5f;
    [SerializeField] Crosshair crosshair;

    Transform playerT;
    Camera cam;
    Player player;

    float xRotation = 0f;

    private float currentRecoilX = 0f;
    private float currentRecoilY = 0f;
    private float lerpSpeed = 5f;

    Vector3 targetPosition, normalPosition, crouchPosition;
    float startFOV;


    void Start()
    {
        cam = GetComponent<Camera>();
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerT != null)
        {
            crouchPosition = new Vector3(0, crouchY, 0);
            normalPosition = new Vector3(0, offsetY, 0);
            player = playerT.GetComponent<Player>();
        }
        startFOV = cam.fieldOfView;
    }

    void Update()
    {
        if (player.controllerIsStop()) return;

        handleCrouchingPOV();

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        float totalXRotation = xRotation + currentRecoilX;
        float totalYRotation = currentRecoilY;

        transform.localRotation = Quaternion.Euler(totalXRotation, totalYRotation, 0f);

        playerT.Rotate(Vector3.up * mouseX);

        currentRecoilX = Mathf.Lerp(currentRecoilX, 0f, recoilRecoverySpeed * Time.deltaTime);
        currentRecoilY = Mathf.Lerp(currentRecoilY, 0f, recoilRecoverySpeed * Time.deltaTime);
    }

    private void handleCrouchingPOV()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            targetPosition = crouchPosition;
        }
        else targetPosition = normalPosition;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * lerpSpeed);
    }

    public void toggleScope(bool useScope)
    {
        cam.fieldOfView = useScope ? 15 : startFOV;
        crosshair.toggleSniperCrosshair(useScope);
    }

    public void applyRecoil(float recoil)
    {
        currentRecoilX -= recoil;
        float randomRecoilY = Random.Range(-recoil, recoil) * 0.5f;
        currentRecoilY += randomRecoilY;
    }

}
