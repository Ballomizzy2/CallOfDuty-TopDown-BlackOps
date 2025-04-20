using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewPerk", menuName = "Perk System/Perk")]
public class PerkSodasSO : ScriptableObject
{
    //holds a perk with a potential list of modifiers;
    public string perkName;
    public int price;
    public string description;

    public List<StatModifier> statModifiers;

}
