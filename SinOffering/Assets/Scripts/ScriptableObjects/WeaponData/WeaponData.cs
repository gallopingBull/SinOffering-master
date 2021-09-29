using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "WeaponData", menuName ="DataObjects/Weapon Data")]
public class WeaponData : ScriptableObject//, IData<WeaponData>
{
    Weapon.WeaponTypes WeaponType;
    public int WeaponPrice; // price for store to get

    [SerializeField]
    public WeaponAttributeData[] AttributeDataList;

    #region interface testing
    public int GetPrice()
    {  
        return WeaponPrice;
    }

    public int GetLevel()
    {
        throw new System.NotImplementedException();
    }

    public float GetValue()
    {
        throw new System.NotImplementedException();
    }

    public WeaponData GetData()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
    


