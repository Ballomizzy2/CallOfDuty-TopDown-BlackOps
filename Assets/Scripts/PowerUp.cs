using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public GameObject nukeBlastPrefab;
    public GameObject playerPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);

            if (type == PowerUpType.Nuke)
            {
                Instantiate(nukeBlastPrefab, transform.position, Quaternion.identity);
                PowerUpSoundManager.Instance.PlaySoundEffect(PowerUpSoundManager.Instance.nukeClip);
                PlayerVoicelineManager.Instance.PlayVoiceline(PlayerVoicelineManager.Instance.pickupNukeClips);
            }
            else if (type == PowerUpType.MaxAmmo)
            {
                playerPrefab.GetComponentInChildren<Gun>().reserveAmmo = playerPrefab.GetComponentInChildren<Gun>().gunData.reserveAmmo;
                playerPrefab.GetComponentInChildren<Gun>().currentAmmo = playerPrefab.GetComponentInChildren<Gun>().gunData.magazineSize;
                //figure out how to fill ammo for second weapon
                PowerUpSoundManager.Instance.PlaySoundEffect(PowerUpSoundManager.Instance.maxAmmoClip);
                PlayerVoicelineManager.Instance.PlayVoiceline(PlayerVoicelineManager.Instance.pickupMaxAmmoClips);
                PlayerVoicelineManager.Instance.outOfAmmoSaid = false;

            }
            else if (type == PowerUpType.InstaKill)
            {
                PowerUpManager.Instance.instaKillEffect();
                PowerUpSoundManager.Instance.PlaySoundEffect(PowerUpSoundManager.Instance.instaKillClip);
                PlayerVoicelineManager.Instance.PlayVoiceline(PlayerVoicelineManager.Instance.pickupInstaKillClips);
            }
            else if (type == PowerUpType.DoublePoints)
            {
                GameManager_Scores.Instance.StartDoublePoints();
                PowerUpSoundManager.Instance.PlaySoundEffect(PowerUpSoundManager.Instance.doublePointsClip);
                PlayerVoicelineManager.Instance.PlayVoiceline(PlayerVoicelineManager.Instance.pickupDoublePointsClips);
            }
            else if (type == PowerUpType.Carpenter)
            {
                //List<BarricadeController> fixEm= SpawnManager.Instance.GetBars();
                //loop thru and fix and call the object's RepairAllBoards()
                //PowerUpSoundManager.Instance.PlaySoundEffect(PowerUpSoundManager.Instance.carpenterClip);
                //PlayerVoiceManager.Instance.PlayVoiceline(PlayerVoiceManager.Instance.pickupCarpenterClips);
            }
            
            Destroy(gameObject);
        }
    }
}
