using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class GameModeSelectionMenu : MonoBehaviour
{
    private IGameModeSelectionMenu _client = null;
    private Dictionary<int, OfferingData> offeringDatabase;
    //public List<Button> offeringButtons = null;
    public GameObject offeringMenu = null;
    public GameObject offeringButton_Prefab;
    public Image characterSprite; 

    public Transform spawnParent;

    private HUDManager hm;

    [SerializeField]
    private GameMode _gameMode;

    public TextMeshProUGUI offeringTitle_Text,
        offeringSummary_Text,
        offeringType_Text,
        offeringReward_Faith_Text,
        offeringReward_Silver_Text;

    public Image[] offeringGradeImages;

    private void Start()
    {
        offeringDatabase = OfferingDatabase._instance.GetOfferingDatabase();
        hm = HUDManager._instance;
        SetClient();
        InitMenu();
    }

    public void SetClient()
    {
        _client = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // called when button is played
    public void SendGameMode(GameMode gameMode)
    {
        Debug.Log("setting data in GameManager()");
        if (_client == null)
            return;

        Debug.Log("gameMode: " + gameMode.ToString());
        _client.GetGameMode(gameMode);
    }

    public void InitMenu()
    {
        //offeringButtons.Clear();
        int offeringCount = offeringDatabase.Count;
        Debug.Log("offeringCount = " + offeringCount);
        GameObject tmpBut;
        OfferingData tmpData;

        for (int i = 0; i < offeringCount; i++)
        {
            // make new button
            tmpBut = Instantiate(offeringButton_Prefab, spawnParent.transform);

            if (i == 0)
            {
                tmpBut.transform.Find("Button").gameObject.AddComponent<SetSelectedButton>();//AddComponent<SetSelectedButton>();
                //tmpBut.transform.Find("Button").gameObject.GetComponent<SetSelectedButton>().Set_Selected_Button();
            }  

            tmpData = offeringDatabase[i];
            InitButton(tmpBut.GetComponentInChildren<OfferingSelectionButton>(), tmpData);
        }
    }

    public void DisplayOfferingDesciption(OfferingData offeringData)
    {
        offeringSummary_Text.text  = offeringData.offeringSummary;
        offeringType_Text.text = offeringData.gameMode.ToString();
        characterSprite.sprite = offeringData.characterPortrait;

        for (int i = 0; i < offeringData.rewards.Length; i++)
        {
            if (offeringData.rewards[i].rewardType == RewardType.faith)
                offeringReward_Faith_Text.text = offeringData.rewards[i].baseRewardValue.ToString();
            if (offeringData.rewards[i].rewardType == RewardType.silver)
                offeringReward_Silver_Text.text = offeringData.rewards[i].baseRewardValue.ToString();
        }
    }

    public void InitButton(OfferingSelectionButton button, OfferingData _offeringData)
    {
        button.SetData(_offeringData);
        button.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();

        button.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        SendGameMode(_offeringData.gameMode));
    }


    private int GetFirstDigit(int n)
    {
        while (n >= 10)
            n /= 10;
        return n;
    }
}


public interface IGameModeSelectionMenu
{
    void GetGameMode(GameMode gameMode);
    
}

