using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCompletedMenu : MonoBehaviour
{
    private IMatchCompletedMenu _client = null;
    //public GameObject menu = null;
    //private HUDManager hm;

    [SerializeField]
    private MatchResultData _matchData;

    public void SetClient(IMatchCompletedMenu client)
    {
        _client = client;
    }

    public void SetMatchData()
    {
        _client.SetMatchData(_matchData);
    }

    public void InitMenu()
    {
        // assign text values to UI elements
    }

    public void InitButton(/*OfferingSelectionButton _button, OfferingSelectionData _offeringData, string _upgradeName, int curUpgradeLvl*/)
    {
    }

}

public interface IMatchCompletedMenu
{
    //void AssignUIData(MatchResultData resultData);
    //void AssignUIData(MatchResultData resultData);
    //void AssignUIData(MatchResultData resultData);
    public void SetMatchData(MatchResultData resultData);

}

