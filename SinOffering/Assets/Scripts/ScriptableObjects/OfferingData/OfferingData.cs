using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
[CreateAssetMenu(fileName = "OfferingData", menuName ="DataObjects/Offering Data")]
public class OfferingData : ScriptableObject//, IData<WeaponData>
{
    public string offeringTitle;
    [TextArea]
    public string offeringSummary; // price for store to get
    [Tooltip("Keep value at '0' if no time limit is required (infinite)")]
    public float timeLimit = 0;
    
    public Image characterPortrait;
    public GameMode gameMode;
    public Reward[] rewards;
}


// end match reward
[System.Serializable]
public struct Reward
{
    public RewardType rewardType;
    public int baseRewardValue;
}

public enum RewardType
{
    faith, silver//, someItem, weapon, ability etc...
}
    


 