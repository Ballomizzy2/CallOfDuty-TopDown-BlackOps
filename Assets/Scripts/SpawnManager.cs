using UnityEngine;
using System.Collections.Generic;
using TMPro;
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
///             ----ah man, listen to individal zombies onDeath...?----
/// </summary>
public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
   
    public class SpawnWave
    {
        public GameObject enemyType;
        public int count;
    }
    public static SpawnManager Instance { get; private set; }
    [Header("Wave settings")]
    public List<SpawnWave> waves = new List<SpawnWave>();

    [Header("References")]
    public Transform player;
    public float heightOffset = 0.25f;

    [Header("Map Data")]
    [SerializeField] private Transform[] spawnPoints;//og line

    [SerializeField] private List<SpawnInBox> spawnerList;
    [SerializeField] private List<BarricadeController> barricadeList;
    [SerializeField] private RoomSpawnerData startingRoom;

    [Header("Game Data")]
    [SerializeField] private int round=1;
    [SerializeField] private int totalZombiesInRound;
    [SerializeField] private int reserveZombies;
    [SerializeField] private int currentZombies;
    [SerializeField] private int maxZombiesOnMap = 24;
    [SerializeField] private int bossRound;
    [SerializeField] private bool roundOver = false;
    [SerializeField] private float gracePeriodTimer = 3f;

    [SerializeField] private TextMeshProUGUI roundText;


    [Header("Debugging stuff")]
    [SerializeField] private bool isUsingOriginalUse = true;


    #region ──  Initialisation ────────────────────────────────────────────────────
    private void Awake()
    {
        Instance = this;    
        if (isUsingOriginalUse)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("SpawnPoint");
            spawnPoints = new Transform[objs.Length];
            for (int i = 0; i < objs.Length; i++)
                spawnPoints[i] = objs[i].transform;

            if (spawnPoints.Length == 0)
                Debug.LogWarning("[SpawnManager] No objects tagged “SpawnPoint” found in the scene.");
        }
        else
        {
            //controlled spawning
            AddZombieMapData(startingRoom.ZombieSpawnersInRoom());
            AddBarricadeMapData(startingRoom.BarricadesInRoom());


            roundOver = true;


        }
        roundText.text = round.ToString();
        bossRound = UnityEngine.Random.Range(5, 8);
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
    private void Update()
    {
        if (roundOver &&!isUsingOriginalUse)
        {
            ReadyUp();
        }
    }
    public List<BarricadeController> GetBars()
    {
        return barricadeList;
    }

    private void TotalZombiesMath()
    {
        if (round < 9)
        {
            totalZombiesInRound = (round * 6) + 4;
        }
        else if (round > 10 &&round <35)
        {
            totalZombiesInRound = 24+6 *(round - 10);
        }
        else
        {
            totalZombiesInRound = (int)(0.15 * (Mathf.Pow(36, 2)+6));
        }
        reserveZombies = totalZombiesInRound;
        
        
    }

    private void ShuffleTheZombies()
    {
        
        if (reserveZombies > 0)
        {
            while (currentZombies < maxZombiesOnMap)
            {
                //use avalible spawners...but how?
                //bounce thu each one with a random number? x into each until current>max?  hm...
                Debug.Log($"total zombies in round: {totalZombiesInRound}, {currentZombies}/{totalZombiesInRound}");
                currentZombies++;
                reserveZombies--;
                if (reserveZombies < 0) break;
            }
        }
        else if(reserveZombies==0 && currentZombies==0)
        {
            //round is over
            GameManager_Scores.Instance.PointsPerRound();
            roundOver = true;
        }

    }

    private void ReadyUp()
    {
        gracePeriodTimer -=Time.deltaTime;
        if (gracePeriodTimer < 0)
        {
            roundText.text=round.ToString();
            if(round % bossRound == 0)
            {
                //would have a refrence to boss script to handle their unique spawn?
            }
            else
            {
                TotalZombiesMath();
                ShuffleTheZombies();
            }
            gracePeriodTimer = 3f;
            roundOver = false;
        }
    }

    public void AddZombieMapData(List<SpawnInBox> temp)
    {
        temp = startingRoom.ZombieSpawnersInRoom();
        foreach (SpawnInBox spawn in temp)
        {
            spawnerList.Add(spawn);
        }
    }
    public void AddBarricadeMapData(List<BarricadeController> barTemp)
    {
        barTemp = startingRoom.BarricadesInRoom();
        foreach (BarricadeController bar in barTemp)
        {
            barricadeList.Add(bar);
        }
    }
    public void AddMysteryBoxMapData()
    {

    }


    /* idea for listen
     * private void Enemy_OnDeath( ... )
     * {
     *      currentZombies--;
     *      ShuffleTheZombies();
     * }
     */
    
}
