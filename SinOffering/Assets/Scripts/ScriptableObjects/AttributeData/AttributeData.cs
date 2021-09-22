using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "AttributeData", menuName = "DataObjects/Attribute Data")]
public class AttributeData : ScriptableObject
{
    public AttributeUpgradeTypes.UpgradeType UpgradeType;
    public int upgradePrice; //price for store to get

    [SerializeField]
    public AttributeUpgradeData[] AttributeDataList;
}



