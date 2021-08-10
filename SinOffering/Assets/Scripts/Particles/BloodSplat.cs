using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplat : MonoBehaviour
{
    private ParticleSystem particle;
    public GameObject BloodSplat_Prefab;
    public Transform BloodSplat_Transform;
    private Transform parent;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private AudioSource source;
    public AudioClip[] sounds;

    public float SoundCapResetSpeed = .55f;
    public int MaxSounds = 3;

    private float timePassed;
    private int soundsPlayed;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
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
    public void AssignParent(Transform _parent)
    {
        parent = _parent; 
        
    }
    /*
    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);
        int count = collisionEvents.Count;
        if (parent != null)
        {
            for (int i = 0; i < count; i++)
            {
                //print("splashing blood on: " + other.gsameObject.name);
                BloodSplat_Transform = other.gameObject.transform;
                //
                print(parent.name + " splashing blood on: " + other.gameObject.name);
                if (other.gameObject.tag == "Blood")
                {
                    print("touching blood");
                    continue;
                }

                if (other.gameObject.tag == "Player" ||
                    other.gameObject.tag == "Enemy")
                {
                    if (other.gameObject.transform == parent)
                    {
                        print("touching parent");
                        //continue;
                    }
                    GameObject tmp = Instantiate(BloodSplat_Prefab, collisionEvents[i].intersection, Quaternion.Euler(0, 0, Random.Range(0, 360.0f)), BloodSplat_Transform);
                }


                if (other.gameObject.tag == "Wall" ||
                    other.gameObject.tag == "Floor")
                {
                    var hitRotation = Quaternion.FromToRotation(Vector3.up, collisionEvents[i].normal);
                    //print("splashing blood on: " + other.gameObject.tag);
                    GameObject tmp = Instantiate(BloodSplat_Prefab, collisionEvents[i].intersection, hitRotation, BloodSplat_Transform);

                    tmp.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                }


                if (soundsPlayed < MaxSounds)
                {
                    //soundsPlayed++;
                    //source.pitch = Random.Range(0.9f, 1.1f);
                    //source.PlayOneShot(sounds[Random.Range(0, sounds.Length)], Random.Range(.1f,.35f));
                }
            }
        }
    }*/
}
