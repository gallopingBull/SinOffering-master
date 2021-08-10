using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour
{
    public ParticleSystem particleLauncher;
    public ParticleDecalPool splatDecalPool;

    public ParticleSystem splatterParticles;
    private List<ParticleCollisionEvent> collisionEvents;
    
    //these variables are used for the splat masks used
    //only on player and enemies
  
    public MaskDecalPool maskDecalPool;


    // Start is called before the first frame update
    void Start()
    {
        splatterParticles = GameObject.Find("BloodSplat_Impact_FX").GetComponent<ParticleSystem>();
        splatDecalPool = GameObject.Find("SplatterBloodDecalParticlesPool").GetComponent<ParticleDecalPool>();
        maskDecalPool = GameObject.Find("BloodSplatMask_PoolLauncher").GetComponent<MaskDecalPool>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {   
        if (collisionEvents == null)
            return;
            
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);
                
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            if (collisionEvents[i].colliderComponent != null)
            {

                if ((other.gameObject.tag == "Player" ||
                    other.gameObject.tag == "Enemy") &&
                    other.gameObject.tag != "Blood")
                {
                    maskDecalPool.ParticleHit(collisionEvents[i]);
                    EmitAtLocation(collisionEvents[i]);
                }
                else
                {
                    //print("hitting something else");
                    splatDecalPool.ParticleHit(collisionEvents[i]);
                    EmitAtLocation(collisionEvents[i]);
                }
                maskDecalPool.SplatSound();
            }
        }

    }
    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatterParticles.transform.position = particleCollisionEvent.intersection;
        splatterParticles.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
        
        splatterParticles.Emit(1);
    }
}
