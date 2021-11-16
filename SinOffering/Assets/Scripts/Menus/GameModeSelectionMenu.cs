using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameModeSelectionMenu : MonoBehaviour
{
    private IGameModeSelectionMenu _client = null;
    private Dictionary<int, OfferingData> offeringDatabase;
    //public Button[] menuButtons = null;
    //public GameObject menu = null;

    public TextMeshProUGUI offeringTitle_Text,
        offeringSummary_Text,
        offeringType_Text,
        offeringReward_Faith_Text,
        offeringReward_Silver_Text;

    public Image[] offeringGradeImages;

    private HUDManager hm;
    
    [SerializeField]
    private GameMode _gameMode;

    private void Start()
    {
        offeringDatabase = OfferingDatabase._instance.GetOfferingDatabase();
        hm = HUDManager._instance;
    }

    public void SetClient(IGameModeSelectionMenu client)
    {
        _client = client;
    }

    public void SendGameMode()
    {
        _client.SetGameMode(_gameMode);
    }

    public void InitMenu()
    {

    }

    public void InitButton(/*OfferingSelectionButton _button, OfferingSelectionData _offeringData, string _upgradeName, int curUpgradeLvl*/)
    {

    }
}

public interface IGameModeSelectionMenu
{
    void SetGameMode(GameMode gameMode);
}

