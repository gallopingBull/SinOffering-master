using UnityEngine;

public class Weapon_Shotgun : Weapon {

    public int MaxShells = 8;

    protected override void SpawnProjectile(int dir)
    {
        GameObject tmpProjectile;
        Quaternion bulletRot;
        for (int i = 0; i < MaxShells; i++)
        {
            bulletRot = CalculateSpread();
            tmpProjectile =
                Instantiate(ProjectilePrefab,
                spawnLoc.transform.position,
                bulletRot);
            tmpProjectile.GetComponent<Projectile>().FireProjectile(dir);

        }
        Recoil.WeaponRecoil();
    }
}
