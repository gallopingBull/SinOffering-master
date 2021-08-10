using System.Collections;
using UnityEngine;

public class Weapon_DualPistols : Weapon {
    public override void InitWeapon() {
        MaxAmmo = Ammo;
    }
    public override void FlipWeapon(int dir) { }

    public override void FireWeapon()
    {
        if (Ammo > 0 && !UnlimitedAmmo)
        {
            canFire = false;
            nextFire = fireRate;

            SoundManager.PlaySound(fireClip); // move this into weapon subclasses for more specific behavior
            SpawnProjectile(pc.dir);
            MuzzleFire_L_Particle.Play();
            MuzzleFire_R_Particle.Play();
            MuzzleFireLightCaller(L_MuzzleFire_Light);
            MuzzleFireLightCaller(R_MuzzleFire_Light);

            Ammo--;
            CameraShake.instance.Shake(DurationCamShake,
                         AmmountCamShake, SmoothTransition);
        }
    }

    protected override IEnumerator MuzzleFireLight()
    {

        float tmpIntensity = Random.Range(.5f, 1);
        float tmpColor = Random.Range(100, 125);
        muzzleFire_Light.GetComponent<Light>().intensity = tmpIntensity;
        muzzleFire_Light.GetComponent<Light>().color = new Color(255, tmpColor, 0);
        muzzleFire_Light.gameObject.SetActive(true);
        yield return new WaitForSeconds(.005f);
        muzzleFire_Light.gameObject.SetActive(false);
        L_MuzzleFire_Light.gameObject.SetActive(false);
        R_MuzzleFire_Light.gameObject.SetActive(false);

        StopCoroutine("MuzzleFireLight");
    }

    protected override void SpawnProjectile(int dir)
    {
        GameObject tmpProjectile;
        tmpProjectile =
                    Instantiate(ProjectilePrefab,
                        SpawnLocR.transform.position,
                        SpawnLocR.transform.rotation);
        tmpProjectile.GetComponent<Projectile>().FireProjectile(1);

        tmpProjectile =
                   Instantiate(ProjectilePrefab,
                       SpawnLocL.transform.position,
                       SpawnLocL.transform.rotation);
        tmpProjectile.GetComponent<Projectile>().FireProjectile(-1);
    }
}
