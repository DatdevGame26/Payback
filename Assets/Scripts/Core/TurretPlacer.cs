using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    [SerializeField] int initFuel;
    [SerializeField] GameObject[] allTempTurretPrefabs;
    [SerializeField] float placementDistance = 2f; // Khoảng cách sinh mô hình
    [SerializeField] LayerMask placementLayer; // LayerMask để đặt trụ lên

    GameObject currentTempTurretObject;
    TempTurret currentTempTurret;
    bool canPlaceTurret;
    int totalFuel;

    private void Start()
    {
        totalFuel = initFuel;
    }

    void Update()
    {
        chooseTurretWithNumber();
        interactWithTempTurret();
    }

    private void chooseTurretWithNumber()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawnChosenTurret(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spawnChosenTurret(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spawnChosenTurret(2);
        }
    }
    private void interactWithTempTurret()
    {
        if (currentTempTurretObject != null)
        {
            checkIfCanPlaceTurret();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(currentTempTurretObject);
                return;
            }

            moveTurret();

            if (Input.GetKeyDown(KeyCode.E))
            {
                placeTurret();
                return;
            }
            currentTempTurret.updateTurretMaterial(canPlaceTurret);
        }
    }

    void spawnChosenTurret(int turretIndex)
    {
        bool hasEnoughFuel = allTempTurretPrefabs[turretIndex].
            GetComponent<TempTurret>().getFuelNeed() <= totalFuel;
        if (!hasEnoughFuel) return;

        if (currentTempTurretObject == null)
        {
            currentTempTurretObject = Instantiate(allTempTurretPrefabs[turretIndex]);
            currentTempTurret = currentTempTurretObject.GetComponent<TempTurret>();
        }
    }

    void moveTurret()
    {
        RaycastHit hit;
        Vector3 direction = Camera.main.transform.forward;
        Vector3 origin = Camera.main.transform.position;

        // Raycast chỉ với layer đặt trụ
        if (Physics.Raycast(origin, direction, out hit, placementDistance, placementLayer))
        {
            currentTempTurretObject.transform.position = hit.point; // Đặt vị trí ở điểm va chạm
        }
        else
        {
            // Nếu không va chạm, đặt ở vị trí mặc định
            currentTempTurretObject.transform.position = origin + direction * placementDistance;
        }
    }

    void placeTurret()
    {
        if (canPlaceTurret)
        {
            currentTempTurret.startBuilding();
            totalFuel -= currentTempTurret.getFuelNeed();
            currentTempTurretObject = null;
        }

    }

    void checkIfCanPlaceTurret()
    {
        RaycastHit hit;
        Vector3 direction = Camera.main.transform.forward;
        Vector3 origin = Camera.main.transform.position;
        if (!Physics.Raycast(origin, direction, out hit, placementDistance, placementLayer))
        {
            canPlaceTurret = false;
            return;
        }

        Collider[] hitColliders = Physics.OverlapBox(currentTempTurretObject.transform.position,
        currentTempTurretObject.GetComponent<Collider>().bounds.extents, currentTempTurretObject.transform.rotation);
        foreach (Collider col in hitColliders)
        {
            // Bỏ qua chính currentTempTurret và các vật thuộc layer đặt trụ
            int colLayerMask = 1 << col.gameObject.layer;
            if (col.gameObject != currentTempTurretObject
                && (placementLayer & colLayerMask) == 0)
            {
                canPlaceTurret = false;
                return;
            }
        }
        canPlaceTurret = true;

    }

    public string getTotalFuel()
    {
        return totalFuel.ToString();
    }
}
