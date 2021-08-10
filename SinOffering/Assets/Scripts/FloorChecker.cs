using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorChecker : MonoBehaviour
{
    private PlayerController player;
    private void Awake()
    {
        player = GetComponentInParent<PlayerController>(); 
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            print("is over floor");
            player.IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            print("is over floor");
            //player.Grounded = false;
        }
    }
}
