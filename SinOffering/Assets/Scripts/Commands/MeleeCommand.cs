using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class MeleeCommand : ICommand {

    public Transform[] AttackPoints;
    [SerializeField]
    private float attackRange = 1;
    public LayerMask enemyLayer;

    public List<Collider> hitEnemies = new List<Collider>();
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }
    public override void Execute() { MeleeAttack(); }
    public override void Redo()
    {

    }


    private void LateUpdate()
    {
        if (pc.state == Entity.State.meleeing)
        {
            if (pc.dir != 1)
            {
                //Debug.Log("in update");   
                Vector3 tmpPos = pc.WeaponSprite.gameObject.transform.localPosition;
                Vector3 tmpRot = pc.WeaponSprite.gameObject.transform.eulerAngles;
                tmpPos.x *= -1;
                tmpRot.z *= -1;
                //Debug.Log("tmp.x: " + tmpPos.x);
                pc.WeaponSprite.gameObject.transform.localPosition = tmpPos;
                pc.WeaponSprite.gameObject.transform.eulerAngles = tmpRot;
            }
        }
    }

    public void MeleeAttack()
    {
        // return if reached max attack count
       
        if (pc.IsGrounded) //check if not in state jump state
        {
            pc.StateManager.EnterState(Entity.State.meleeing);
    
            for (int i = 0; i < AttackPoints.Length; i++)
                hitEnemies = Physics.OverlapSphere(AttackPoints[i].position, attackRange, enemyLayer).ToList<Collider>();

            if (hitEnemies != null)
            {
                for (int i = 0; i < hitEnemies.Count; i++)
                {
                    Debug.Log("hitemeies["+i+"] - " + "enemy name: " + hitEnemies[i].gameObject.name);
                    if (!hitEnemies[i].gameObject.GetComponent<EnemyController>().dying)
                    {
                        //hitEnemies[i].gameObject.GetComponent<EnemyController>().Damage(.5f);
                        hitEnemies[i].gameObject.GetComponent<EnemyController>().Damage(10f);
                    }       
                }
            }

            hitEnemies.Clear();
            return;
        }

        // melee from falling state
        if (pc.state == Entity.State.falling || pc.state == Entity.State.Jumping)
        {
        
        }
    }

    private void OnDrawGizmos()
    {
        if (AttackPoints[0] == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoints[0].position, attackRange);
        Gizmos.DrawWireSphere(AttackPoints[1].position, attackRange);
    }
}
