using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
/// <summary>
/// this script holds alll the possible loactaions of the mysterybox (mysterybox can spawn behind locked rooms to encourage exploration)
/// since this knows about all locations, Fire sale methods will be made here, but handled by PowerUp Manager?
/// </summary>
public class MysteryBoxLocation : MonoBehaviour
{
    //only 1 of these scripts should exist, so Instance should be fine
    public static MysteryBoxLocation Instance {  get; private set; }
    [SerializeField]private  List<MysteryBoxDisplayHandler> mBox;
    [SerializeField]private List<RoomSpawnerData> RoomData;
    [SerializeField] private int originalBoxActive;


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
        mBox.Clear();
        foreach (RoomSpawnerData roomSpawnerData in RoomData)
        {
            mBox.Add(roomSpawnerData.GetMysteryBoxObject());
        }
    }

    public void ChooseRoom()
    {
        int temp= UnityEngine.Random.Range(0, mBox.Count);
        mBox[temp].EnableBox();
    }

    public void ActivateFireSale()
    {

        //remember the orignal box
        for (int i = 0; i < mBox.Count; i++)
        {
            if (mBox[i].BoxStatus())
            {
                originalBoxActive = i;
            }
        }
        foreach (MysteryBoxDisplayHandler handler in mBox)
        {
            //turn on all boxes
            //reduces all prices to 10
            handler.EnableBox();

            handler.GetMysteryBoxScript().SetFireSalePrice();
        }


        //disable teddy bear?(temporarily remove from weapon list?)

    }
    public void EndFireSale()
    {
        for (int i = 0; i < mBox.Count; i++)
        {
            mBox[i].GetMysteryBoxScript().EndFireSalePrice();
            //return price to 950
            if (i == originalBoxActive)
            {
                //do NOT diasable orignal
                continue;
            }
            mBox[i].DisableBox();
            //return teddy bear?(return to weapon list?)
        }

    }


}
