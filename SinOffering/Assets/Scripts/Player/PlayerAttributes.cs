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
        
    }
    public AttributeData GetData(string _type)
    {
        _data = attributeDatabase[_type.ToString()];
        return _data;
    }

    public int GetCurrentAttributeLevel(AttributeUpgradeTypes.UpgradeType _upgradeType)
    {
        int tmpLvl = 0;
        switch (_upgradeType)
        {
            case AttributeUpgradeTypes.UpgradeType.health:
                //tmpLvl = healthAttributeLevel;
                tmpLvl = 1;
                break;
            case AttributeUpgradeTypes.UpgradeType.mana:
                //tmpLvl = GetRandomValue((int)manaAttributeLevel);
                tmpLvl = 1;//GetRandomValue(2);
                break;
            case AttributeUpgradeTypes.UpgradeType.speed:
                //tmpLvl = GetRandomValue((int)speedAttributeLevel);
                tmpLvl = 2;// GetRandomValue(1);
                break;
            case AttributeUpgradeTypes.UpgradeType.strength:
                //tmpLvl = GetRandomValue((int)strengthAttributeLevel);
                tmpLvl = 1; //GetRandomValue(2);
                break;
            case AttributeUpgradeTypes.UpgradeType.dashAttack:
                tmpLvl = 2; //GetRandomValue(3);
                break;
            case AttributeUpgradeTypes.UpgradeType.dashSlam:
                tmpLvl = 2; //GetRandomValue(3);
                break;
            case AttributeUpgradeTypes.UpgradeType.evade:
                tmpLvl = 3; //GetRandomValue(1);
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
