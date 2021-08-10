using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "WeaponData", menuName ="DataObjects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    Weapon.WeaponTypes WeaponType;
    public int WeaponPrice; //price for store to get

    [SerializeField]
    public WeaponAttributeData[] AttributeDataList;
}
    


