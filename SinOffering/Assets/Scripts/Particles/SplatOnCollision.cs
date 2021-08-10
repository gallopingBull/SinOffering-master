using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatOnCollision : MonoBehaviour
{
    public ParticleSystem particleLauncher;

    List<ParticleCollisionEvent> collisionEvents; 
    // Start is called before the first frame update
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
