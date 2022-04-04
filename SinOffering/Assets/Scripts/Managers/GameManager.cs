using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour, IGameModeSelectionMenu
{
    #region variables

    public int Points; // points player currently has
    public bool Paused;

    public bool GameCompleted;

    #region delete these after moving them into gameModeAttribuites
    private int _maxPoints = 0; // this should be referemce to MaxPoint value in _offeringData
    
    private float _gameTime = 60f;
    private float _startTime;
    

    private bool _spawnCrates = false;
    // crate spawn  locations
    public Transform[] _spawnLocs;
    private int _lastSpawnLoc;
    private GameObject _crate;
    #endregion
    
    public GameObject GameWonPanel, GameFailedPanel, pauseMenu;

    //[HideInInspector]
    // create game states
    public bool GameModeSelected = false; 
    public bool InGame = false; 
    public bool InLobbby = false; 

    private IMatchCompletedMenu _client = null;

    private MatchResultData _offeringResults;
    public OfferingData _offeringData;
    private GameMode _gameMode;

    private Text _pointsText; 
    private Text _timeText; 

    //[HideInInspector]
    public GameObject RadialMenu;
    //[HideInInspector]
    private static GameManager _instance;
    private GameObject enemyManager;

    [HideInInspector]
    public CameraManager camManager;
    private Scene currentScene;
    private Scene lobbyScene;

    //private bool _weaponsEnabled = true;
    //private bool _dashAbilityEnabled = true;
    //private bool _meleeEnabled = true;

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

    public static GameManager Instance { get => _instance; }


    #endregion

    #endregion

    #region functions

    // called first
    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called seconds
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        camManager = CameraManager.instance;

        // check and assign lobby sceneID
        if (currentScene.name == "LobbySceneTemplateTest")
            lobbyScene = currentScene;

        if (sceneName == "Limbo")
        {
            var tmpButton = GameObject.Find("ReturnToLobbyButton").GetComponent<Button>();
            tmpButton.onClick.AddListener(QuitGame);
            enemyManager = GameObject.Find("EnemyManager");
            if (_offeringData != null)
                InitGameRound();
        }
        //Debug.Log("OnSceneLoaded: " + scene.name);
        if (SoundManager.instance.volumeSlider == null)
            SoundManager.instance.InitSoundManager();
        if (currentScene.name != "MainMenu")
            SetMenus();
    }
    
    private void Awake()
    {
        //pointsText = GameObject.Find("Text_PointsHUD").GetComponent<Text>();
        if (_instance != null)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
    }
    
    void Start () 
    {
        if (SoundManager.instance.volumeSlider == null)
            SoundManager.instance.InitSoundManager();
        //if (!SoundManager.MusicSource.isPlaying)
        //SoundManager.PlayMusicTrack();

    }
	
	void FixedUpdate () 
    {
        if (!Paused && !GameCompleted)
        {
            CurGameTime += Time.deltaTime; 
            //Debug.Log("curGameTime: " + CurGameTime);

            // only have this here to check if time attack mode is completed
            // since I cant have an update loop in a scriptable oject
            
            if (_offeringData != null && InGame)
            {
                if (_offeringData.gameMode == GameMode.timeAttack)
                    _timeText.text = ElapsedTime(Time.time);
                #region testing
                //if (_offeringData.gameMode == GameMode.timeAttack && _offeringData.GameCompleted())
                //{
                //    WonGame();
                //}
                #endregion
            }

        }
    }

    // set game rules
    private void InitGameRound()
    {
        switch (_gameMode)
        {
            case GameMode.randomGunBoxes:
                Debug.Log("init - RandomGunMode");
                if (_timeText != null)
                    _timeText.gameObject.SetActive(false);
                _pointsText = GameObject.Find("Text_PointsHUD").GetComponent<Text>();   
                _pointsText.GetComponentInParent<CanvasGroup>().alpha = 1;
                Points = 0;
                RandomGunMode tmpRand = (RandomGunMode)_offeringData.matchSetting;
                _spawnCrates = tmpRand.SpawnCrates;
                _spawnLocs = tmpRand.spawnLocs;
                _maxPoints = tmpRand.MaxPoints;
                _crate = tmpRand.Crates;
                Invoke("SpawnCrate", 1f);
                break;

            case GameMode.randomGunBoxesShield:
                Debug.Log("init - RandomGunModeEx");
                if (_timeText != null)
                    _timeText.gameObject.SetActive(false);
                _pointsText = GameObject.Find("Text_PointsHUD").GetComponent<Text>();
                _pointsText.GetComponentInParent<CanvasGroup>().alpha = 1;
                Points = 0;
                RandomGunModeExtreme tmpRandEx = (RandomGunModeExtreme)_offeringData.matchSetting;
                _spawnCrates = tmpRandEx.SpawnCrates;
                _spawnLocs = tmpRandEx.spawnLocs;
                _maxPoints = tmpRandEx.MaxPoints;
                _crate = tmpRandEx.Crates;
                Invoke("SpawnCrate", 1f);
                break;

            case GameMode.survival:
                //SurvivalMode tmpSurvival = (SurvivalMode)_offeringData.matchSetting;
                // check if all waves are cleared
                break;
            case GameMode.timeAttack:
                Debug.Log("init - TimeAttacl");
                if (_pointsText != null)
                    _pointsText.gameObject.SetActive(false);

                _timeText = GameObject.Find("Text_TimeHUD").GetComponent<Text>();
                _timeText.GetComponentInParent<CanvasGroup>().alpha = 1;
                TimeAttackMode tmpTime = (TimeAttackMode)_offeringData.matchSetting;
                _startTime = 0;
                _startTime = Time.time;
                _gameTime = 0;
                _gameTime = tmpTime.MatchTime;
                // try to last the longest. earn more times with kills
                
                break;

            case GameMode.highScore:
                //TimeAttackMode tmpTime = (TimeAttackMode)_offeringData.matchSetting;
                // try to reach a certain score by a specific time
                break;

            case GameMode.dashAbilityOnly:
                //DashAbilityMode tmpDash = (DashAbilityMode)_offeringData.matchSetting;
                break;

            case GameMode.meleeOnly:
                //MeleeMode tmpMelee = (MeleeMode)_offeringData.matchSetting;
                break;

            case GameMode.weaponsOnly:
                //MeleeMode tmpMelee = (MeleeMode)_offeringData.matchSetting;
                break;

            default:
                break;
        }
        GameCompleted = false; 

        InGame = true;
    }
  
    private void WonGame()
    {
        GameModeSelected = false;
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            if (RadialMenu.activeInHierarchy)
                RadialMenu.SetActive(false);
        }

        InGame = false;
        GameCompleted = true;
        enemyManager.GetComponentInChildren<EnemySpawner>().enableSpawn = false;
        enemyManager.GetComponentInChildren<EnemySpawner>().DisableSpawner();

        if (_offeringData.gameMode == GameMode.randomGunBoxes)
            _pointsText.GetComponentInParent<CanvasGroup>().alpha = 0;
        


        if (_offeringData.gameMode == GameMode.timeAttack)
            _timeText.GetComponentInParent<CanvasGroup>().alpha = 0;
        

        s_CurGameTime = FormatTime(CurGameTime);
        _offeringResults.totalMatchTime = s_CurGameTime;

        _client = GameWonPanel.GetComponent<IMatchCompletedMenu>();
        _client.SetMatchData(_offeringResults, _offeringData);
        
        GameWonPanel.SetActive(true);
        Invoke("ReturnToLobby", 3);
    }

    public void FailedGame()
    {
        InGame = false;
        GameCompleted = true;
        enemyManager.GetComponentInChildren<EnemySpawner>().enableSpawn = false;
        enemyManager.GetComponentInChildren<EnemySpawner>().DisableSpawner();
        GameFailedPanel.SetActive(true);

        if (_offeringData.gameMode == GameMode.randomGunBoxes)
            _pointsText.GetComponentInParent<CanvasGroup>().alpha = 0;

        if (_offeringData.gameMode == GameMode.timeAttack)
            _timeText.GetComponentInParent<CanvasGroup>().alpha = 0;

        s_CurGameTime = FormatTime(CurGameTime);
        _offeringResults.totalMatchTime = s_CurGameTime;

        _client = GameFailedPanel.GetComponent<IMatchCompletedMenu>();
        _client.SetMatchData(_offeringResults, _offeringData);

        Invoke("ResetGame", 3);
    }

    public void QuitGame()
    {
        InGame = false;
        GameCompleted = true;
        enemyManager.GetComponentInChildren<EnemySpawner>().enableSpawn = false;
        enemyManager.GetComponentInChildren<EnemySpawner>().DisableSpawner();
        GameFailedPanel.SetActive(true);


        if (_offeringData.gameMode == GameMode.randomGunBoxes)
            _pointsText.GetComponentInParent<CanvasGroup>().alpha = 0;

        if (_offeringData.gameMode == GameMode.timeAttack)
            _timeText.GetComponentInParent<CanvasGroup>().alpha = 0;

        s_CurGameTime = FormatTime(CurGameTime);
        _offeringResults.totalMatchTime = s_CurGameTime;

        _client = GameFailedPanel.GetComponent<IMatchCompletedMenu>();
        _client.SetMatchData(_offeringResults, _offeringData);

        Invoke("ReturnToLobby", 3);
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

    public void AddPoint()
    {
        Points++;
        _pointsText.text = Points.ToString();
        if (Points >= _maxPoints)
            WonGame();
        else
            SpawnCrate();
    }

    public void IncrementSilver(int value)
    {
        _offeringResults.totalSilverAccrued += value; 
        TotalSilver += value;
        GameEvents.OnCurrencyUpdateEvent?.Invoke(TotalSilver);
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
        // maybe an enemy that steals some silver during combat
        //_offeringResults.totalSilverAccrued -= value; 
        TotalSilver -= value;
        GameEvents.OnCurrencyUpdateEvent?.Invoke(TotalSilver);
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
                enemyManager.GetComponentInChildren<EnemySpawner>().enableSpawn = true;
                enemyManager.GetComponentInChildren<EnemySpawner>().EnableEnemySpawner();
            }
        }
        if (pauseMenu == null)
             pauseMenu = GameObject.Find("PauseMenuUI");
        pauseMenu.SetActive(false);
    }

    string ElapsedTime(float time)
    {
        float elapsedTime = time - _startTime;
        int minutes = (int)((_gameTime - elapsedTime) / 60) % 60;
        int seconds = (int)((_gameTime - elapsedTime) % 60);
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = String.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);


        if (elapsedTime >= _gameTime)
        {
            //check time to last hihest time???
            WonGame();
        }
        //Debug.Log("elapsedTime: "+ elapsedTime);
        return timeText;
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

    void IGameModeSelectionMenu.SetOfferingData(OfferingData offeringData)
    {
        GameModeSelected = true; // this is only used for offering gate object
        _offeringData = offeringData;
        _gameMode = _offeringData.gameMode;
    }

    #endregion

}

public enum GameMode
{
    randomGunBoxes,
    randomGunBoxesShield,
    survival,
    timeAttack,
    highScore,
    dashAbilityOnly,
    meleeOnly,
    weaponsOnly
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

[System.Serializable]
public struct GameModeModifier
{
    public bool WeaponsEnabled;
    public bool DashAbilityEnabled;
    public bool MeleeEnabled;
    public bool OneHitKO;
    public bool LowGravity;
    public bool NoEnemies;
    public bool SloMo;
}
