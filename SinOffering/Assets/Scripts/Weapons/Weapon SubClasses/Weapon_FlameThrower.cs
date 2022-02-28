using UnityEngine;

public class Weapon_FlameThrower : Weapon {
    private float ogYRot; 

    protected override void Awake()
    {
        ogYRot = ProjectilePrefab.transform.rotation.y;
        pc = PlayerController.instance;
        Recoil = GetComponent<Recoil>();
    }
    protected override void FixedUpdate()
    {
        FireRateCheck();
        if (Input.GetButtonUp("Fire1") &&
            ProjectilePrefab.GetComponent<ParticleSystem>().isPlaying)
            ProjectilePrefab.GetComponent<ParticleSystem>().Stop();
    }

    public override void FireWeapon()
    {
        //spawnLoc = GetMuzzleDirection();

        canFire = false;
        nextFire = fireRate;

        SoundManager.PlaySound(fireClip); // move this into weapon subclasses for more specific behavior

        SpawnProjectile(1);//activate particle here instead

        CameraShake.instance.Shake(DurationCamShake,
                     AmmountCamShake, SmoothTransition);
    }

    protected override void MoveWeaponToSocket()
    {
        if (ProjectilePrefab.transform.rotation.y != ogYRot)
            ProjectilePrefab.transform.Rotate(0, 180, 0, Space.Self);
         else
            ProjectilePrefab.transform.Rotate(0, -180, 0, Space.Self);
        
        
        transform.position = pc.weaponManager.mainHandSocket.position;
        //i don't like hard coding this negative value here... change it in some time
        //print(ProjectilePrefab.transform.rotation.y);
        //ProjectilePrefab.transform.position = GetMuzzleDirection().transform.position;
    }

    protected override void SpawnProjectile(int dir)
    { 
        if (!ProjectilePrefab.GetComponent<ParticleSystem>().isPlaying)
            ProjectilePrefab.GetComponent<ParticleSystem>().Play();
    }
}
