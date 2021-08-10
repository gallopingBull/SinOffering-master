using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int points; // points player currently has
    public int MaxPoints = 10;

    public GameObject GameWonPanel, GameFailedPanel, pauseMenu;

    public bool paused; 
    public bool SpawnCrates = false; 
    //crate spawn  locations
    public Transform[] spawnLocs;
    private int lastSpawnLoc;
    public GameObject Crate;


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
    public float TotalGameTime;

    #endregion

    private void Awake()
    {
        pointsText = GameObject.Find("Text_PointsHUD").GetComponent<Text>();
        instance = this;
    }
    // Use this for initialization
    void Start () {
        camManager = CameraManager.instance;
        /*
        if (!SoundManager.MusicSource.isPlaying) {

            SoundManager.PlayMusicTrack();
        }*/

        if (SpawnCrates)
        {
            Invoke("SpawnCrate", 1f);
        }
            
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (points == MaxPoints)
        {
           WonGame();
        }
	}

    private void WonGame()
    {
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            if (RadialMenu.activeInHierarchy)
            {
                RadialMenu.SetActive(false);
            }
        }
      
        gameCompleted = true;
        
        GameWonPanel.SetActive(true);
        Invoke("ResetGame", 3);
    }

    public void FailedGame()
    {
        gameCompleted = true;
        GameFailedPanel.SetActive(true);
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

    private void SpawnCrate()
    {

        int locIndex = Random.Range(0, spawnLocs.Length);

        while (locIndex == lastSpawnLoc)
        {
            locIndex = Random.Range(0, spawnLocs.Length);
        }
        //print("localIndex: " + locIndex);
        Instantiate(Crate, spawnLocs[locIndex].position, spawnLocs[locIndex].rotation);
 
        lastSpawnLoc = locIndex;
    }
}
