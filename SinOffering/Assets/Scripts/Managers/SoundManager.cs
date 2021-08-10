using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SoundManager : MonoBehaviour
{
    [HideInInspector]
    public static SoundManager instance;
    [HideInInspector]
    public static AudioSource source;
    [SerializeField]
    public static AudioSource MusicSource;
    [HideInInspector]
    public static AudioSource SFXSource;


    private string sceneName;

    public GameObject audioPanel;
    public Slider volumeSlider;
    public Slider sfxSlider;

    public TextMeshProUGUI volumeText;
    public TextMeshProUGUI sfxText;

    //there volume variables are set by
    //PlayerSettings Class
    public static float musicVolume = 75; 
    public static float sfxVolume = 75;

    void Awake()
    {
        if (instance == null) 
            instance = this;

        //DontDestroyOnLoad(instance.gameObject); //might keep this, but need to figure out how to extend this into
        //other scenes correctly 
        MusicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        SFXSource = GetComponent<AudioSource>();
        instance.audioPanel = GameObject.Find("Panel_AudioSettings");
        sceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    { 
        InitSoundTexts();
    }

    public static void PlaySound(AudioClip sound)
    {
        SFXSource.PlayOneShot(sound);
    }

    public static void PlayMusicTrack()
    {
        MusicSource.Play();
    }

    public void SetMusicVolumeSlider()
    {
        musicVolume = volumeSlider.value;
        sfxVolume = sfxSlider.value;
        volumeText.text = (Mathf.Round(musicVolume * 100)).ToString();
        sfxText.text = (Mathf.Round(sfxVolume * 100)).ToString();

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
        SFXSource.volume = sfxVolume;
        MusicSource.volume = musicVolume;
    }

    //this is called from PlayerSettings() when game is loading 
    //player settings
    public void SetVolume(float musicVol, float sfxVol) 
    {
        musicVolume = musicVol;
        sfxVolume = sfxVol;

        instance.volumeText.text = musicVolume.ToString();
        instance.sfxText.text = sfxVolume.ToString();

        instance.volumeSlider.value = musicVolume;
        instance.sfxSlider.value = sfxVol;

        MusicSource.volume = musicVol;
        SFXSource.volume = sfxVol;
    }

    private void InitSoundTexts()
    {
        instance.volumeSlider = GameObject.Find("Slider_MusicVolume").GetComponent<Slider>();
        instance.sfxSlider = GameObject.Find("Slider_SFXVolume").GetComponent<Slider>();

        instance.volumeText = GameObject.Find("Text_MusicVolumeVal").GetComponent<TextMeshProUGUI>();
        instance.sfxText = GameObject.Find("Text_SFXVal").GetComponent<TextMeshProUGUI>();

        instance.audioPanel.SetActive(false);
        if (sceneName != "MainMenu")    
        {
            GameObject.Find("Panel_PauseMainMenu").SetActive(true);
            GameManager.instance.pauseMenu.SetActive(false);
        }
    
        //after assignincall playersettings.cs to update slider/text values with saved 
        GameObject.Find("Main Camera").GetComponent<PlayerSettings>().InitSound();
            
    }
}
