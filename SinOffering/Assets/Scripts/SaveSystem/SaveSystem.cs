using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private GameData _gameData = new GameData();
    private PlayerSettings _playerSettings = new PlayerSettings();

    private GameManager gameManager;
    private PlayerController player;

    public PlayerSettings PlayerSettings { get => _playerSettings; set => _playerSettings = value; }
    public GameData GameData { get => _gameData; set => _gameData = value; }

    private void Start()
    {   
        gameManager = GameManager.instance;
        player = PlayerController.instance;
        //InitData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SaveGameData();

        if (Input.GetKeyDown(KeyCode.L))
            LoadGameData();
    }
    
    private void InitData()
    {
      
    }
    public void SaveGameData()
    {
        _gameData.WeaponAttributesDatas.Clear();
        for (int i = 0; i < player.weaponManager.Weapons.Length; i++)
        {
            var curWeapon = player.weaponManager.Weapons[i].GetComponent<Weapon>().WeaponAttributes;
            
            if (!curWeapon.WeaponPurchased)
                continue;

            _gameData.WeaponAttributesDatas.Add(curWeapon);
        }

        _gameData.TotalWins = _gameData.TotalWins + gameManager.CurWins;
        _gameData.TotalLosses = _gameData.TotalLosses + gameManager.CurLosses;
        _gameData.TotalGamesPlayed = _gameData.TotalGamesPlayed + gameManager.CurGamesPlayed;
        _gameData.TotalGameTime = _gameData.TotalGameTime + gameManager.CurGameTime;
        _gameData.TotalEnemyKills = _gameData.TotalEnemyKills + gameManager.CurEnemyKills;
        _gameData.TotalDeaths = _gameData.TotalDeaths + gameManager.CurDeaths;
        _gameData.TotalSuicides = _gameData.TotalSuicides + gameManager.CurSuicides;
        _gameData.TotalSilver = gameManager.TotalSilver + gameManager.curSilver;

        Debug.Log("Saving Game...");
        var json = JsonUtility.ToJson(_gameData);
        
        PlayerPrefs.SetString("GameData", json);
    }

    private void LoadGameData()
    {
        Debug.Log("Loading Game...");
        string json = PlayerPrefs.GetString("GameData");
        _gameData = JsonUtility.FromJson<GameData>(json);

        //Debug.Log(json);
        for (int i = 0; i < _gameData.WeaponAttributesDatas.Count; i++)
        {
            var curWeapon = _gameData.WeaponAttributesDatas[i];
            for (int j = 0; j < player.weaponManager.Weapons.Length; j++) {
                Debug.Log("curWeapon: " + curWeapon.weaponName);
                if (curWeapon.weaponName != player.weaponManager.Weapons[j].GetComponent<Weapon>().GetWeaponName())
                    continue;

                player.weaponManager.Weapons[j].GetComponent<Weapon>().WeaponAttributes.SetWeaponAttributeData(_gameData.WeaponAttributesDatas[i]);
                player.weaponManager.Weapons[j].GetComponent<Weapon>().SetWeaponAttributeFields();
            }
        }

        gameManager.CurWins = _gameData.TotalWins;
        gameManager.CurLosses = _gameData.TotalLosses;
        gameManager.CurGamesPlayed = _gameData.TotalGamesPlayed;
        gameManager.CurGameTime = _gameData.TotalGameTime;
        gameManager.CurEnemyKills = _gameData.TotalEnemyKills;
        gameManager.CurDeaths = _gameData.TotalDeaths;
        gameManager.CurSuicides = _gameData.TotalSuicides;
        gameManager.TotalSilver = _gameData.TotalSilver;
    }
    private void SavePlayerSettings()
    {

    }

    private void LoadPlayerSettings()
    {

    }
}

