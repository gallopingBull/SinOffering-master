using UnityEngine;

public class Weapon_RPG : Weapon {

    public float JumpSpeedModifier;
    public float RunSpeedModifier;

    public override void InitWeapon()
    {
        MaxAmmo = Ammo;
        FlipWeapon(pc.dir);
        ModifyEntitySpeed(JumpSpeedModifier, RunSpeedModifier);
    }

    protected override void MoveToSocket(int dir)
    {
        /*
        if (muzzleFire_Light != null)
        {
            muzzleFire_Light.transform.position = GetMuzzleDirection().transform.position;
            Vector3 tmpPos = new Vector3(muzzleFire_Light.transform.position.x, MuzzleFire_Light.transform.position.y, MuzzleFire_Light.transform.position.z - 1);
            muzzleFire_Light.transform.position = tmpPos;
        }*/

        //MuzzleFire_Particle.transform.position = GetMuzzleDirection().transform.position;
        //player facing right
        if (dir == 1)
        {
            //two handed weapons
            //move weapon to opposite hand socket
            WeaponSprite.transform.position = pc.weaponManager.GetComponent<WeaponManager>().LHandSocket.position;
        }
        else
        {
            WeaponSprite.transform.position = pc.weaponManager.GetComponent<WeaponManager>().RHandSocket.position;
        }
    }

    protected override void SpawnProjectile(int dir)
    {
        GameObject tmpProjectile;
        tmpProjectile =
            Instantiate(ProjectilePrefab,
            spawnLoc.transform.position,
             spawnLoc.transform.rotation);
        tmpProjectile.GetComponent<Projectile_RPG>().FireProjectile(dir);
        GetComponent<Recoil>().WeaponRecoil();
    }
}
