using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Projectile : MonoBehaviour
{
    protected Rigidbody rb;
    protected int direction;

    public float Speed = 1000f;
    [HideInInspector]
    public float DamageAmmount = 1; 


    public bool IsPistol, IsShotGun, IsMachineGun, isLaser;
    public float LifeTime = 3f; //.1 - .5f for shotgun
    public GameObject impactParticle;
    public GameObject enemyimpactParticle;


    // Start is called before the first frame update

    //public SpriteRenderer ProjectileSprite; 
    

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (IsShotGun)
            LifeTime = Random.Range(.01f, .4f);
        Invoke("DestroyProjectile", LifeTime);
    }

    public virtual void FireProjectile(int dir)
    {
        direction = dir;
        if (dir == -1)
        {
            transform.localScale = new Vector3(-(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }
        rb.velocity = (transform.right * dir) * Speed;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isLaser)
        {   //if (other.gameObject.tag == "Bullet") { }
            //if (other.gameObject.tag == "Player") { }
            //print("other: " + other.name);
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponentInParent<Entity>().Damage(DamageAmmount);
                other.gameObject.GetComponentInParent<RecoilTest>().WeaponRecoil(direction);   

                EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
                DestroyProjectile();
                return;
            }
            if (other.gameObject.tag == "Wall")
            {

                EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
                DestroyProjectile();
                return;
            }
            if (other.gameObject.tag == "Floor")
            {
                EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
                DestroyProjectile();
                return;
            }
        }
    }



    protected void EnableImpactParticle(Transform hitLoc, string tag)
    {
        Transform tmpTransform = hitLoc;
        GameObject tmpParticle;

        switch (tag)
        {
            case "Wall":
                if (direction == 1)
                {
                    tmpTransform.position = new Vector3(GetComponent<Transform>().position.x - .25f, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);
                    tmpTransform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    tmpTransform.position = new Vector3(GetComponent<Transform>().position.x + .25f, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);
                    tmpTransform.rotation = new Quaternion(0, -90, 0, 0);
                }
                tmpParticle = Instantiate(impactParticle, tmpTransform.position, tmpTransform.rotation);
                break;
            case "Floor":
                tmpParticle = Instantiate(impactParticle, tmpTransform.position, tmpTransform.rotation);
                var shape = tmpParticle.GetComponent<ParticleSystem>().shape;
                shape.rotation = new Vector3(-90f, 0, 0);
                break;
            case "Enemy":
                if (direction == 1)
                {
                    tmpTransform.position = new Vector3(GetComponent<Transform>().position.x - .25f, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);
                    tmpTransform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    tmpTransform.position = new Vector3(GetComponent<Transform>().position.x + .25f, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);
                    tmpTransform.rotation = new Quaternion(0, -90, 0, 0);
                }
                tmpParticle = Instantiate(enemyimpactParticle, tmpTransform.position, tmpTransform.rotation);
                break;
            default:
                break;
        }
    }


    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
