using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour {
    public float explosionLifeTime = .5f;
    public LayerMask layerMask;
    private bool canKill = true;
    public List<GameObject> targets;
    public Vector3 impactPoint;
    public Vector3 targetPos;


    private void Start()
    {
        print("exploded");
        impactPoint = transform.position; 
        targets = new List<GameObject>();
        Invoke("DestroyCollider", explosionLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("other: "+ other.name);
        //if (other.gameObject.tag == "Bullet") { }
        //if (other.gameObject.tag == "Player") { }
        if (other.gameObject.tag == "Enemy" )
        {
            //other.gameObject.GetComponentInParent<Entity>().Explode();
            if (!other.gameObject.GetComponentInParent<EnemyController>().dying && canKill)
            {
                //other.gameObject.GetComponentInParent<EnemyController>().Explode();
                targets.Add(other.gameObject);
            }

        }
    }

    private void DestroyCollider()
    {
        canKill = false;
        if (targets.Count == 0)
        {
            return;    
        }
        /*
        RaycastHit2D[] hits = Physics2D.RaycastAll(impactPoint, targetPos, layerMask);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject tmp = hit.transform.gameObject;
            print(tmp.name + " add to gameObjectsToCut<>");
        }*/

        
        foreach (GameObject target in targets.ToList())
        {
            //shoot raycast line between impact point and target positions
            targetPos = target.transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(impactPoint, targetPos - impactPoint, 10f, layerMask);

            foreach(RaycastHit2D hit in hits)
            {
                //check if line hits wall/floor/platform between impact point and target
                if (hit.collider != null)
                {
                    print("hit = " + hit.transform.gameObject);
                    if (hit.transform.gameObject.tag == "Wall" ||
                   hit.transform.gameObject.tag == "Floor" ||
                   hit.transform.gameObject.tag == "Platform")
                    {
                        break;
                    }

                    if(hit.transform.gameObject.tag == "Enemy")
                    {
                        print("hit = false");
                        target.GetComponentInParent<EnemyController>().Explode();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Debug.DrawRay(impactPoint, targetPos.normalized, Color.blue);
        Gizmos.DrawWireSphere(impactPoint, .7f);
        Gizmos.DrawWireSphere(targetPos, .7f);

    }
}
