using UnityEngine;
using System.Collections.Generic;
/// <summary>
///             ----This should handle the number of zombies on map and track them and the current round----
/// round- determines how many zombies n their strength (when reserve AND currentZombieOnMap==0, ++round)
/// totalZombies- track total zombies for round, do math based on round to determine
/// reserveZombies- IF current LESS THAN maxZombiesOnMap, spawn more zombies until ==maxZombiesOnMap (decrement reserveZombies in process)
/// currentZombies- number of zombies on the map
/// maxZombiesOnMap- caps the number of zombies on map (24)
/// 
///              ----BACAUSE THIS ALSO TRACKS BARRICADES, carpenter should talk to this script?...----
/// should talk to spawners/access spawners
/// 
///             ----So far, all instances of this script is disabled?----
/// </summary>
public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnWave
    {
        public GameObject enemyType;
        public int count;
    }

    [Header("Wave settings")]
    public List<SpawnWave> waves = new List<SpawnWave>();

    [Header("References")]
    public Transform player;
    public float heightOffset = 0.25f;

    [Header("Map Data")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<BarricadeController> barricadeList;

    [Header("Debugging stuff")]
    [SerializeField] private bool isUsingOriginalUse = true;


    #region ──  Initialisation ────────────────────────────────────────────────────
    private void Awake()
    {

        GameObject[] objs = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints = new Transform[objs.Length];
        for (int i = 0; i < objs.Length; i++)
            spawnPoints[i] = objs[i].transform;

        if (spawnPoints.Length == 0)
            Debug.LogWarning("[SpawnManager] No objects tagged “SpawnPoint” found in the scene.");
    }
    #endregion

    #region ──  Public API ───────────────────────────────────────────────────────
    public void SpawningWave()
    {
        if (spawnPoints.Length == 0) return;   // safe-guard

        foreach (SpawnWave wave in waves)
        {
            for (int i = 0; i < wave.count; i++)
            {
                Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // If the SpawnPoint is on the NavMesh this is usually enough,
                // otherwise you can still sample the NavMesh around “point.position”.
                Vector3 spawnPos = point.position + Vector3.up * heightOffset;

                GameObject enemy = Instantiate(
                    wave.enemyType,
                    spawnPos,
                    point.rotation);          // inherit any desired facing

                enemy.GetComponent<EnemyController>().target = player;
            }
        }
    }
    #endregion
    public List<BarricadeController> GetBars()
    {
        return barricadeList;
    }
    
}
