using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using System;


public class GameManager : MonoBehaviour, IGameModeSelectionMenu{

    #region variables

    public int points; // points player currently has
    #region delete these after moving them into gameModeAttribuites
    public int MaxPoints = 10; // this should be referemce to MaxPoint value in _offeringData
    public bool SpawnCrates = false;
    // crate spawn  locations
    public Transform[] spawnLocs;
    private int lastSpawnLoc;
    public GameObject Crate;
    #endregion
    
    public GameObject GameWonPanel, GameFailedPanel, pauseMenu;

    private MatchResultData _offeringResults;
    private OfferingData _offeringData;
    private GameMode _gameMode;
    //[HideInInspector]
    public bool gameModeSelected = false; 
    public bool inLobbby = false; 

    private IMatchCompletedMenu _client = null;

    public bool paused; 

    public bool gameCompleted;

    private Text pointsText; 

    [HideInInspector]
    public static GameManager instance;

    //[HideInInspector]
    public GameObject RadialMenu;

    public List<GameObject> lights;
    [HideInInspector]
    public CameraManager camManager;

    #region Peristent GameStats

    //[HideInInspector]
    public int TotalSilver = 10000;
    [HideInInspector]
    public int curSilver = 10;

    public int TotalCurrentFaith = 10000;
    public int TotalFaithSpent = 10000;
    //[HideInInspector]
    //public int currentFaith = 10; // maybe redundant variable

    [HideInInspector]
    public int TotalEnemyKills;
    [HideInInspector]
    public int CurEnemyKills;

    [HideInInspector]
    public int TotalDeaths;
    [HideInInspector]
    public int CurDeaths;

    [HideInInspector]
    public int TotalSuicides;
    [HideInInspector]
    public int CurSuicides;

    [HideInInspector]
    public int CurGamesPlayed;
    [HideInInspector]
    public int TotalGamesPlayed;

    [HideInInspector]
    public int CurWins;
    [HideInInspector]
    public int TotalWins;

    [HideInInspector]
    public int CurLosses;
    [HideInInspector]
    public int TotalLosses;

    [HideInInspector]
    public float CurGameTime;
    [HideInInspector]

    public string s_CurGameTime;
    [HideInInspector]
    public float TotalGameTime;

    #endregion


    #endregion

    #region functions
    private void Awake()
    {
        pointsText = GameObject.Find("Text_PointsHUD").GetComponent<Text>();
        instance = this;
    }

    void Start () {
        camManager = CameraManager.instance;
        
        //if (!SoundManager.MusicSource.isPlaying)
            //SoundManager.PlayMusicTrack();

        //if (SpawnCrates)
            //Invoke("SpawnCrate", 1f);
    }
	
	void FixedUpdate () {
        if (!paused && !gameCompleted)
            CurGameTime += Time.deltaTime; //Debug.Log("curGameTime: " + CurGameTime);

        //if (_offeringData != null)
           //if (_offeringData.GameCompleted())
                //WonGame();

        if (points == MaxPoints)
            WonGame();
    }

    // set game rules
    private void InitGameRound()
    {
        switch (_gameMode)
        {
            case GameMode.randomGunBoxes:
                SpawnCrates = true;
                Invoke("SpawnCrate", 1f);
                break;
            case GameMode.randomGunBoxesShield:
                SpawnCrates = true;
                Invoke("SpawnCrate", 1f);
                break;
            case GameMode.survival:
                break;
            case GameMode.timeAttack:
                break;
            case GameMode.dashAbilityOnly:
                break;
            case GameMode.meleeOnly:
                break;
            default:
                break;
        }
    }
   
    private void WonGame()
    {
        gameModeSelected = false;
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            if (RadialMenu.activeInHierarchy)
                RadialMenu.SetActive(false);
        }
      
        gameCompleted = true;

        s_CurGameTime = FormatTime(CurGameTime);
        _offeringResults.totalMatchTime = s_CurGameTime;

