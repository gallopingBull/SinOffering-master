using UnityEngine;
public class MeleeCommand : ICommand {



    public Transform AttackPoint;
    [SerializeField]
    private float attackRange = 1;
    public LayerMask enemyLayer;


    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }
    public override void Execute() { MeleeAttack(); }
    public override void Redo()
    {

    }
    
    public void MeleeAttack()
    {
        // return if reached max attack count
       
        // jump from ground 
        if (pc.IsGrounded) //check if not in state jump state
        {
            pc.StateManager.EnterState(Entity.State.meleeing);
            Collider[] hitEnemies = Physics.OverlapSphere(AttackPoint.position, attackRange, enemyLayer);
            for (int i = 0; i < hitEnemies.Length; i++)
            {

            }

            return;
        }

        // jump from falling state
        if (pc.state == Entity.State.falling || pc.state == Entity.State.Jumping)
        {
        
        }
    }

    private void OnDrawGizmos()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawSphere(AttackPoint.position, attackRange);
        
    }
}
