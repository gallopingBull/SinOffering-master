using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalPool : MonoBehaviour
{

    public int maxDecals = 100;
    public float decalSizeMin = .5f;
    public float decalSizeMax = 1.5f;

    private ParticleSystem decalParticleSystem; 
    private int partcileDecalDataIndex;
    private ParticleDecalData[] particleData;
    private ParticleSystem.Particle[] particles; 

    // Start is called before the first frame update
    private void Start()
    {
        decalParticleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[maxDecals];
        particleData = new ParticleDecalData[maxDecals];
        for (int i = 0; i < maxDecals; i++)
        {
            particleData[i] = new ParticleDecalData();
        }
    }
    public void ParticleHit(ParticleCollisionEvent particleCollisionEvent)
    {   
        SetParticleData(particleCollisionEvent);
        DisplayParticles();
    }
    private void SetParticleData(ParticleCollisionEvent particleCollisionEvent)
    {
        
        if (partcileDecalDataIndex >= maxDecals)
        {
            partcileDecalDataIndex = 0;
        }
        if (particleCollisionEvent.colliderComponent != null)
        {

            //ignore any collision with player or enemies so 
            //blood splat particles only land on platforms or walls
            if (particleCollisionEvent.colliderComponent.tag == "Player" ||
            particleCollisionEvent.colliderComponent.tag == "Enemy"||
            particleCollisionEvent.colliderComponent.name == "headCollider")
            {
                return;
            }

            //record collision position, rotation, size, and color
            particleData[partcileDecalDataIndex].position = particleCollisionEvent.intersection;

            Vector3 particleRotationEuler = Quaternion.LookRotation(-particleCollisionEvent.normal).eulerAngles;

            particleRotationEuler.z = Random.Range(0, 360f);
            particleData[partcileDecalDataIndex].rotation = particleRotationEuler;


            particleData[partcileDecalDataIndex].size = Random.Range(decalSizeMin, decalSizeMax);

            partcileDecalDataIndex++;

        }
       
    }

    private void DisplayParticles()
    {
        for (int i = 0; i < particleData.Length; i++)
        {
            particles[i].position = particleData[i].position;
            particles[i].rotation3D = particleData[i].rotation;
            particles[i].startSize = particleData[i].size;
        }
        decalParticleSystem.SetParticles(particles, particles.Length);
    }
}
