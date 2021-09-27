using UnityEngine;

[System.Serializable]
public class DataTemplate //: <T>
{
    //[SerializeField]
    //public T data;
}
[System.Serializable]
public class WeaponDataTemplate : IData<WeaponData>
{
    public WeaponData data;

    public int GetLevel()
    {
        return data.GetLevel();
    }

    public int GetPrice()
    {
        return data.WeaponPrice;
        //throw new System.NotImplementedException();
    }

    public float GetValue()
    {
        return data.GetValue();
    }

    WeaponData IData<WeaponData>.GetData()
    {
        return data;
    }
}


public class AttributeDataTemplate : IData<AttributeData>
{
    public AttributeData data;

    public AttributeData GetData()
    {
        return data;
    }

    public int GetLevel()
    {
        throw new System.NotImplementedException();
    }

    public int GetPrice()
    {
        throw new System.NotImplementedException();
    }

    public float GetValue()
    {
        throw new System.NotImplementedException();
    }    
}

public interface IData <T>
{
    int GetPrice();
    int GetLevel();
    float GetValue();
    T GetData();
}
