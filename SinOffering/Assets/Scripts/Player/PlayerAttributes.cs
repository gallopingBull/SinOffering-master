using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerAttributes 
{
    [SerializeField]
    private AttributeData _data;
    private Dictionary<string, AttributeData> attributeDatabase;
    
    [SerializeField]
    int healthAttributeLevel = 0;
    [SerializeField]
    float max_health = 0;
    float health = 0;

    [SerializeField]
    int manaAttributeLevel = 0;
    [SerializeField]
    float max_mana = 0;
    float mana = 0;

    [SerializeField]
    int speedAttributeLevel = 0;
    [SerializeField]
    float max_speed = 0;
    float speed = 0;

    [SerializeField]
    int strengthAttributeLevel = 0;
    [SerializeField]
    float max_strength = 0;
    float strength = 0;

    //float airControl; // maybe...
    //int equipmentSize; // maybe...

    [SerializeField]
    int evadeAttributeLevel = 0;
    bool doubleJumpedUnlocked = false;

    bool double_evade = false; 
    float evade_delay = 2.5f; 
    float evade_manaCost;

    //dashattack stuff
    [SerializeField]
    int dashAttack_AttributeLevel = 0;
    [SerializeField]
    int dashSlam_AttributeLevel = 0;
    [SerializeField]
    int postDashAttack_AttributeLevel = 0;
    
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
    
    public Dictionary<AttributeUpgradeTypes.UpgradeType, int> GetAttributeLevelData()
    {
        Dictionary<AttributeUpgradeTypes.UpgradeType, int> tmpDic = new Dictionary<AttributeUpgradeTypes.UpgradeType, int>();
        
        #region testing
        //tmpDic.Add(AttributeUpgradeTypes.UpgradeType.health,  GetRandomValue(4));
        //tmpDic.Add(AttributeUpgradeTypes.UpgradeType.mana,  GetRandomValue(4));
        //tmpDic.Add(AttributeUpgradeTypes.UpgradeType.dashAttack, GetRandomValue(4));
        //tmpDic.Add(AttributeUpgradeTypes.UpgradeType.dashSlam, dashSlam_AttributeLevel);
        //tmpDic.Add(AttributeUpgradeTypes.UpgradeType.postDashAttack, postDashAttack_AttributeLevel);
        //tmpDic.Add(AttributeUpgradeTypes.UpgradeType.strength, strengthAttributeLevel);
        //tmpDic.Add(AttributeUpgradeTypes.UpgradeType.evade, evadeAttributeLevel);
        //tmpDic.Add(AttributeUpgradeTypes.UpgradeType.speed, speedAttributeLevel);
        #endregion

        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.health, healthAttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.mana, manaAttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.dashAttack, dashAttack_AttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.dashSlam, dashSlam_AttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.postDashAttack, postDashAttack_AttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.strength, strengthAttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.evade, evadeAttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.speed, speedAttributeLevel);

        return tmpDic;
    }

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
