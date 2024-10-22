using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTurret : MonoBehaviour
{
    [SerializeField] GameObject spawnTurret;
    [SerializeField] int fuel;
    [SerializeField] float buildingTime;
    [SerializeField] GameObject inBuildingProcessFX;
    [SerializeField] GameObject finishBuildingFX;
    [Header("Material")]
    [SerializeField] Material canBePlacedMaterial;
    [SerializeField] Material cannotBePlacedMaterial;
    [SerializeField] MeshRenderer[] allTurretMeshRenders;

    void Start()
    {
    }

    public void startBuilding()
    {
        inBuildingProcessFX.SetActive(true);
        StartCoroutine(CountDownBuilding());
    }

    public void updateTurretMaterial(bool canBePlaced)
    {
        Material chosenMaterial = canBePlaced ? canBePlacedMaterial : cannotBePlacedMaterial;

        foreach (MeshRenderer meshRenderer in allTurretMeshRenders)
        {
            // Thay đổi tất cả materials của MeshRenderer
            Material[] materials = meshRenderer.materials; // Lấy mảng materials hiện tại

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = chosenMaterial; // Gán material mới
            }
            meshRenderer.materials = materials; // Cập nhật materials
        }

    }

    IEnumerator CountDownBuilding()
    {
        yield return new WaitForSeconds(buildingTime);
        inBuildingProcessFX.SetActive(false);
        Instantiate(spawnTurret, transform.position, Quaternion.identity);
        Instantiate(finishBuildingFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public int getFuelNeed()
    {
        return fuel;
    }
}
