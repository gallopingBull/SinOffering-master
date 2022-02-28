using UnityEngine;

public class Weapon_RPG : Weapon {

    public float JumpSpeedModifier;
    public float RunSpeedModifier;

    public override void InitWeapon()
    {
        MaxAmmo = Ammo;
        FlipWeaponSprite(pc.dir);
        ModifyEntitySpeed(JumpSpeedModifier, RunSpeedModifier);
    }

    /*
    protected override void MoveWeaponToSocket()
    {
        var tmp = pc.weaponManager.mainHandSocket.localPosition;
        transform.localPosition= new Vector3(-tmp.x, tmp.y, tmp.z);
    }*/

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
