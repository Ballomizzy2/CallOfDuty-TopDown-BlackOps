using UnityEngine;
using System.Collections.Generic;

public class SpawnByIndex : MonoBehaviour
{
    [Header("Prefab List")]
    public List<GameObject> prefabList = new List<GameObject>();

    [Header("Index to Spawn (real-time)")]
    public int prefabIndex = 0;

    private int lastIndex = -1; // 上一次使用的 index
    private GameObject lastSpawned; // 已生成的 prefab（用于销毁）

    void Update()
    {
        if (prefabList.Count == 0)
            return;

        // 如果 prefabIndex 改变了
        if (prefabIndex != lastIndex)
        {
            // 销毁旧的
            if (lastSpawned != null)
            {
                Destroy(lastSpawned);
            }

            // 合法索引检查
            if (prefabIndex >= 0 && prefabIndex < prefabList.Count)
            {
                lastSpawned = Instantiate(prefabList[prefabIndex], Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Prefab index out of range: " + prefabIndex);
            }

            lastIndex = prefabIndex;
        }
    }
}
