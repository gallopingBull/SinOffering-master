using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchCompletedMenu : MonoBehaviour, IMatchCompletedMenu
{
    [SerializeField]
    private MatchResultData _matchData;
    private OfferingData _offeringData;

    #region textmeshpro objects
    public TextMeshProUGUI 
        offeringTitle_Text,
        ////offeringFailedMessage_Text,
        //offeringType_Text,
        offeringReward_Faith_Text,
        offeringReward_Silver_Text,
        
        matchResults_totalTime_Text,
        matchResults_totalKills_Text,
        matchResults_totalDeaths_Text,
        matchResults_totalDamageRecieved_Text,
        matchResults_totalBiggestKillStreak_Text,
        matchResults_totalGunKills_Text,
        matchResults_totalMeleeKills_Text,
        matchResults_totalEnemiesSliced_Text,
        matchResults_totalEnemiesExploded_Text,
        matchResults_totalSilverAccrued_Text,
        matchResults_totalItemsUsed_Text;
    #endregion
    
    // assign text values to UI elements
    public void InitMenu()
    {
        offeringTitle_Text.text = _offeringData.offeringTitle;
        //offeringFailedMessage_Text,
        //offeringType_Text.text = _offeringData.gameMode.ToString();
        //offeringReward_Faith_Text.text = _offeringData.rewards[0].baseRewardValue.ToString();
        //offeringReward_Silver_Text.text = _offeringData.rewards[1].baseRewardValue.ToString();

        matchResults_totalTime_Text.text = _matchData.totalMatchTime.ToString();
        matchResults_totalKills_Text.text = _matchData.totalKills.ToString();
        matchResults_totalDeaths_Text.text = _matchData.totalDeaths.ToString();
        //matchResults_totalDamageRecieved_Text.text = _matchData.totalDamageRecieved.ToString();
        //matchResults_totalBiggestKillStreak_Text.text = _matchData.totalBiggestKillStreak.ToString();
        matchResults_totalGunKills_Text.text = _matchData.totalGunKills.ToString();
        matchResults_totalMeleeKills_Text.text = _matchData.totalMeleeKills.ToString();
        matchResults_totalEnemiesSliced_Text.text = _matchData.totalEnemiesSliced.ToString();
        matchResults_totalEnemiesExploded_Text.text = _matchData.totalEnemiesExploded.ToString();
        matchResults_totalSilverAccrued_Text.text = _matchData.totalSilverAccrued.ToString();
        //matchResults_totalItemsUsed_Text.text = _matchData.totalItemsUsed.ToString();
    }

    public void InitButton(/*OfferingSelectionButton _button, OfferingSelectionData _offeringData, string _upgradeName, int curUpgradeLvl*/)
    {
    }

    void IMatchCompletedMenu.SetMatchData(MatchResultData resultData, OfferingData offeringData)
    {
        _matchData = resultData;
        _offeringData = offeringData;
        InitMenu();
    }
}

public interface IMatchCompletedMenu
{
    public void SetMatchData(MatchResultData resultData, OfferingData offeringData);

}

