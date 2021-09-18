using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterShield : MonoBehaviour
{
    GameManager gameManager;

    private int curkill;
    private int difference;
    private int killRequired;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameManager.instance;
        killRequired = GetComponentInParent<Crates>().EnemyKilledMAX;
        difference = gameManager.CurEnemyKills;
    }

    private void Update()
    {
        curkill = (gameManager.CurEnemyKills - difference);
        if (curkill >= killRequired)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        //print("AlterShield.cs ||colliding with: " + col.gameObject.name);
        if (col.gameObject.name == "Player")
        {
            if (curkill < killRequired)
            {
                //print("kill player");
                Debug.LogError("killing player from altershiekd.cs");
                col.gameObject.GetComponent<PlayerController>().Killed();
            }
        }
    }
}
