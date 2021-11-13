using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCompletedMenu : MonoBehaviour, IMatchCompletedMenu
{
    
    //public GameObject menu = null;
    //private HUDManager hm;

    [SerializeField]
    private MatchResultData _matchData;


    public void SetMatchData()
    {
        //_client.SetMatchData(_matchData);
    }

    public void InitMenu()
    {
        // assign text values to UI elements
    }

    public void InitButton(/*OfferingSelectionButton _button, OfferingSelectionData _offeringData, string _upgradeName, int curUpgradeLvl*/)
    {
    }

    void IMatchCompletedMenu.SetMatchData(MatchResultData resultData)
    {
        _matchData = resultData;
    }
}

public interface IMatchCompletedMenu
{
    public void SetMatchData(MatchResultData resultData);

}

