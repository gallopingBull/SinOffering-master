using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //these should be moved to 
    private bool _weaponPurchased;
    private bool _secondaryFire;

    #region variables
    [HideInInspector]
    public enum WeaponTypes
    {
        Dual_Pistol, 
        Shotgun,
        RPG,
        GattlingGun,
        DiscLauncher,
    }

    [SerializeField]
    protected string weaponName;
    protected WeaponTypes weaponType;
    
    public SpriteRenderer WeaponSprite;

    public ParticleSystem MuzzleFire_L_Particle;
    public ParticleSystem MuzzleFire_R_Particle;

    protected GameObject muzzleFire_Light; 
    public GameObject L_MuzzleFire_Light;
    public GameObject R_MuzzleFire_Light;
    
    public int Ammo = 15;
    protected int MaxAmmo;
    public bool UnlimitedAmmo;

    public float fireRate = 10f;
    public bool canFire = false;
    protected float nextFire = -1f;

    protected PlayerController pc; // reference to player controller
    protected WeaponManager weaponManager;
    protected PostProcessManager postProcessManager; 

    public GameObject ProjectilePrefab;

    //muzzle direction (change varible names to "muzzles")
    public GameObject spawnLoc;
    public GameObject SpawnLocL, SpawnLocR;

    //Variables for weapon spread
    //public bool EnableSpread; //whether or not the weapon has any projectile spread
    public float MinSpread, MaxSpread;

    public AudioClip fireClip;

    //weapon recoil that is applied to player when firing
    protected Recoil Recoil;

    //intensity and duration of camera shake when gun
    //is firing
    public float DurationCamShake, AmmountCamShake;
    public bool SmoothTransition = false;

    [SerializeField]
    protected WeaponAttributes weaponAttributes = new WeaponAttributes();

    public WeaponAttributes WeaponAttributes { get => weaponAttributes; set => weaponAttributes = value; }
    #endregion

    #region functions
    public virtual void FireWeapon()
    {
        if (!UnlimitedAmmo)
        {
            if (Ammo > 0)
            {
                print("FireWeapon() called in Weapons.cs");
                spawnLoc = GetMuzzleDirection();

                canFire = false;
                nextFire = fireRate;

                SoundManager.PlaySound(fireClip); // move this into weapon subclasses for more specific behavior
                SpawnProjectile(pc.dir);
                PPMCaller();//adjust post process fx
                EnableMuzzleFX();
                if (muzzleFire_Light == null)
                {
                    if (pc.dir == 1)
                    {
                        MuzzleFireLightCaller(R_MuzzleFire_Light);
                    }
                    else
                    {
                        MuzzleFireLightCaller(L_MuzzleFire_Light);
                    }
                }
                else
                {
                    if (pc.dir == 1)
                    {
                        MuzzleFireLightCaller(R_MuzzleFire_Light);
                    }
                    else
                    {
                        MuzzleFireLightCaller(L_MuzzleFire_Light);
                    }
                }
                Ammo--; 
                CameraShake.instance.Shake(DurationCamShake,
                             AmmountCamShake, SmoothTransition);
            }
        }
        else
        {
            spawnLoc = GetMuzzleDirection();

            canFire = false;
            nextFire = fireRate;

            SoundManager.PlaySound(fireClip); // move this into weapon subclasses for more specific behavior
            SpawnProjectile(pc.dir);
            EnableMuzzleFX();
            if (muzzleFire_Light == null)
            {
                if (pc.dir == 1)
                {
                    MuzzleFireLightCaller(R_MuzzleFire_Light);
                }
                else
                {
                    MuzzleFireLightCaller(L_MuzzleFire_Light);
                }
            }
            else
            {
                if (pc.dir == 1)
                {
                    MuzzleFireLightCaller(R_MuzzleFire_Light);
                }
                else
                {
                    MuzzleFireLightCaller(L_MuzzleFire_Light);
                }
            }

            CameraShake.instance.Shake(DurationCamShake,
                         AmmountCamShake, SmoothTransition);
        }
    }

    public virtual void ReleaseTrigger()
    {
        //this is a test funtion for Weapon_Gattling.cs
        //will use other weapon instances
    }

    public virtual void ReloadWeapon()
    {
        Ammo = MaxAmmo;
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    public virtual void FlipWeapon(int dir)
    {
        if (dir == 1)
        {
            WeaponSprite.flipX = false;
        }
        else
        {
            WeaponSprite.flipX = true;
        }
        MoveToSocket(pc.dir);
    }

    protected virtual void Awake()
    {   
        pc = PlayerController.instance;
        weaponManager = pc.weaponManager;
        postProcessManager = PostProcessManager.intance;
        Recoil = GetComponent<Recoil>(); 
    }

    //onlyt use when game hasnt loaded any data/
    //weird work around will find a better way to check this conditon
    public void SetTempAttributeData()
    {
        gameObject.SetActive(true);
        
        if (weaponAttributes.weaponName == "")
        {
            weaponAttributes.SetUPWeaponAttribute(weaponName);
        }

        gameObject.SetActive(false);
    }

    //might switch out fixedupdates for update
    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
        FireRateCheck();
    }


    //turn this into a protected abstract function so eacg
    protected abstract void SpawnProjectile(int dir);


    public virtual void InitWeapon()
    {
        MaxAmmo = Ammo;
        FlipWeapon(pc.dir);
    }


    public void SetWeaponAttributeFields()
    {
        Debug.Log("set weapon attrbutes field being called");
        _weaponPurchased = weaponAttributes.WeaponPurchased;
        _secondaryFire = weaponAttributes.SecondaryFire;
        MaxAmmo = weaponAttributes.MaxAmmo;
        Ammo = MaxAmmo;
        fireRate = weaponAttributes.fireRate;
        ProjectilePrefab.GetComponent<Projectile>().DamageAmmount = weaponAttributes.WeaponDamage;
    }
    protected virtual void MoveToSocket(int dir)
    {
        /*
        if (muzzleFire_Light != null)
        {
            muzzleFire_Light.transform.position = GetMuzzleDirection().transform.position;
            Vector3 tmpPos = new Vector3(muzzleFire_Light.transform.position.x, MuzzleFire_Light.transform.position.y, MuzzleFire_Light.transform.position.z - .25f);
            muzzleFire_Light.transform.position = tmpPos;
        } */

        //print("MuzzleFire_Particle.transform.position: "+ MuzzleFire_Particle.transform.position);
        //player facing right
        if (dir == 1)
        {
            WeaponSprite.transform.position = weaponManager.RHandSocket.position;
        }
        //player facing left
        else
        {
            WeaponSprite.transform.position =weaponManager.LHandSocket.position;
        }
    }

    protected GameObject GetMuzzleDirection()
    {
        if (pc.dir == 1)
            return SpawnLocR;
        else
            return SpawnLocL;
    }

    private void EnableMuzzleFX()
    {
        if (pc.dir == 1)
        {
            MuzzleFire_R_Particle.Play();
        }
        else
        {
            MuzzleFire_L_Particle.Play();
        }
    }

    protected void MuzzleFireLightCaller(GameObject light)
    {
        muzzleFire_Light = light;
        StartCoroutine("MuzzleFireLight");
    }

    protected virtual IEnumerator MuzzleFireLight()
    {

        float tmpIntensity = Random.Range(.5f, 1);
        float tmpColor = Random.Range(100, 125);
        muzzleFire_Light.GetComponent<Light>().intensity = tmpIntensity;
        muzzleFire_Light.GetComponent<Light>().color = new Color(255, tmpColor, 0);
        muzzleFire_Light.gameObject.SetActive(true);
        yield return new WaitForSeconds(.005f);
        muzzleFire_Light.gameObject.SetActive(false);
        StopCoroutine("MuzzleFireLight");
    }

    //call this when weapon will change player's move/jump speed when it's equipped
    protected void ModifyEntitySpeed(float jumpModVal, float runModVal)
    {
        pc.JumpSpeed *= jumpModVal;
        pc.Speed *= runModVal;
    }

    //should this be put in Playercontroller.cs instead? 
    //avoid having to have multiple update() in weapon.cs
    //sinve th eplayer can be checked if they can shoot
    protected void FireRateCheck()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
            return;
        }
        else
        {
            canFire = true;
        }
    }

    protected virtual void PPMCaller()
    {
        postProcessManager.OnFire(.5f, 1, true);
    }

    protected Quaternion CalculateSpread()
    {
        Vector3 randDir = (Vector3.right).normalized;
        float angle = Mathf.Atan2(randDir.y, randDir.x) * Mathf.Rad2Deg;
        float spread = Random.Range(MinSpread, MaxSpread);
        return Quaternion.Euler(new Vector3(0, 0, angle + spread));
    }


    #endregion
}
