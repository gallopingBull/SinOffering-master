using UnityEngine;
public class MeleeCommand : ICommand {

    public Transform[] AttackPoints;
    [SerializeField]
    private float attackRange = 1;
    public LayerMask enemyLayer;

    Collider[] hitEnemies = new Collider[2];
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
                hitEnemies = Physics.OverlapSphere(AttackPoints[i].position, attackRange, enemyLayer);

            if (hitEnemies != null)
            {
                for (int i = 0; i < hitEnemies.Length; i++)
                    if (!hitEnemies[i].gameObject.GetComponent<EnemyController>().dying)
                        hitEnemies[i].gameObject.GetComponent<EnemyController>().Damage(10);
            }
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
