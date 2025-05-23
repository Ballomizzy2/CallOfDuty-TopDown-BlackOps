using UnityEngine;
/// <summary>
/// a map room prefab cannot hold a door, so doors have to exist where all maps are present?
/// feed this door script the two room it connects and it will wake them up open purchase and add their spawners to SpawnManager
/// does this mean this needs to be refreneced in the door purchase script's interact?...?
/// </summary>
public class DoorController : MonoBehaviour
{

    [SerializeField] private RoomSpawnerData mapA;
    [SerializeField] private RoomSpawnerData mapB;
    //reference the spawnmanager

    public void SendDataToSpawnManager()
    {
        //if map has not been "woken up" before
        if (!mapA.enabled)
        {
            mapA.enabled = true;
            SpawnManager.Instance.AddBarricadeMapData(mapA.BarricadesInRoom());
            SpawnManager.Instance.AddZombieMapData(mapA.ZombieSpawnersInRoom());
            //mysterybox
            //send this to spawn manager
        }
        else if (!mapB.enabled)
        {
            mapB.enabled = true;
            SpawnManager.Instance.AddBarricadeMapData(mapB.BarricadesInRoom());
            SpawnManager.Instance.AddZombieMapData(mapB.ZombieSpawnersInRoom());
            //send this to 
        }
    }
}
