using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crates : MonoBehaviour {

    public Weapon[] Weapons;
    public int spawnLoc;
    public AudioClip crateObtainedClip;

    private PlayerController player;
    private GameManager gm;

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
        gm = GameManager.instance;
        difference = player.CurEnemyKills;
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
                curkill = (player.CurEnemyKills - difference);
                if (curkill >= EnemyKilledMAX)
                {
                    SoundManager.PlaySound(crateObtainedClip);
                    gm.AddPoint();
                    GenerateWeapon();
                }
            }
            else
            {
                SoundManager.PlaySound(crateObtainedClip);
                gm.AddPoint();
                GenerateWeapon();
            }
        }
    }

    private void GenerateWeapon()
    {
        maxWeaponIndex = player.weaponManager.GetComponent<WeaponManager>().Weapons.Length - 1;
        randWeapon = Random.Range(0, maxWeaponIndex);

        while (player.weaponManager.GetComponent<WeaponManager>().CurWeapon == randWeapon)
        {
            randWeapon = Random.Range(0, maxWeaponIndex);
        }
        prevWeapon = player.weaponManager.GetComponent<WeaponManager>().CurWeapon;
        player.weaponManager.GetComponent<WeaponManager>().EquipWeapon(randWeapon);
        Destroy(gameObject);
    }

    private void AddCrateToCamTargets()
    {

        gm.camManager.AddCameraTargets(gameObject.transform, 30f);

        /*
        if (MultiTargetCam.instance.enabled &&
            MultiTargetCam.instance.AddCrates)
        {
            MultiTargetCam.instance.targets.Add(gameObject.transform);
        }
        */
    }

    /*private Weapon GenerateWeapon()
    {
        return weapon = Weapons[Random.Range(0, Weapons.Length)]; 
    }*/
}
