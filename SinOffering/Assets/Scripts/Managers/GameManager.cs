using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour, IGameModeSelectionMenu
{
    #region variables

    public int points; // points player currently has
    public bool paused;

    public bool gameCompleted;


    #region delete these after moving them into gameModeAttribuites
    private int _maxPoints = 0; // this should be referemce to MaxPoint value in _offeringData
    private bool _spawnCrates = false;
    // crate spawn  locations
    public Transform[] _spawnLocs;
    private int _lastSpawnLoc;
    private GameObject _crate;
    #endregion
    
    public GameObject GameWonPanel, GameFailedPanel, pauseMenu;


    //[HideInInspector]
    public bool gameModeSelected = false; 
    public bool inLobbby = false; 

    private IMatchCompletedMenu _client = null;

    private MatchResultData _offeringResults;
    public OfferingData _offeringData;
    private GameMode _gameMode;

    private Text pointsText; 

    //[HideInInspector]
    public GameObject RadialMenu;
    //[HideInInspector]
    public static GameManager instance;
    private GameObject enemyManager;

    [HideInInspector]
    public CameraManager camManager;
    private Scene currentScene;
    private Scene lobbyScene;


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
        //pointsText = GameObject.Find("Text_PointsHUD").GetComponent<Text>();

        Debug.Log("instance: "+ instance);
        if (instance == null)
        {
            instance = this;
            Debug.Log(gameObject);
            DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
        }
        else
        {
            Debug.Log("instance.SetMenus()");
            //GameManager.instance.SetMenus();
            Destroy(this.gameObject.transform.parent.gameObject);
        }
            
    }
    
    void Start () {
        camManager = CameraManager.instance;

        //if (!SoundManager.MusicSource.isPlaying)
        //SoundManager.PlayMusicTrack();
    
    }
	
	void FixedUpdate () {
        if (!paused && !gameCompleted)
            CurGameTime += Time.deltaTime; //Debug.Log("curGameTime: " + CurGameTime);

        // only have this here to check if time attack mode is completed
        // since I cant have an update loop in a scriptable oject
        if (_offeringData != null)
            if (_offeringData.gameMode == GameMode.timeAttack && _offeringData.GameCompleted())
                WonGame();
    }

    // set game rules
    private void InitGameRound()
    {
        switch (_gameMode)
        {
            case GameMode.randomGunBoxes:
                Debug.Log("init - RandomGunMode");
                points = 0;
                RandomGunMode tmpRand = (RandomGunMode)_offeringData.matchSetting;
                _spawnCrates = tmpRand.SpawnCrates;
                _spawnLocs = tmpRand.spawnLocs;
                _maxPoints = tmpRand.MaxPoints;
                _crate = tmpRand.Crates;
                Invoke("SpawnCrate", 1f);
                break;

            case GameMode.randomGunBoxesShield:
                Debug.Log("init - RandomGunModeEx");
                points = 0;
                RandomGunModeExtreme tmpRandEx = (RandomGunModeExtreme)_offeringData.matchSetting;
                _spawnCrates = tmpRandEx.SpawnCrates;
                _spawnLocs = tmpRandEx.spawnLocs;
                _maxPoints = tmpRandEx.MaxPoints;
                _crate = tmpRandEx.Crates;
                Invoke("SpawnCrate", 1f);
                break;
            case GameMode.survival:
                //SurvivalMode tmpSurvival = (SurvivalMode)_offeringData.matchSetting;
                break;
            case GameMode.timeAttack:
                //TimeAttackMode tmpTime = (TimeAttackMode)_offeringData.matchSetting;
                break;
            case GameMode.dashAbilityOnly:
                //DashAbilityMode tmpDash = (DashAbilityMode)_offeringData.matchSetting;
                break;
            case GameMode.meleeOnly:
                //MeleeMode tmpMelee = (MeleeMode)_offeringData.matchSetting;
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
        Invoke("ReturnToLobby", 3);
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
    private void ReturnToLobby()
    {
        // add lobby scene in parameter
        GetComponent<LoadScene>().LoadSceneByIndex(5);
    }

    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        // check and assign lobby sceneID
        if(currentScene.name == "LobbySceneTemplateTest")
            lobbyScene = currentScene;

        if (sceneName == "Limbo")
        {
            enemyManager = GameObject.Find("EnemyManager");
            if (_offeringData != null)
                InitGameRound();
        }
        Debug.Log("OnSceneLoaded: " + scene.name); 

        if(currentScene.name != "MainMenu")
            SetMenus();
    }

    public void AddPoint()
    {
        points++;
        //pointsText.text = points.ToString();
        if (points >= _maxPoints)
            WonGame();
        else
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
        // change _spawnLoc to a single gameobject refrence - not arrat/list!!!
        int locIndex = UnityEngine.Random.Range(0, _spawnLocs[0].childCount);

        while (locIndex == _lastSpawnLoc)
            locIndex = UnityEngine.Random.Range(0, _spawnLocs[0].childCount);
 
        Instantiate(_crate, _spawnLocs[0].GetChild(locIndex).position, _spawnLocs[0].GetChild(locIndex).rotation);
        
        _lastSpawnLoc = locIndex;
    }

    public void IncrementFaithSpent(int value)
    {
        TotalFaithSpent += value;
    }



    private void SetMenus() 
    {
        Debug.Log("--- SetMenus() ---");
        //StartCoroutine("SetMenusDelay");
        if (GameWonPanel == null )
        {
            if (currentScene.name != "HubScene" && 
                currentScene.name != "LobbySceneTemplateTest")
            {
                Debug.Log("setting UI panels");
                GameWonPanel = GameObject.Find("GameWonPanel");
                GameWonPanel.SetActive(false);

                GameFailedPanel = GameObject.Find("GameFailedPanel");
                GameFailedPanel.SetActive(false);
                enemyManager.SetActive(true);
            }
        }
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseMenuUI");
            pauseMenu.SetActive(false);
        }
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



