using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// singleton class that loads and saves game state/stats/progress and other pertinent data.
/// </summary>

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem _instance;
    private GameData _gameData = new GameData();
    private PlayerSettings _playerSettings = new PlayerSettings();
    private GameManager _gameManager;
    private PlayerController _player;

    public PlayerSettings PlayerSettings { get => _playerSettings; set => _playerSettings = value; }
    
    public GameData GameData { get => _gameData; set => _gameData = value; }
    
    public static SaveSystem Instance { get => _instance; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _player = PlayerController.instance;
        //InitData();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
            _ = SaveGameDataAsync();

        //if (Input.GetKeyDown(KeyCode.K))
        //    SaveGameData();

        //if (Input.GetKeyDown(KeyCode.L))
        //    LoadGameData();
        if (Input.GetKeyDown(KeyCode.L))
            _ = LoadGameDataAsync();
    }

    private void InitData()
    {

    }


    // non async save game data
    public void SaveGameData()
    {
        _gameData.WeaponAttributesDatas.Clear();
        for (int i = 0; i < _player.weaponManager.Weapons.Length; i++)
        {
            var curWeapon = _player.weaponManager.Weapons[i].GetComponent<Weapon>().WeaponAttributes;

            if (!curWeapon.WeaponPurchased)
                continue;

            _gameData.WeaponAttributesDatas.Add(curWeapon);
        }

        _gameData.PlayerAttributes = new PlayerAttributes();
        _gameData.PlayerAttributes = _player.Attributes;

        _gameData.TotalWins = _gameData.TotalWins + _gameManager.CurWins;
        _gameData.TotalLosses = _gameData.TotalLosses + _gameManager.CurLosses;
        _gameData.TotalGamesPlayed = _gameData.TotalGamesPlayed + _gameManager.CurGamesPlayed;
        _gameData.TotalGameTime = _gameData.TotalGameTime + _gameManager.CurGameTime;
        _gameData.TotalEnemyKills = _gameData.TotalEnemyKills + _gameManager.CurEnemyKills;
        _gameData.TotalDeaths = _gameData.TotalDeaths + _gameManager.CurDeaths;
        _gameData.TotalSuicides = _gameData.TotalSuicides + _gameManager.CurSuicides;
        _gameData.TotalSilver = _gameManager.TotalSilver + _gameManager.curSilver;
        _gameData.TotalFaith = _gameManager.TotalCurrentFaith;
        _gameData.TotalFaithSpent = _gameManager.TotalFaithSpent;

        Debug.Log("Saving Game...");
        var json = JsonUtility.ToJson(_gameData);
        PlayerPrefs.SetString("GameData", json);
    }

    private async Task SaveGameDataAsync()
    {
        Debug.Log("Saving Game...");
        _gameData.WeaponAttributesDatas.Clear();
        for (int i = 0; i < _player.weaponManager.Weapons.Length; i++)
        {
            var curWeapon = _player.weaponManager.Weapons[i].GetComponent<Weapon>().WeaponAttributes;
            if (!curWeapon.WeaponPurchased)
                continue;
            _gameData.WeaponAttributesDatas.Add(curWeapon);
        }

        _gameData.PlayerAttributes = new PlayerAttributes();
        _gameData.PlayerAttributes = _player.Attributes;

        _gameData.TotalWins = _gameData.TotalWins + _gameManager.CurWins;
        _gameData.TotalLosses = _gameData.TotalLosses + _gameManager.CurLosses;
        _gameData.TotalGamesPlayed = _gameData.TotalGamesPlayed + _gameManager.CurGamesPlayed;
        _gameData.TotalGameTime = _gameData.TotalGameTime + _gameManager.CurGameTime;
        _gameData.TotalEnemyKills = _gameData.TotalEnemyKills + _gameManager.CurEnemyKills;
        _gameData.TotalDeaths = _gameData.TotalDeaths + _gameManager.CurDeaths;
        _gameData.TotalSuicides = _gameData.TotalSuicides + _gameManager.CurSuicides;
        _gameData.TotalSilver = _gameManager.TotalSilver + _gameManager.curSilver;
        _gameData.TotalFaith = _gameManager.TotalCurrentFaith;
        _gameData.TotalFaithSpent = _gameManager.TotalFaithSpent;

        await WriteGameData(_gameData);
        Debug.Log("Finished saving game...");
    }

    private Task WriteGameData(GameData gameData)
    {
        Debug.Log("Writing to disk...");
        var json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("GameData", json);
        return Task.CompletedTask;
    }
     
    // non async load game data
    private void LoadGameData()
    {
        Debug.Log("Loading Game...");
        string json = PlayerPrefs.GetString("GameData");
        _gameData = JsonUtility.FromJson<GameData>(json);

        //Debug.Log(json);
        for (int i = 0; i < _gameData.WeaponAttributesDatas.Count; i++)
        {
            var curWeapon = _gameData.WeaponAttributesDatas[i];
            for (int j = 0; j < _player.weaponManager.Weapons.Length; j++)
            {
                Debug.Log("curWeapon: " + curWeapon.weaponName);
                if (curWeapon.weaponName != _player.weaponManager.Weapons[j].GetComponent<Weapon>().GetWeaponName())
                    continue;

                _player.weaponManager.Weapons[j].GetComponent<Weapon>().WeaponAttributes.SetWeaponAttributeData(_gameData.WeaponAttributesDatas[i]);
                _player.weaponManager.Weapons[j].GetComponent<Weapon>().SetWeaponAttributeFields();
            }
        }

        //player.Attributes = _gameData.PlayerAttributes;
        _player.SetPersistentPlayerAttributeData(_gameData.PlayerAttributes);

        _gameManager.CurWins = _gameData.TotalWins;
        _gameManager.CurLosses = _gameData.TotalLosses;
        _gameManager.CurGamesPlayed = _gameData.TotalGamesPlayed;
        _gameManager.CurGameTime = _gameData.TotalGameTime;
        _gameManager.CurEnemyKills = _gameData.TotalEnemyKills;
        _gameManager.CurDeaths = _gameData.TotalDeaths;
        _gameManager.CurSuicides = _gameData.TotalSuicides;
        _gameManager.TotalSilver = _gameData.TotalSilver;
        _gameManager.TotalCurrentFaith = _gameData.TotalFaith;
        _gameManager.TotalFaithSpent = _gameData.TotalFaithSpent;

        GameEvents.OnCurrencyUpdateEvent?.Invoke(_gameManager.TotalSilver);
        GameEvents.OnFaithUpdateEvent?.Invoke(_gameManager.TotalCurrentFaith);
    }

    private async Task LoadGameDataAsync()
    {
        Debug.Log("Loading Game...");

        Task<GameData> GetGameDataTask = ReadGameData("GameData");
        _gameData = await GetGameDataTask;

        SetGameData(_gameData);
    }

    private Task<GameData> ReadGameData(string dataID)
    {
        GameData gameData;
        string json = PlayerPrefs.GetString(dataID);
        gameData = JsonUtility.FromJson<GameData>(json);
        return Task.FromResult(gameData);
    }

    private void SetGameData(GameData gameData)
    {
        //Debug.Log(json);
        for (int i = 0; i < gameData.WeaponAttributesDatas.Count; i++)
        {
            var curWeapon = gameData.WeaponAttributesDatas[i];
            for (int j = 0; j < _player.weaponManager.Weapons.Length; j++)
            {
                Debug.Log("curWeapon: " + curWeapon.weaponName);
                if (curWeapon.weaponName != _player.weaponManager.Weapons[j].GetComponent<Weapon>().GetWeaponName())
                    continue;

                _player.weaponManager.Weapons[j].GetComponent<Weapon>().WeaponAttributes.SetWeaponAttributeData(gameData.WeaponAttributesDatas[i]);
                _player.weaponManager.Weapons[j].GetComponent<Weapon>().SetWeaponAttributeFields();
            }
        }

        //player.Attributes = _gameData.PlayerAttributes;
        _player.SetPersistentPlayerAttributeData(_gameData.PlayerAttributes);

        _gameManager.CurWins = gameData.TotalWins;
        _gameManager.CurLosses = gameData.TotalLosses;
        _gameManager.CurGamesPlayed = gameData.TotalGamesPlayed;
        _gameManager.CurGameTime = gameData.TotalGameTime;
        _gameManager.CurEnemyKills = gameData.TotalEnemyKills;
        _gameManager.CurDeaths = gameData.TotalDeaths;
        _gameManager.CurSuicides = gameData.TotalSuicides;
        _gameManager.TotalSilver = gameData.TotalSilver;
        _gameManager.TotalCurrentFaith = gameData.TotalFaith;
        _gameManager.TotalFaithSpent = gameData.TotalFaithSpent;

        GameEvents.OnCurrencyUpdateEvent?.Invoke(_gameManager.TotalSilver);
        GameEvents.OnFaithUpdateEvent?.Invoke(_gameManager.TotalCurrentFaith);
    }

    private void SavePlayerSettings()
    {
    }

    private void LoadPlayerSettings()
    {
    }
}

