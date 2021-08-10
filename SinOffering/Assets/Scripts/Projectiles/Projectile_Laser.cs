using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Laser : Projectile
{
    public GameObject explosionSphere;
    public LineRenderer lr;

    protected override void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        if (GetComponentInParent<Weapon_Laser>().LaserEnabled)
        {
            print(GetComponentInParent<Weapon_Laser>().spawnLoc.transform);
            print("parent.dir: " + GetComponentInParent<PlayerController>().dir);
            lr.SetPosition(0, GetComponentInParent<Weapon_Laser>().spawnLoc.transform.position);
            RaycastHit hit;
            if (GetComponentInParent<PlayerController>().dir == 1)
            {
                if (Physics.Raycast(GetComponentInParent<Weapon_Laser>().spawnLoc.transform.position,
               (transform.right), out hit))
                {
                    if (hit.collider)
                    {
                        if (hit.transform.tag != "Player") {
                            //print("hit.collider: ");
                            print("hit.collider: " + hit.collider.name);
                            lr.SetPosition(1, hit.point);
                            CheckForEnemyCollision(hit.collider, hit.point);
                        }
                        else
                        {
                            print("not hitting anything facinf right");
                            lr.SetPosition(1, (transform.right ) * 5000);
                        }
                    }
                    //if (hit.transform.name == "Player") { return; }
                }

            }
            if (GetComponentInParent<PlayerController>().dir == -1)
            {
                if (Physics.Raycast(GetComponentInParent<Weapon_Laser>().spawnLoc.transform.position,
                (transform.right * -1f), out hit))
                {
                    if (hit.collider)
                    {
                        if (hit.transform.tag != "Player")
                        {
                            //print("hit.collider: ");
                            print("hit.collider: " + hit.collider.name);
                            lr.SetPosition(1, hit.point);
                            CheckForEnemyCollision(hit.collider, hit.point);
                        }
                        else
                        {
                            print("not hitting anything facing left");
                            lr.SetPosition(1, (transform.right *-1) * 5000);
                        }
                    }
                }
            }

            else
            {
                print("not hitting anything");
                lr.SetPosition(1, (transform.right * GetComponentInParent<PlayerController>().dir) * 5000);
            }
        }
    }

    private void CheckForEnemyCollision(Collider other, Vector3 hit)
    {
        //print("laser hit: " + other.gameObject.name);
        //if (other.gameObject.tag == "Bullet") { }
        //if (other.gameObject.tag == "Player") { }
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponentInParent<Entity>().Damage(DamageAmmount);
            EnableImpactParticle(GetComponent<Transform>(), other.gameObject.tag);
        }

        GameObject tmpTransform = new GameObject() ;
        tmpTransform.transform.position = hit;
        EnableImpactParticle(tmpTransform.transform, other.gameObject.tag);
        Destroy(tmpTransform);
    }
}
