using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{

    //Player/Abiliies/Weapon Stats
    //public List<PlayerAttributes> PlayerAttributes = new List<PlayerAttributes>
    //public List<AbilityAttributes> PlayerAttributes = new List<PlayerAttributes>
    //public List<WeaponSavaData> WeaponAttributesDatas = new List<WeaponAttributes>();
    public List<WeaponAttributes> WeaponAttributesDatas = new List<WeaponAttributes>();

    //Game Progress
    //list of unlocked levels
    //list of completed missions
    //last level loaded before game app was exited

    //General Game Stats
    public int TotalWins;
    public int TotalLosses;

    public int TotalEnemyKills;
    public int TotalDeaths;
    public int TotalSuicides;
    public int TotalGamesPlayed;

    public int TotalSilver;

    public float TotalGameTime; 
}


//instead of saving weapon attributes, i'll save this instead to decrease the 
//the memory size of each weapon
public class WeaponSaveData
{
    [HideInInspector]
    public string weaponName;
    [HideInInspector]
    public int fireRateLevel = 0;
    [HideInInspector]
    public int WeaponDamageLevel = 0;
    [HideInInspector]
    public int AmmoCapacityLevel = 0;
    [HideInInspector]
    public bool SecondaryFire = false;
}
