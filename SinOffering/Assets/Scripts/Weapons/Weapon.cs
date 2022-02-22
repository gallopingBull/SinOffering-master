using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    #region variables

    // these should be moved to 
    private bool _weaponPurchased;
    private bool _secondaryFire;

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
    public SpriteRenderer BloodSprite;

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

    // muzzle direction (change varible names to "muzzles")
    public GameObject spawnLoc;
    public GameObject SpawnLocL, SpawnLocR;

    // Variables for weapon spread
    //public bool EnableSpread; // whether or not the weapon has any projectile spread
    public float MinSpread, MaxSpread;

    public AudioClip fireClip;

    // weapon recoil that is applied to player when firing
    protected Recoil Recoil;

    // intensity and duration of camera shake when gun
    // is firing
    public float DurationCamShake, AmmountCamShake;
    public bool SmoothTransition = false;

    [SerializeField]
    protected WeaponAttributes weaponAttributes = new WeaponAttributes();

    public WeaponAttributes WeaponAttributes { get => weaponAttributes; set => weaponAttributes = value; }
    #endregion

    #region functions

    protected virtual void Awake()
    {
        pc = PlayerController.instance;
        weaponManager = pc.weaponManager;
        postProcessManager = PostProcessManager.intance;
        Recoil = GetComponent<Recoil>();
    }
    // might switch out fixedupdates for update
    protected virtual void Update()
    {
    }
    protected virtual void FixedUpdate()
    {
        FireRateCheck();
    }

    public virtual void FireWeapon()
    {
        if (!UnlimitedAmmo && Ammo > 0)
            Ammo--;

        if (!pc.inputHandler.aiming)
            SetSpawnLoc();

        canFire = false;
        nextFire = fireRate;

        SoundManager.PlaySound(fireClip); // move this into weapon subclasses for more specific behavior

        int tmp = pc.dir;

        if (pc.inputHandler.aiming)
            tmp = pc.inputHandler._aimDir;
        SpawnProjectile(tmp);
        EnableMuzzleFX();
        EnableMuzzleLight();
        CameraShake.instance.Shake(DurationCamShake, AmmountCamShake, SmoothTransition);
    }

    public void SetSpawnLoc()
    {
        spawnLoc = GetMuzzleDirection();
    }

    public virtual void ReleaseTrigger()
    {
        // this is a test funtion for Weapon_Gattling.cs
        // will use other weapon instances
    }

    public virtual void ReloadWeapon()
    {
        Ammo = MaxAmmo;
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    // only use when game hasnt loaded any data
    // weird work around will find a better way to check this conditon
    public void SetTempAttributeData()
    {
        gameObject.SetActive(true);
        
        if (weaponAttributes.weaponName == "")
            weaponAttributes.SetUPWeaponAttribute(weaponName);

        gameObject.SetActive(false);
    }

    private void EnableMuzzleLight()
    {
        if (pc.dir == 1)
            MuzzleFireLightCaller(R_MuzzleFire_Light);
        else
            MuzzleFireLightCaller(L_MuzzleFire_Light);
    }

    //turn this into a protected abstract function so eacg
    protected abstract void SpawnProjectile(int dir);

    public virtual void InitWeapon()
    {
        MaxAmmo = Ammo;
        FlipWeaponSprite(pc.dir);
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

    public void ResetPosition(int aimDirection)
    {
        int tmpDir = pc.dir;

        // if weapon not facing correcg direction
        if (tmpDir != aimDirection)
            FlipWeaponSprite(tmpDir);
        MoveWeaponToSocket(tmpDir);
        
        transform.rotation = 
            Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
    }
        
    public virtual void FlipWeaponSprite(int dir)
    {
        if (dir == 1)
        {
            WeaponSprite.flipX = false;
            if (BloodSprite != null)
                BloodSprite.flipX = false;
        }
        else
        {
            WeaponSprite.flipX = true;
            if (BloodSprite != null)
                BloodSprite.flipX = true;
        }
            
        MoveWeaponToSocket(dir);
    }
    protected virtual void MoveWeaponToSocket(int dir)
    { 
        // player facing right

        if (dir == 1)
            WeaponSprite.transform.position = weaponManager.RHandSocket.position;
        else
            WeaponSprite.transform.position = weaponManager.LHandSocket.position;

    }
    
    // rescales weapoon on x-axis
    protected void CalculateMuzzleDirection()
    {
        var tmpScale = pc.dir * Vector3.right;
        tmpScale.y = gameObject.transform.localScale.y;
        tmpScale.z = gameObject.transform.localScale.z;

        gameObject.transform.localScale = tmpScale;

        float xVal = pc.dir * gameObject.transform.localPosition.x;
        var tmpPos = gameObject.transform.localPosition;
        tmpPos.x = xVal;

        gameObject.transform.localPosition = tmpPos;
    }
    protected GameObject GetMuzzleDirection()
    {
        int tmp = pc.dir;

        if (pc.inputHandler.aiming)
            tmp = pc.inputHandler._aimDir;
        if (tmp == 1)
            return SpawnLocR;
        else
            return SpawnLocL;
    }

    private void EnableMuzzleFX()
    {
        int tmp = pc.dir;

        if (pc.inputHandler.aiming)
            tmp = pc.inputHandler._aimDir;
        if (tmp == 1)
            MuzzleFire_R_Particle.Play();
        else
            MuzzleFire_L_Particle.Play();
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

    // call this when weapon will change player's move/jump speed when it's equipped
    protected void ModifyEntitySpeed(float jumpModVal, float runModVal)
    {
        pc.JumpSpeed *= jumpModVal;
        pc.Speed *= runModVal;
    }

    // should this be put in Playercontroller.cs instead? 
    // avoid having to have multiple update() in weapon.cs
    // since the player can be checked if they can shoot
    protected void FireRateCheck()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
            return;
        }
        else
            canFire = true;
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
        return Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + angle + spread));
    }

    #endregion
}
