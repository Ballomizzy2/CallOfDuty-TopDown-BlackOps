using UnityEngine;

public class SpawnInBox : MonoBehaviour
{
    [Header("Spawn Settings")]
    public BoxCollider areaCollider;      // 指定生成范围的 Box Collider
    public GameObject prefabToSpawn;      // 要生成的 prefab
    public int spawnCount = 5;            // 生成的数量

    private void Awake()
    {
        areaCollider = GetComponent<BoxCollider>();
    }
    void Start()
    {
        GameObject zombieFolder = new GameObject("Zombie-Folder");
        if (areaCollider == null || prefabToSpawn == null)
        {
            Debug.LogError("Missing reference in RandomSpawnerInBox.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
          
            Vector3 spawnPos = GetRandomPointInBox(areaCollider);
            GameObject zombie =Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            zombie.transform.parent = zombieFolder.transform;
        }
    }

    // 获取 BoxCollider 内的随机位置
    Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 center = box.center + box.transform.position;
        Vector3 size = box.size * 0.5f;

        float x = Random.Range(center.x - size.x, center.x + size.x);
        float y = Random.Range(center.y - size.y, center.y + size.y);
        float z = Random.Range(center.z - size.z, center.z + size.z);

        return new Vector3(x, y, z);
    }
}
