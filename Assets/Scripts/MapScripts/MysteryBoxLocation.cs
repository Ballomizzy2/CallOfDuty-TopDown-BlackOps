using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MysteryBoxLocation : MonoBehaviour
{
    //only 1 of these scripts should exist, so Instance should be fine
    public static MysteryBoxLocation Instance {  get; private set; }
    [SerializeField]private  List<MysteryBoxDisplayHandler> mBox;
    [SerializeField]private List<RoomSpawnerData> RoomData;
    void Start()
    {
     
        PopulateOurBoxes();
        ChooseRoom();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void PopulateOurBoxes()
    {
        foreach(RoomSpawnerData roomSpawnerData in RoomData)
        {
            mBox.Add(roomSpawnerData.GetMysteryBoxObject());
        }
    }

    public void ChooseRoom()
    {
        int temp= UnityEngine.Random.Range(0, mBox.Count);
        mBox[temp].EnableBox();
    }
}
