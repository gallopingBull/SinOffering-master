using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// class displays performance, player, and game data for testing purposes only.
/// </summary>

public class TestInfo : MonoBehaviour
{
    public GameObject DebuggerCanvas;
    public Text stateText, velocity, point_Text, curWeapon_text,
        grounded_text, jumpEnabled_Text, jumpCount_Text, dashCount_Text;

    private PlayerController _player;
    private GameManager _gm;
    public int TargetFrameRate = 60;

    private float _deltaTime = 0.0f;

    public static bool EnableDebugInfo = true;

    // Use this for initialization
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFrameRate;
        _player = PlayerController.instance;
        _gm = GameManager.Instance;
    }
    
    //This displays testing data 
    private void DisplayPlayerControllerData()
    {
        if (_gm == null)
            return;
        if (!_gm.GameCompleted && _player != null)
        {
            stateText.text = _player.state.ToString();
            velocity.text = _player.rb.velocity.ToString();
            point_Text.text = _gm.Points.ToString();
            grounded_text.text = _player.IsGrounded.ToString();
            jumpEnabled_Text.text = _player.jumpEnabled.ToString();
            jumpCount_Text.text = _player.jumpCount.ToString();
            dashCount_Text.text = _player.GetComponent<DashCommand>().dashCount.ToString();

            if (_player.EquippedWeapon != null)
                curWeapon_text.text = _player.EquippedWeapon.GetComponent<Weapon>().GetWeaponName();
            else
                curWeapon_text.text = "no weapon";

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EnableDebugInfo)
            DisplayPlayerControllerData();

        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

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

            float msec = _deltaTime * 1000.0f;
            float fps = 1.0f / _deltaTime;
            if (fps < 30)
                style.normal.textColor = Color.yellow;
            else
            {
                if (fps < 10)
                    style.normal.textColor = Color.red;
                else
                    style.normal.textColor = Color.green;
            }

            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
        else
            style.normal.textColor = Color.clear;
    }
}
