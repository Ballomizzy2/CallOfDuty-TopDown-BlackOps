using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnWave
    {
        public GameObject enemyType;
        public int count;
    }

    public List<SpawnWave> waves;
    public float spawnRadius = 10f;
    public Transform player;

    public void SpawningWave()
    {
        foreach (var wave in waves)
        {
            for (int i = 0; i < wave.count; i++)
            {
                Vector3 spawnPos = GetRandomNavMeshPoint(spawnRadius);
                GameObject enemy = Instantiate(wave.enemyType, spawnPos, Quaternion.identity);
                enemy.GetComponent<EnemyController>().target = player;
            }
        }
    }

    private Vector3 GetRandomNavMeshPoint(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out var hit, radius, 1))
            return hit.position;
        return transform.position;
    }
}
