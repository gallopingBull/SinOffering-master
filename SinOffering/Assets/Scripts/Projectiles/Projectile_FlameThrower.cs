using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FlameThrower : Projectile
{

    public GameObject explosionSphere;
    public List<ParticleCollisionEvent> collisionEvents;
    private ParticleSystem ps; 


    protected override void Awake() {
        ps = GetComponent<ParticleSystem>(); 
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        //print("in onparticlecollision()");
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            //print("fire from: " + transform.parent.name + "collided with: " + other.name);
            if (other.tag == "Enemy")
            {
                other.GetComponent<EnemyController>().EnableFire();
                //print("light enemy on fire");
            }
        }
    }

    //using on particle collison instead - keep this empty to override
    //previous method.
    protected override void OnTriggerEnter(Collider other) { }

    public override void DestroyProjectile() { Destroy(gameObject); }

}
