using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public List<GameObject> powerUpPrefabs;
    public GameObject playerPrefab;
    
    public static PowerUpManager Instance;
    internal bool instaKillActive = false;
    internal float instaKillMaxtimer;
    internal float instaKillTimer;

    [Header("FireSale")]
    internal bool fireSaleActive = false;
    internal float fireSaleMaxtimer;
    internal float fireSaleTimer;


    [Header("Debug")]
    [SerializeField] private bool isGuarantee = false;

    private void Awake() {
        //Debug.Log("[PowerUpManager] Awake called from: " + gameObject.name);
        if (Instance == null) 
        {
            Instance = this;
        }
    }

    public void TryDropPowerUp(Vector3 position)
    {
        
        float dropChance;
        if (!isGuarantee)
        {
            //CHANGE THIS AFTER TESTING TO 0.03f
            dropChance = 0.03f;
        }
        else
        {
            dropChance = 1f;
        }
        
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
    //instakill
    internal void instaKillEffect()
    {
        StartCoroutine(InstaKillRoutine(30));
    }
    
    private IEnumerator InstaKillRoutine(float duration)
    {
        instaKillActive = true;
        instaKillMaxtimer=duration;
        instaKillTimer=duration;
        HUDController.Instance.EnablePowerUpUI(PowerUpType.InstaKill);
        Debug.Log("InstaKill Active");

        while (instaKillTimer > 0f)
        {
            instaKillTimer -= Time.deltaTime;
            yield return null;
        }

        instaKillTimer = 0f;
        instaKillActive = false;
        HUDController.Instance.DisablePowerUpUI(PowerUpType.InstaKill);
        Debug.Log("InstaKill Ended");
    }
    public float GetInstaKillTimerNormalized()
    {
        return instaKillMaxtimer > 0 ? instaKillTimer / instaKillMaxtimer : 0f;
    }
    //fire sale
    internal void FireSaleEffect()
    {
        StartCoroutine(FireSaleRoutine(15));
    }
    private IEnumerator FireSaleRoutine(float duration)
    {
        fireSaleActive = true;
        fireSaleMaxtimer = duration;
        fireSaleTimer = duration;
        HUDController.Instance.EnablePowerUpUI(PowerUpType.FireSale);
        MysteryBoxLocation.Instance.ActivateFireSale();
        while (fireSaleTimer > 0f)
        {
            fireSaleTimer -= Time.deltaTime;
            Debug.Log(fireSaleTimer);
            yield return null;
        }
        fireSaleTimer = 0;
        fireSaleActive = false;
        HUDController.Instance.DisablePowerUpUI(PowerUpType.FireSale);
        MysteryBoxLocation.Instance.EndFireSale();

    }
    public float GetFireSaleTimerNormalized()
    {
        return fireSaleMaxtimer > 0 ? fireSaleTimer / fireSaleMaxtimer : 0f;
    }
}
