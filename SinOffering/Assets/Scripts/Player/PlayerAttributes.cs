using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerAttributes 
{
    #region variables
    [SerializeField]
    private AttributeData _data;
    private Dictionary<string, AttributeData> attributeDatabase;
    
    [SerializeField]
    private int healthAttributeLevel = 0;
    [SerializeField]
    private float max_health = 0;
    private float health = 0;

    [SerializeField]
    private int manaAttributeLevel = 0;
    [SerializeField]
    private float max_mana = 0;
    private float mana = 0;

    [SerializeField]
    private int speedAttributeLevel = 0;
    [SerializeField]
    private float max_speed = 0;
    private float speed = 0;

    [SerializeField]
    private int strengthAttributeLevel = 0;
    [SerializeField]
    private float max_strength = 0;
    private float strength = 0;

    //float airControl; // maybe...
    //int equipmentSize; // maybe...

    [SerializeField]
    private int evadeAttributeLevel = 0;
    private bool doubleJumpedUnlocked = false;
    
    private bool double_evade = false; 
    private float evade_delay = 2.5f; 
    private float evade_manaCost;
    
    //dashattack stuff
    [SerializeField]
    private int dashAttack_AttributeLevel = 0;
    [SerializeField]
    private int dashSlam_AttributeLevel = 0;
    [SerializeField]
    private int postDashAttack_AttributeLevel = 0; // to immidetely follow the dash attack with a stronger melee attack

    private float dashAttack_manaCost;
    private float dashAttack_distance;
    private float dashAttack_time; // timer before dash attack ecexutes automatically
    private bool dashSlamUnlocked = false;
    private bool dashMeleeEndUnlocked = false; // ability that gives the player an oppurtunity


    #region attribute fields
    public int HealthAttributeLevel { get => healthAttributeLevel; set => healthAttributeLevel = value; }
    public int ManaAttributeLevel { get => manaAttributeLevel; set => manaAttributeLevel = value; }
    public int StrengthAttributeLevel { get => strengthAttributeLevel; set => strengthAttributeLevel = value; }
    public int SpeedAttributeLevel { get => speedAttributeLevel; set => speedAttributeLevel = value; }
    public int DashAttack_AttributeLevel { get => dashAttack_AttributeLevel; set => dashAttack_AttributeLevel = value; }
    public int DashSlam_AttributeLevel { get => dashSlam_AttributeLevel; set => dashSlam_AttributeLevel = value; }
    public int PostDashAttack_AttributeLevel { get => postDashAttack_AttributeLevel; set => postDashAttack_AttributeLevel = value; }
    public int EvadeAttributeLevel { get => evadeAttributeLevel; set => evadeAttributeLevel = value; }

    #endregion

    #endregion

    #region functions
    void Awake()
    {
        attributeDatabase = AttributeDatabase._instance.GetAttributeDatabase();
    }
    
    public Dictionary<AttributeUpgradeTypes.UpgradeType, int> GetAttributeLevelData()
    {
        Dictionary<AttributeUpgradeTypes.UpgradeType, int> tmpDic = new Dictionary<AttributeUpgradeTypes.UpgradeType, int>();
        
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.health, healthAttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.mana, ManaAttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.dashAttack, dashAttack_AttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.dashSlam, dashSlam_AttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.postDashAttack, postDashAttack_AttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.strength, strengthAttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.evade, evadeAttributeLevel);
        tmpDic.Add(AttributeUpgradeTypes.UpgradeType.speed, speedAttributeLevel);

        return tmpDic;
    }

    // used for testing random attribute levels
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

    public void SetPlayerAttributeData(AttributeData data)
    {
        //healthAttributeLevel = data.AttributeDataList[].AttributeLevel; 

        //SetHealthValue(data.AttributeDataList[].AttributeValue);
    }

    private void SetHealthValue(float _value)
    {
        max_health = _value;
        health = max_health;
    }

    private void SetManaValue(float _value)
    {
        max_mana = _value;
        mana = max_mana;
    }

    private void SetStrengthValue(float _value)
    {
        max_strength = _value;
        strength = max_strength;
    }
    private void SetDashAttackValue(float _value)
    {
        max_strength = _value;
        strength = max_strength;
    }

    int GetRandomValue(int _maxValue)
    {
        int _num;
        return _num = (int)UnityEngine.Random.Range(0, _maxValue);
    }
    #endregion
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
