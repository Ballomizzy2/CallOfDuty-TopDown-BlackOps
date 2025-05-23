using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
/// <summary>
/// this script holds the zombie spawners and a mystery box spawner
/// when room "wakes up" feed the spawn manager this data via DoorController
/// </summary>
public class RoomSpawnerData : MonoBehaviour
{
    [SerializeField] private List<SpawnInBox> zombieSpawners;
    [SerializeField] private List<BarricadeController> barricadeList;
    //mystery box spawner holder
    [SerializeField] private bool isStartingRoom = false;
    private void Start()
    {
        if (isStartingRoom)
        {
            this.enabled = true;
        }
        else
        {
            this.enabled = false;
        }
            
    }


}
