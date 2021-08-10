using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterShield : MonoBehaviour
{
    private PlayerController pc;

    private int curkill;
    private int difference;
    private int killRequired;
    // Start is called before the first frame update
    void Awake()
    {
        pc = PlayerController.instance;
        killRequired = GetComponentInParent<Crates>().EnemyKilledMAX;
        difference = pc.CurEnemyKills;
    }

    private void Update()
    {
        curkill = (pc.CurEnemyKills - difference);
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
