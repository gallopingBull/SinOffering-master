using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_GattlingGun : Weapon 
{
    #region variables
    public float JumpSpeedModifier;
    public float RunSpeedModifier;

    public int curChargeTime, desiredTime; // time to heat up gun before it can begin firing
    private bool weaponCharged;
    public bool weaponReady;
    public bool IsFiring;

    public bool IsLocked;
    public float CoolDownTime; 

    public float curFireTime, MaxFireTime; //time before gun overheats and stops firing

    //[HideInInspector]
    public AudioSource source; //this is a temp audio source to play sfx of weapons
                               //being charged/ revved up
    public AudioClip WeaponChargeSound;
    public AudioClip WeaponCoolingSound;
    public AudioClip WeaponReadySound;
    public AudioClip WeaponAlarmSound;

    public ParticleSystem ps;
    #endregion
    #region functions
    public override void InitWeapon()
    {
        MaxAmmo = Ammo;
        curChargeTime = 0;
        curFireTime = 0;
        weaponReady = true;
        weaponCharged = false;
        canFire = false;
        FlipWeaponSprite(pc.dir);
        ModifyEntitySpeed(JumpSpeedModifier, RunSpeedModifier);
    }

    protected override void MoveWeaponToSocket(int dir)
    {
        if (dir == 1)
            transform.position = pc.weaponManager.LHandSocket.position;
        else
            transform.position = pc.weaponManager.RHandSocket.position;
    }

    private void Update()
    {
        if ((Input.GetButton("Fire1") || Input.GetAxis("RightTrigger") > 0 )&& weaponReady)
        {
            //playsound when weapon is beign charged up
            if (!source.isPlaying && !weaponCharged)
                source.PlayOneShot(WeaponChargeSound);

            if (!IsFiring) { curChargeTime++; }

            //handles firing
            if (curChargeTime >= desiredTime)
            {
                if (!IsFiring) { IsFiring = true; }
                if (!MuzzleFire_L_Particle.isPlaying)
                {
                    //MuzzleFire_Particle.Play();
                }
                if (source.isPlaying) { source.Stop(); }
                if (!weaponCharged) { weaponCharged = true; }
                curFireTime++;
                FireRateCheck();

                //telgraph weapon is overheating.
                if (curFireTime >= (MaxFireTime * .5f))
                {
                    if (!ps.isPlaying)
                    {
                        source.PlayOneShot(WeaponAlarmSound);//activate warning sound
                        ps.Play();//activate muzzle particle
                    }
                }

                //weapon overheated, start cooling down.
                if (curFireTime > MaxFireTime)
                    StartCoroutine(LockUpWeapon());
            }
        }

        else
        {
            //if trigger released before weapon starts firing
            if (!IsFiring)
            {
                if (curChargeTime <= 0)
                    weaponCharged = false;
                else
                    curChargeTime--;
                //print("curChargeTime: " + curChargeTime);
            }
        }
    }
  
    public override void ReleaseTrigger()
    {
        if (weaponReady)
        {
            curFireTime = 0;
            IsFiring = false;
        }
        if (MuzzleFire_L_Particle.isPlaying || MuzzleFire_R_Particle.isPlaying)
        {
            MuzzleFire_L_Particle.Stop();
            MuzzleFire_R_Particle.Stop();
            ps.Stop();
        }
    }

    //gattling gun locks up and is forced into cool down 
    private IEnumerator  LockUpWeapon()
    {
        if (!IsLocked)
        {
            IsLocked = true;
            IsFiring = false;
            if (MuzzleFire_L_Particle.isPlaying || MuzzleFire_R_Particle.isPlaying)
            {
                MuzzleFire_L_Particle.Stop();
                MuzzleFire_R_Particle.Stop(); 
            }
            if (!ps.isStopped) { ps.Stop(); }

            weaponReady = false;
            weaponCharged = false;
            canFire = false;

            if (source.isPlaying) { source.Stop(); }

            curChargeTime = 0;
            curFireTime = 0;

            yield return new WaitForSeconds(CoolDownTime);

            //gattling gun ready to use
            weaponReady = true;
            source.PlayOneShot(WeaponReadySound);

            IsLocked = false;
            StopCoroutine(LockUpWeapon());
        }
    }

    protected override void SpawnProjectile(int dir)
    {
        GameObject tmpProjectile;
        Quaternion bulletRot;
        bulletRot = CalculateSpread();
        tmpProjectile =
            Instantiate(ProjectilePrefab,
            spawnLoc.transform.position,
            bulletRot);
        tmpProjectile.GetComponent<Projectile>().FireProjectile(dir);
        GetComponent<Recoil>().WeaponRecoil();
    }
    #endregion
}
