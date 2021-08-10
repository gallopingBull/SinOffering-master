using UnityEngine;

public class Weapon_SMG : Weapon {

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
    }
}