        _client = GameWonPanel.GetComponent<IMatchCompletedMenu>();
        _client.SetMatchData(_offeringResults, _offeringData);
        
        GameWonPanel.SetActive(true);
        Invoke("ResetGame", 3);
    }

    public void FailedGame()
    {
        gameCompleted = true;
        GameFailedPanel.SetActive(true);

        s_CurGameTime = FormatTime(CurGameTime);
        _offeringResults.totalMatchTime = s_CurGameTime;

        _client = GameFailedPanel.GetComponent<IMatchCompletedMenu>();
        _client.SetMatchData(_offeringResults, _offeringData);

        Invoke("ResetGame", 3);
    }

    private void ResetGame()
    {
        GetComponent<LoadScene>().ReloadCurrentScene();
    }

    public void AddPoint()
    {
        points++;

        /*
         * i think these lights should be an observer for the points
         * lights turn on based on that
         * */

        switch (points)
        {
            case 6:
                lights[0].SetActive(true);
                break;
            case 7:
                lights[1].SetActive(true);
                break;
            case 8:
                lights[2].SetActive(true);
                break;
            case 9:
                lights[3].SetActive(true);
                break;
            case 10:
                lights[4].SetActive(true);
                break;
            default:
                break;
        }

        pointsText.text = points.ToString(); 
        SpawnCrate();
    }

    public void IncrementSilver(int value)
    {
        _offeringResults.totalSilverAccrued += value; 
        TotalSilver += value;
        HUDManager._instance.SetUIObjectValues();
    }

    public void IncrementKillCount(int value)
    {
        _offeringResults.totalKills += value;
        TotalEnemyKills += value;
    }


    public void IncrementDeathCount()
    {
        _offeringResults.totalDeaths++;
        TotalDeaths++;
    }

    public void DecrementSilver(int value)
    {
        //_offeringResults.totalSilverAccrued -= value; // maybe an enemy that steals some silver during combat
        TotalSilver -= value;
        HUDManager._instance.SetUIObjectValues();
    }
    private void SpawnCrate()
    {
        int locIndex = UnityEngine.Random.Range(0, spawnLocs.Length);

        while (locIndex == lastSpawnLoc)
            locIndex = UnityEngine.Random.Range(0, spawnLocs.Length);

        Instantiate(Crate, spawnLocs[locIndex].position, spawnLocs[locIndex].rotation);
        lastSpawnLoc = locIndex;
    }

    public void IncrementFaithSpent(int value)
    {
        TotalFaithSpent += value;
    }

    void IGameModeSelectionMenu.SetOfferingData(OfferingData offeringData)
    {
        gameModeSelected = true; // this is only used for offering gate object
        _offeringData = offeringData;
        _gameMode = _offeringData.gameMode;
    }
    string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = String.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }
    #endregion

}
[System.Serializable]
[CreateAssetMenu(fileName = "GameMode", menuName = "DataObjects/GameMode")]
public class GameModeAttributesSSS: ScriptableObject
{
    // game mode
    // rules 
    // objective
    // match time

    public int MaxPoints = 10;
    public bool SpawnCrates = false;
    // crate spawn  locations
    public Transform[] spawnLocs;
    private int lastSpawnLoc;
    public GameObject Crate;
    //abstract public void CheckGameStatus();

}

//class RandomWeapons : GameModeAttributes
//{
    // game mode
    // rules 
    // objective
    // match time

    //override public void CheckGameStatus() { 

    //}

//}

public enum GameMode
{
    randomGunBoxes,
    randomGunBoxesShield,
    survival,
    timeAttack,
    dashAbilityOnly,
    meleeOnly
}
public struct MatchResultData
{
    public string favoriteWeapon;
    public string totalMatchTime;

    public int totalKills;
    public int totalDeaths;
    public int totalDamageRecieved;
    public int totalBiggestKillStreak;
    public int totalGunKills;
    public int totalMeleeKills;
    public int totalEnemiesSliced;
    public int totalEnemiesExploded;
    public int totalSilverAccrued;
    public int totalItemsUsed;
}



