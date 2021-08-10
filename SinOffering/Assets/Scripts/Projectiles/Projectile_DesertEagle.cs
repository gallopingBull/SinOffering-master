using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_DesertEagle : Projectile
{


    protected override void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            //other.gameObject.GetComponentInParent<Entity>().Killed();
            other.gameObject.GetComponentInParent<EnemyController>().Explode();
            EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
            DestroyProjectile();
        }
        if (other.gameObject.tag == "Wall")
        {

            EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
            DestroyProjectile();
        }
        if (other.gameObject.tag == "Floor")
        {
            EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
            DestroyProjectile();
        }
    }
}
