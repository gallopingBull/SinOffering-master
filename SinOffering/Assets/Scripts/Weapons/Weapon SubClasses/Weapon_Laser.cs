using UnityEngine;

public class Weapon_Laser : Weapon {

    public float JumpSpeedModifier = 1;
    public float RunSpeedModifier = 1;

    public bool LaserEnabled;

    public LineRenderer lr;
    private int direction; 

    public override void InitWeapon()
    {
        FlipWeapon(pc.dir);
        ModifyEntitySpeed(JumpSpeedModifier, RunSpeedModifier);
    }
    public override void FireWeapon()
    {
        if (spawnLoc != GetMuzzleDirection())
            spawnLoc = GetMuzzleDirection();

        canFire = true;
        if (!LaserEnabled)
            LaserEnabled = true;

        SoundManager.PlaySound(fireClip); // move this into weapon subclasses for more specific behavior
        SpawnProjectile(pc.dir);
        Recoil.WeaponRecoil();
        CameraShake.instance.Shake(DurationCamShake,
                     AmmountCamShake, SmoothTransition);
    }

    protected override void MoveToSocket(int dir)
    {
        //print("moce to socket called in weapoin_laser");
        if (dir == 1)
        {
            lr.enabled = false;
            WeaponSprite.transform.position = pc.weaponManager.GetComponent<WeaponManager>().LHandSocket.position;
        }
        else
        {
            lr.enabled = false;
            WeaponSprite.transform.position = pc.weaponManager.GetComponent<WeaponManager>().RHandSocket.position;
        }
        if (!lr.enabled)
            lr.enabled = true;

        lr.transform.position = GetMuzzleDirection().transform.position;
    }

    protected override void FixedUpdate()
    {
        FireRateCheck();
        if (Input.GetButtonUp("Fire1"))
            ReleaseTrigger();
    }

    public override void ReleaseTrigger()
    {
        LaserEnabled = false;
        canFire = false;
        lr.gameObject.SetActive(false);
    }

    protected override void SpawnProjectile(int dir)
    {
        lr.gameObject.SetActive(true);
        LaserEnabled = true;
    }
}
