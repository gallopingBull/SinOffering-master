using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerAttributes 
{
    [SerializeField]
    public AttributeData _data;
    private Dictionary<string, AttributeData> attributeDatabase;

    int healthAttributeLevel = 2;
    float max_health = 0;
    float health = 0;
    
    int manaAttributeLevel = 0;
    float max_mana = 0;
    float mana = 0;

    int speedAttributeLevel = 0;
    float max_speed = 0;
    float speed = 0;

    int strengthAttributeLevel = 0;
    float max_strength = 0;
    float strength = 0;

    //float airControl; // maybe...
    //int equipmentSize; // maybe...
    int evadeAttributeLevel = 0;
    bool doubleJumpedUnlocked = false;

    bool double_evade = false; 
    float evade_delay = 2.5f; 
    float evade_manaCost;

    //dashattack stuff
    int dashAttributeLevel = 0;
    float dashAttack_manaCost;
    float dashAttack_distance;
    float dashAttack_time; // timer before dash attack ecexutes automatically
    bool dashSlamUnlocked = false;
    bool dashMeleeEndUnlocked = false; // ability that gives the player an oppurtunity
                                      // to immidetely follow the dash attack with a stronger melee attack
    
    void Awake()
    {
        attributeDatabase = AttributeDatabase._instance.GetAttributeDatabase();
    }
    /*
    public List<KeyValuePair<string, AttributeData>> GetAttributeLevelData()
    {
        attributeDatabase = AttributeDatabase._instance.GetAttributeDatabase();
        var upgradeLevels = attributeDatabase;
        for (int i = 0; i < attributeDatabase.Count; i++)
        {
            upgradeLevels = attributeDatabase;
        }
        
        return upgradeLevels;
    }*/

    public int GetCurrentAttributeLevel(AttributeUpgradeTypes.UpgradeType _upgradeType)
    {
        attributeDatabase = AttributeDatabase._instance.GetAttributeDatabase();
        int tmpLvl = 0;
        //AttributeData tmpData;
        switch (_upgradeType)
        {
            case AttributeUpgradeTypes.UpgradeType.health:
                //tmpLvl = healthAttributeLevel;
                //tmpLvl = 4;
                //tmpData = GetData(_upgradeType.ToString());
                //tmpLvl = GetRandomValue(attributeDatabase[_upgradeType.ToString()].AttributeDataList.Length);
                tmpLvl = GetRandomValue(4);
                break;
            case AttributeUpgradeTypes.UpgradeType.mana:
                //tmpLvl = GetRandomValue((int)manaAttributeLevel);
                //tmpLvl = 4;
                tmpLvl = GetRandomValue(2);
                break;
            case AttributeUpgradeTypes.UpgradeType.speed:
                //tmpLvl = GetRandomValue((int)speedAttributeLevel);
                //tmpLvl = 4;
                tmpLvl = GetRandomValue(1);
                break;
            case AttributeUpgradeTypes.UpgradeType.strength:
                //tmpLvl = GetRandomValue((int)strengthAttributeLevel);
                //tmpLvl = 4; 
                tmpLvl = GetRandomValue(3);
                break;
            case AttributeUpgradeTypes.UpgradeType.dashAttack:
                //tmpLvl = 4; 
                tmpLvl = GetRandomValue(2);
                break;
            case AttributeUpgradeTypes.UpgradeType.dashSlam:
                //tmpLvl = 4; 
                tmpLvl = GetRandomValue(0);
                break;
            case AttributeUpgradeTypes.UpgradeType.postDashAttack:
                //tmpLvl = 4; 
                tmpLvl = GetRandomValue(3);
                break;
            case AttributeUpgradeTypes.UpgradeType.evade:
                //tmpLvl = 4; 
                tmpLvl = GetRandomValue(1);
                break;
            default:
                break;
        }
        return tmpLvl;
    }

    int GetRandomValue(int _maxValue)
    {
        int _num;
        return _num = (int)UnityEngine.Random.Range(0, _maxValue);
    }
}

[System.Serializable]
public class PlayerAbilities
{

    //bool doubleJumpedUnlocked = false;

    //bool double_evade = false; 
    //float evade_delay = 2.5f; 
    //float evade_manaCost;

    //dashattack stuff
    //float dashAttack_manaCost;
    //float dashAttack_distance;
    //float dashAttack_time; //timer before dash attack ecexutes automatically
    //bool dashSlamUnlocked = false;
    //bool dashMeleeEndUnlocked = false; //ability that gives the player an oppurtunity
    //to immidetely follow the dash attack with a stronger melee attack
}
