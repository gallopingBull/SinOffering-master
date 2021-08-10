//Attach this script to a GameObject. Make sure it has a Collider component by clicking the Add Component button. Then click Physics>Box Collider to attach a Box Collider component.
//This script creates a BoxCast in front of the GameObject and outputs a message if another Collider is hit with the Collider’s name.
//It also draws where the ray and BoxCast extends to. Just press the Gizmos button to see it in Play Mode.
//Make sure to have another GameObject with a Collider component for the BoxCast to collide with.

using UnityEngine;

public class BoxCastExample : MonoBehaviour
{
    public float m_MaxDistanceX = 3;        //Choose the distance the Box can reach to+
    public float m_MaxDistanceY = 5;        //Choose the distance the Box can reach to

    public bool m_HitDetect, m_HitDetect2, m_HitDetectY;

    Collider m_Collider;
    public RaycastHit[] m_Hits;

    void Start()
    {
        m_Collider = GetComponent<Collider>();
        m_Hits = new RaycastHit[3];
    }

    void FixedUpdate()
    {
        //Test to see if there is a hit using a BoxCast
        //Calculate using the center of the GameObject's Collider(could also just use the GameObject's position), half the GameObject's size, the direction, the GameObject's rotation, and the maximum distance as variables.
        //Also fetch the hit data
        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, -transform.right, out m_Hits[0], transform.rotation, m_MaxDistanceX);
        m_HitDetect2 = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, transform.right, out m_Hits[1], transform.rotation, m_MaxDistanceX);
        m_HitDetectY = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, transform.up, out m_Hits[2], transform.rotation, m_MaxDistanceY);
        if (m_HitDetect)
        {
            //Output the name of the Collider your Box hit
            //Debug.Log("Hit1 : " + m_Hits[0].collider.name);
            if (m_Hits[0].collider.tag == "Enemy")
            {
                //m_Hits[0].collider.GetComponent<EnemyController>().CanMove = false;
            }
        }

        else if (m_HitDetect2)
        {
            //Output the name of the Collider your Box hit
            //Debug.Log("Hit2 : " + m_Hits[1].collider.name);
            if (m_Hits[1].collider.tag == "Enemy")
            {
                //m_Hits[1].collider.GetComponent<EnemyController>().CanMove = false;
            }
        }
        else if (m_HitDetectY)
        {
            //Output the name of the Collider your Box hit
            //Debug.Log("HitY : " + m_Hits[2].collider.name);
            if (m_Hits[2].collider.tag == "Enemy")
            {
                //m_Hits[2].collider.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0); ;

            }
        }
        else
        {

        }
    }

    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, -transform.right * m_Hits[0].distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + -transform.right * m_Hits[0].distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, -transform.right * m_MaxDistanceX);
           
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + -transform.right * m_MaxDistanceX, transform.localScale);
          
        }

        if (m_HitDetect2)
        {
            Gizmos.DrawRay(transform.position, transform.right * m_Hits[1].distance);
            Gizmos.DrawWireCube(transform.position + transform.right * m_Hits[1].distance, transform.localScale);
        }
        else
        {
            Gizmos.DrawWireCube(transform.position + transform.right * m_MaxDistanceX, transform.localScale);
            Gizmos.DrawRay(transform.position, transform.right * m_MaxDistanceX);
        }

        if (m_HitDetectY)
        {
            Gizmos.DrawRay(transform.position, transform.up * m_Hits[2].distance);
            Gizmos.DrawWireCube(transform.position + transform.up * m_Hits[2].distance, transform.localScale);
        }
        else
        {
            Gizmos.DrawWireCube(transform.position + transform.up * m_MaxDistanceY, transform.localScale);
            Gizmos.DrawRay(transform.position, transform.up * m_MaxDistanceY);
        }
    }
}