using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewPerk", menuName = "Perk System/Perk")]
public class PerkSodasSO : ScriptableObject
{
    //holds a perk with a potential list of modifiers;
    public Sprite icon;
    public string perkName;
    public PerkID perkID;
    public int price;
    public string description;

    public List<PerkSodaStatModifier> statModifiers;

}
