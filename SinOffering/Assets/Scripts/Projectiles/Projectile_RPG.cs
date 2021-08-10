using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_RPG : Projectile
{
    private Vector3 startPos;

    float frequency = 20f; 
    float magnitude = .29f;
    Vector3 pos; 

    public GameObject explosionSphere;
    public GameObject ps;
    public ParticleSystem _ps;

    public float cmShakeTime = .75f;
    public float cmShakeIntensity = 20;

    private bool blownUp = false; 

    private void Start()
    {
        Invoke("EnableSmoke",.1f);
        startPos = transform.position;
        pos = transform.position;
        //Invoke("DestroyProjectile", LifeTime);
    }
    private void EnableSmoke()
    {
        _ps.Play();
    }

    private void FixedUpdate()
    {
        if (direction == -1)
        {
            pos -= transform.right * Time.deltaTime * Speed; new Vector3(1, 0, startPos.z);
        }
        //move right
        else
        {
            pos += transform.right * Time.deltaTime * Speed; new Vector3(1, 0, startPos.z);
        }
        transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("missile hit: " + other.gameObject);
        if (!blownUp)
        {
            if (other.gameObject.tag == "Enemy")
            {
                blownUp = true;
                other.gameObject.GetComponentInParent<Entity>().Damage(DamageAmmount);
                other.gameObject.GetComponentInParent<RecoilTest>().WeaponRecoil(direction);

                EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
                DestroyProjectile();
                return;
            }
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Floor")
            {
                blownUp = true;
                EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
                DestroyProjectile();
            }
        }
        
    }

    private void Explode()
    {
        GameObject tmp;
        CameraShake.instance.Shake(cmShakeTime, cmShakeIntensity, true);
        tmp = Instantiate(explosionSphere,
           transform.position,
           transform.rotation);
        tmp.GetComponent<DustTrail>().EnableTrail();
    }
    public override void DestroyProjectile()
    {
        Explode();
        ps.GetComponent<RocketTrail>().StopTrail();
        ps.transform.parent = null;
        Destroy(gameObject);
    }
}
