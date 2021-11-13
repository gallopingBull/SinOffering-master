using UnityEngine;

public class GameModeSelectionMenu : MonoBehaviour
{
    private IGameModeSelectionMenu _client = null;
    //private Dictionary<string, GameModeData> offeringDatabase;
    //public Button[] menuButtons = null;
    //public GameObject menu = null;
    //private HUDManager hm;
    
    [SerializeField]
    private GameMode _gameMode;

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

