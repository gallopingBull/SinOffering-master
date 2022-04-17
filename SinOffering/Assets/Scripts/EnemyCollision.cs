using UnityEngine;

/// <summary>
/// helper class for enemy 3D colliders to correctly collide with other 3D colliders in the scene.
/// </summary>

public class EnemyCollision : MonoBehaviour 
{
    private EnemyController _parent;

    private void Awake() { _parent = GetComponentInParent<EnemyController>(); }

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log(gameObject.name + " hit " + col.gameObject.name + " : EnemyController.cs");
        if (col.gameObject.tag == "Wall")
        {
            if (!_parent.facingLeft)
                _parent.facingLeft = true;
            else
                _parent.facingLeft = false;
        }
        //if (col.gameObject.tag == "Bullet")
        //    Debug.Log("enemy hit by projectile");
        if (col.gameObject.tag == "Player" && !_parent.dying)
            col.GetComponent<Entity>().Damaged(1);
    }
}
