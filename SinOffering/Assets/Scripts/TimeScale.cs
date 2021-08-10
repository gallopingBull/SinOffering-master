using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//scales speeds for entities when player activates dash ability
public class TimeScale : MonoBehaviour
{
    public static float player = 1;
    public static float enemies = 1;
    public static float projectiles = 1;
    public static float global = 1;

    public static float time = .25f;
    public float timeTotal = .25f;

    private static List<GameObject> enemiesList; 

    private void Awake()
    {
        enemiesList = new List<GameObject>();
    }

    public static void EnableSlomo()
    {
        player = .75f;
        enemies = 0f;

        /*
         * enemiesList = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>(); ;

        foreach (GameObject enemy in enemiesList)
        {
            enemy.GetComponent<EnemyController>().rb.isKinematic = true;
            enemy.GetComponent<EnemyController>().rb.useGravity = false;
        }*/
    }

    public static void DisableSlomo()
    {
        player = 1f;
        enemies = 1;

        enemiesList = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>(); ;
        /*
        foreach (GameObject enemy in enemiesList)
        {
            enemy.GetComponent<EnemyController>().rb.isKinematic = false;
            enemy.GetComponent<EnemyController>().rb.useGravity = true;
        }
        */
    }
}
