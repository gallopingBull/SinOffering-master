using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "AttributeData", menuName = "DataObjects/Attribute Data")]
public class AttributeData : ScriptableObject
{
    public AttributeUpgradeTypes.UpgradeType UpgradeType;

    [SerializeField]
    public AttributeUpgradeData[] AttributeDataList;
}



