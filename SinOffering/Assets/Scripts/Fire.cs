using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public List<ParticleCollisionEvent> collisionEvents;
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        //print("in onparticlecollision()");
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            //print("fire from: " + transform.name + "collided with: " + other.name);
            if (other.tag == "Enemy")
            {
                other.GetComponent<EnemyController>().EnableFire();
                //print("light enemy on fire");
            }
        }
    }
}
