using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision2D : MonoBehaviour {

    EnemyController parent;

    private void Awake() { parent = GetComponentInParent<EnemyController>(); }

  

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(gameObject.name + " hit " + col.gameObject.name + " : EnemyController.cs");
        if (col.gameObject.tag == "Wall")
        {
            if (!parent.facingLeft)
            {
                parent.facingLeft = true;
            }
            else
            {
                parent.facingLeft = false;
            }
        }
        if (col.gameObject.tag == "Bullet")
        {
            //print("enemy hit by projectile");
            //Killed();
        }
        if (col.gameObject.tag == "Player" && !parent.dying)
        {
            col.GetComponent<Entity>().Damage(1);
        }
    }
}
