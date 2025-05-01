using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public List<GameObject> powerUpPrefabs;
    public GameObject playerPrefab;
    
    public static PowerUpManager Instance;
    internal bool instaKillActive = false;

    private void Awake() {
        //Debug.Log("[PowerUpManager] Awake called from: " + gameObject.name);
        if (Instance == null) 
        {
            Instance = this;
        }
    }

    public void TryDropPowerUp(Vector3 position)
    {
        //CHANGE THIS AFTER TESTING TO 0.03f
        float dropChance = 0.03f;
        float randomNumber = Random.value;
        //Debug.Log($"[PowerUpManager] Will try to spawn {powerUpPrefabs.Count} prefabs");
        
        if (randomNumber <= dropChance)
        {
            //Debug.Log($"[PowerUpManager] List count at runtime: {powerUpPrefabs.Count}");
            int index = Random.Range(0, powerUpPrefabs.Count);
            if (index >= 0 && index < powerUpPrefabs.Count) 
            {
                GameObject powerup = Instantiate(powerUpPrefabs[index], position + new Vector3(0, 1f, 0), Quaternion.identity);
                PowerUp powerupScript = powerup.GetComponent<PowerUp>();
                powerupScript.playerPrefab = playerPrefab;
            } 
            else 
            {
                Debug.LogError($"[PowerUpManager] Index {index} out of bounds!");
            }
        }
    }

    internal void instaKillEffect()
    {
        StartCoroutine(InstaKillRoutine(30));
    }
    
    private IEnumerator InstaKillRoutine(float duration)
    {
        instaKillActive = true;
        Debug.Log("InstaKill Active");
        
        yield return new WaitForSeconds(duration);

        instaKillActive = false;
        Debug.Log("InstaKill Ended");
    }
}
