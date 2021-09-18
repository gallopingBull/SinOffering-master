using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crates : MonoBehaviour {

    public Weapon[] Weapons;
    public int spawnLoc;
    public AudioClip crateObtainedClip;

    private PlayerController player;
    private GameManager gameManager;

    private int maxWeaponIndex;

    [SerializeField]
    public int EnemyKilledMAX;
    private int curkill;
    private int difference;
    [SerializeField]
    private bool isAlter;

    private int randWeapon;
    private int prevWeapon;


    private void Awake()
    {
        player = PlayerController.instance;
        gameManager = GameManager.instance;
        difference = gameManager.CurEnemyKills;
    }

    private void Start()
    {
        Invoke("AddCrateToCamTargets", .1f);
    }

    private void OnTriggerEnter(Collider col)
    {
        //print("Crates.cs || colliding with: " + col.gameObject.tag);
        if (col.gameObject.tag == "Player")
        {
            if (isAlter)
            {
                curkill = (gameManager.CurEnemyKills - difference);
                if (curkill >= EnemyKilledMAX)
                {
                    SoundManager.PlaySound(crateObtainedClip);
                    gameManager.AddPoint();
                    GenerateWeapon();
                }
            }
            else
            {
                SoundManager.PlaySound(crateObtainedClip);
                gameManager.AddPoint();
                GenerateWeapon();
            }
        }
    }

    private void GenerateWeapon()
    {
        maxWeaponIndex = player.weaponManager.GetComponent<WeaponManager>().Weapons.Length - 1;
        randWeapon = Random.Range(0, maxWeaponIndex);

        while (player.weaponManager.GetComponent<WeaponManager>().CurWeapon == randWeapon)
            randWeapon = Random.Range(0, maxWeaponIndex);

        prevWeapon = player.weaponManager.GetComponent<WeaponManager>().CurWeapon;
        player.weaponManager.GetComponent<WeaponManager>().EquipWeapon(randWeapon);
        Destroy(gameObject);
    }

    private void AddCrateToCamTargets()
    {
        gameManager.camManager.AddCameraTargets(gameObject.transform, 30f);
    }
}
