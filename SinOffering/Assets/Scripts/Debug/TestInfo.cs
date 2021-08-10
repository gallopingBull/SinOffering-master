using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TestInfo : MonoBehaviour {

    public GameObject DebuggerCanvas;
    public Text stateText, velocity, point_Text, curWeapon_text, 
        grounded_text, jumpEnabled_Text, jumpCount_Text, dashCount_Text;

    private GameObject player;
    private GameManager gm;
    public int TargetFrameRate = 60;

    float deltaTime = 0.0f;

    public static bool EnableDebugInfo = true;

    // Use this for initialization
    void Start () {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFrameRate;
        player = GameObject.Find("Player");
        gm = GameManager.instance;
    }
    //This displays testing data 
    private void DisplayPlayerControllerData()
    {
        if (gm == null)
        {
            return;
        }
        if (!gm.gameCompleted && player != null)
        {
            stateText.text = player.GetComponent<PlayerController>().state.ToString();
            velocity.text = player.GetComponent<Entity>().rb.velocity.ToString();
            point_Text.text = gm.points.ToString();
            grounded_text.text = player.GetComponent<PlayerController>().IsGrounded.ToString();
            jumpEnabled_Text.text = player.GetComponent<PlayerController>().jumpEnabled.ToString();
            jumpCount_Text.text = player.GetComponent<PlayerController>().jumpCount.ToString();
            dashCount_Text.text = player.GetComponent<DashCommand>().dashCount.ToString();

            if (player.GetComponent<PlayerController>().EquippedWeapon != null)
            {
                curWeapon_text.text = player.GetComponent<PlayerController>().EquippedWeapon.GetComponent<Weapon>().GetWeaponName();
            }
            else
            {
                curWeapon_text.text = "no weapon";
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (EnableDebugInfo)
            DisplayPlayerControllerData();

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (Input.GetKeyDown(KeyCode.R) ||
        Input.GetKeyDown("joystick button 6"))
            GetComponent<LoadScene>().LoadSceneByIndex(0);

        if (Input.GetKeyDown("joystick button 6") ||
            Input.GetKeyDown(KeyCode.Tab))
        {
            if (EnableDebugInfo == true)
            {
                EnableDebugInfo = false;
                if (DebuggerCanvas.activeSelf)
                    DebuggerCanvas.SetActive(false);
            }
            else
            {
                EnableDebugInfo = true;
                if (!DebuggerCanvas.activeSelf)
                {
                    DebuggerCanvas.SetActive(true);
                    DisplayPlayerControllerData();
                }
            }
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        if (EnableDebugInfo)
        {
            int w = Screen.width, h = Screen.height;

            Rect rect = new Rect(0, 0, w, h * 2 / 60);
            style.alignment = TextAnchor.UpperRight;
            style.fontSize = h * 2 / 60;

            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            if (fps < 30)
            {
                style.normal.textColor = Color.yellow;
            }
            else
            {
                if (fps < 10)
                {
                    style.normal.textColor = Color.red;
                }
                else
                {
                    style.normal.textColor = Color.green;
                }
            }

            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
        else
        {
            style.normal.textColor = Color.clear; 
        }
    }
}
