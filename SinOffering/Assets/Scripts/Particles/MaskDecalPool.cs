using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskDecalPool : MonoBehaviour
{
    public static MaskDecalPool instance; 

    public int maxMasks = 100;
    public float maskSizeMin = .5f;
    public float maskSizeMax = 1.5f;

    private int maskDataIndex;
    private MasksData[] masksData;

    public GameObject BloodSplat_Mask_Prefab;
    private GameObject[] masks;


    //splat sound variables
    private AudioSource source;
    public AudioClip[] sounds;


    public float SoundCapResetSpeed = .55f;
    public int MaxSounds = 3;

    private float timePassed;
    private int soundsPlayed;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        source = SoundManager.SFXSource;
        masksData = new MasksData[maxMasks];
        masks = new GameObject[maxMasks];

        Vector3 tmpPos = new Vector3(100, 100, 100);

        for (int i = 0; i < maxMasks; i++)
        {
            masksData[i] = new MasksData(); 
            masks[i] = Instantiate(BloodSplat_Mask_Prefab,
                        tmpPos,
                        transform.rotation);
            masks[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > SoundCapResetSpeed)
        {
            soundsPlayed = 0;
            timePassed = 0;
        }
    }

    public void ParticleHit(ParticleCollisionEvent particleCollisionEvent)
    {
        if (particleCollisionEvent.colliderComponent != null)
        {
            if (particleCollisionEvent.colliderComponent.tag == "Player" ||
            particleCollisionEvent.colliderComponent.tag == "Enemy" ||
            particleCollisionEvent.colliderComponent.name == "headCollider")
            {               
                if (particleCollisionEvent.colliderComponent.GetComponent<Entity>().BloodCount <
                    particleCollisionEvent.colliderComponent.GetComponent<Entity>().MaxBloodMasks)
                {
                    SetParticleData(particleCollisionEvent);
                    DisplayMasks(particleCollisionEvent.colliderComponent.transform.gameObject);
                }
            }
        }
    }
    private void SetParticleData(ParticleCollisionEvent particleCollisionEvent)
    {
        if (maskDataIndex >= maxMasks)
            maskDataIndex = 0;


        //("maskDataIndex: "+ maskDataIndex);
        //record collision position, rotation, size, and color
        masksData[maskDataIndex].position = particleCollisionEvent.intersection;

        Vector3 particleRotationEuler = particleCollisionEvent.colliderComponent.transform.rotation.eulerAngles;

        particleRotationEuler.z = Random.Range(0, 360f);
        masksData[maskDataIndex].rotation = particleRotationEuler;

        masksData[maskDataIndex].size = Random.Range(maskSizeMin, maskSizeMax);

        maskDataIndex++;
    }

    private void DisplayMasks(GameObject _parent)
    {
        for (int i = 0; i < masks.Length; i++)
        {
            if (_parent == null)
                return;

            if (masks[i] != null && 
                masks[i].activeSelf)
            {
                continue;
            }

            if (_parent.GetComponent<Entity>().BloodCount < _parent.GetComponent<Entity>().MaxBloodMasks)
            {
                masks[i].transform.position = masksData[i].position;
                masks[i].transform.parent = _parent.transform;
                //masks[i].transform.rotation = masksData[i].rotation;

                Vector3 tmpSize = new Vector3(masksData[i].size, masksData[i].size, masksData[i].size);
                masks[i].transform.localScale = tmpSize;

                _parent.GetComponent<Entity>().BloodCount++;
                SplatSound();
                masks[i].SetActive(true);
                break;
            }
            else
            {

            }
        }
    }

    public void SplatSound()
    {
        if (soundsPlayed < MaxSounds)
        {
            soundsPlayed++;
            source.pitch = Random.Range(0.9f, 1.1f);
            source.PlayOneShot(sounds[Random.Range(0, sounds.Length)], Random.Range(.1f,.35f));
        }
    }

    public void DeParentMasks(GameObject mask)
    {
        mask.transform.parent.GetComponent<Entity>().BloodCount--;

        mask.transform.parent = null;

        StartCoroutine(LerpFunction(mask));
      
        
    }

    IEnumerator LerpFunction(GameObject mask)
    {
        if(mask == null)
            StopCoroutine(LerpFunction(mask));

        float time = 0;
        float startValue = mask.GetComponent<SpriteMask>().alphaCutoff;




        while (time < 3)
        {
            mask.GetComponent<SpriteMask>().alphaCutoff = Mathf.Lerp(startValue, 1, time / 3);
            time += Time.deltaTime;
            yield return null;
        }
        mask.GetComponent<SpriteMask>().alphaCutoff = 1;

        while (mask.transform.localScale.x > 0)
        {
            mask.transform.localScale += new Vector3(-.01f, -.01f, -.01f);
            yield return null;
        }

        mask.transform.localScale = Vector3.zero;
        mask.SetActive(false);
     
        StopCoroutine(LerpFunction(mask));
    }
}
