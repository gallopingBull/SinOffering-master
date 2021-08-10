using UnityEngine;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        //print(col.gameObject.name);
        if (col.gameObject.tag == "Player")
        {
            //Debug.LogError("killing player from killzone.cs");

            col.GetComponent<Entity>().Killed();
        }
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponentInParent<EnemyController>().Suicide();
        }
        else
        {
            
        }
    }
}

