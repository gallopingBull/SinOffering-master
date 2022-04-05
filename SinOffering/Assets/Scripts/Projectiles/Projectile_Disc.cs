using UnityEngine;

/// <summary>
/// derived projectile class that handles projectile behavior
/// for the disc launcher weapon.
/// </summary>

public class Projectile_Disc : Projectile
{
    private Vector3 _pos;

    private int _impactCount = 0;
    [SerializeField] int _maxImpacts = 3;

    public float SpeedBoost = 1.5f; //speed mulitplier added to velocity when disc makes impact with wall

    #region test variables for sprite cutter
    public LayerMask layerMask;
    //private Vector2 sliceStart, sliceEnd;
    public Sprite _sliceSprite;
    #endregion

    public float cmShakeTime = .75f;
    public float cmShakeIntensity = 20;

    private void Start()
    {
        _pos = transform.position;
    }

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (direction == -1)
            _pos -= transform.right * Time.fixedDeltaTime * Speed;
        //move right
        else
            _pos += transform.right * Time.fixedDeltaTime * Speed;
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
                GetComponent<SpriteCutter>().Slice(transform.position, transform.position * -direction, layerMask);
            }
            else
            {
                print("moving left");
                GetComponent<SpriteCutter>().Slice(transform.position, 
                    new Vector2 (transform.position.x -  2, transform.position.y), layerMask);
            }
          
            other.gameObject.GetComponentInParent<Entity>().Killed();
        }

        if (other.gameObject.tag == "Player")
        {
            print("hit player");
            if (_impactCount == 0)
                return;
        }

        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Floor")
        {
            print("in floor/wall condition");
            CameraShake.instance.Shake(cmShakeTime, cmShakeIntensity, false);
            _impactCount++;
            if (direction == -1)
                direction = 1;
            else
                direction = -1;
            FireProjectile(direction);
        }
        
        if (_impactCount == _maxImpacts)
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

        if (_impactCount == 0)
            rb.velocity = (transform.right * dir) * Speed;
        else
            rb.velocity = (transform.right * dir) * Speed * 1.5f;
    }
        
    public override void DestroyProjectile()
    {
        //add impact particle here
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Debug.DrawRay(origin, dir, Color.blue);
        //  Gizmos.DrawWireSphere(end, sphereCastRadius);
    }
}
