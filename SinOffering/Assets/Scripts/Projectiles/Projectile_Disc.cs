using System.Collections;
using System.Collections.Generic;
using UnitySpriteCutter;
using UnityEngine;

public class Projectile_Disc : Projectile
{
    private Vector3 startPos;
    private Vector3 pos;

    private int impactCount = 0;
    public float SpeedBoost = 1.5f; //speed mulitplier added to velocity when disc makes impact with wall

    public LayerMask layerMask;
    private Vector2 sliceStart, sliceEnd;
    public Sprite _sliceSprite;


    public float cmShakeTime = .75f;
    public float cmShakeIntensity = 20;

    private void Start()
    {
        startPos = transform.position;
        pos = transform.position;
    }

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (direction == -1)
        {
            pos -= transform.right * Time.fixedDeltaTime * Speed; new Vector3(1, 0, startPos.z);
        }
        //move right
        else
        {
            pos += transform.right * Time.fixedDeltaTime * Speed; new Vector3(1, 0, startPos.z);
        }
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        print("other.name: " + other.name);

        if (other.gameObject.tag == "Enemy")
        {
            print("disc velocity: "+rb.velocity.x);
            if (direction ==1)
            {
                print("moving right");
                GetComponent<ISlice>().DiscSlice(transform.position, transform.position * -direction, layerMask);
            }
            else{
                print("moving left");
                GetComponent<ISlice>().DiscSlice(transform.position, 
                    new Vector2 (transform.position.x -  2, transform.position.y), layerMask);
            }
          
            other.gameObject.GetComponentInParent<Entity>().Killed();
        }

        if (other.gameObject.tag == "Player")
        {
            print("hit player");
            if (impactCount == 0)
            {
                return;
            }
            //Debug.LogError("killing player from cprojectikedisc.cs");
            /*
            if (rb.velocity.x == 13)
            {
                print("moving right");
                GetComponent<ISlice>().Slice(transform.position, transform.position * -direction, layerMask);
            }
            else
            {
                print("moving left");
                GetComponent<ISlice>().Slice(transform.position, new Vector2(transform.position.x - 2, transform.position.y), layerMask);
            }
            other.gameObject.GetComponentInParent<Entity>().Killed();
            */
        }
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Floor")
        {
            print("in floor/wall condition");
            CameraShake.instance.Shake(cmShakeTime, cmShakeIntensity, false);
            impactCount++;
            if (direction == -1)
            {
                direction = 1;

            }
            else { direction = -1; }
            FireProjectile(direction);
            
        }
        if (impactCount == 3)
        {
            CameraShake.instance.Shake(cmShakeTime, cmShakeIntensity, true);
            EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
            DestroyProjectile();
        }
    }

    public override void FireProjectile(int dir)
    {
        direction = dir;
        if (dir == -1)
        {
            transform.localScale = new Vector3(-(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }

        if (impactCount == 0)
        {
            rb.velocity = (transform.right * dir) * Speed;
        }
        else
        {
            rb.velocity = (transform.right * dir) * Speed * 1.5f;
        }   
    }
        
    public override void DestroyProjectile()
    {
        //add impact particle here
        Destroy(gameObject);
    }

    private void KillEnemy()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //Debug.DrawRay(origin, dir, Color.blue);
        //  Gizmos.DrawWireSphere(end, sphereCastRadius);
    }
}
