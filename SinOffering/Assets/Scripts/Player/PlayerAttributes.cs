using System;

[System.Serializable]
public class PlayerAttributes 
{

    //float max_health = 0;
    //float mana = 0;

    //float airControl; // maybe...
    //int equipmentSize; // maybe...

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



    public int GetAttributeLevel(AttributeUpgradeTypes.UpgradeType _upgradeType)
    {
        int tmpLvl = 0;
        switch (_upgradeType)
        {
            case AttributeUpgradeTypes.UpgradeType.health:
                break;
            case AttributeUpgradeTypes.UpgradeType.mana:
                break;
            case AttributeUpgradeTypes.UpgradeType.speed:
                break;
            case AttributeUpgradeTypes.UpgradeType.strength:
                break;
            case AttributeUpgradeTypes.UpgradeType.dashAttack:
                break;
            case AttributeUpgradeTypes.UpgradeType.evade:
                break;
            default:
                break;
        }

        return tmpLvl;
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
