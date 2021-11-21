using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// behavior for gate doors to slide open/close along x-axis.
/// </summary>

public class GateBehavior : MonoBehaviour
{
    // references to door child-objects
    public GameObject lDoor, rDoor;

    [SerializeField]
    private bool _openDoor = false;

    [Tooltip("Speed value determines how fast door opens/closes.")]
    public float speed;
    [Tooltip("Distance doors show be offset from on Z-axis.")]
    public float offSetDistance;

    private Vector3 l_originPos, r_originPos;
    private Vector3 l_targetPos, r_targetPos;
  
    private void Start()
    {
        l_originPos = lDoor.transform.position;
        r_originPos = rDoor.transform.position;

        l_targetPos = SetHorizontalTargetPosition(l_originPos, offSetDistance);
        r_targetPos = SetHorizontalTargetPosition(r_originPos, -offSetDistance);
    }
        
    void Update()
    {
        if (_openDoor)
        {
            if (lDoor.transform.position != l_targetPos ||
                rDoor.transform.position != r_targetPos)
            {
                lDoor.transform.position = Vector3.MoveTowards(lDoor.transform.position, l_targetPos, speed * Time.deltaTime);
                rDoor.transform.position = Vector3.MoveTowards(rDoor.transform.position, r_targetPos, speed * Time.deltaTime);
            }
        }

        else
        {
            if (lDoor.transform.position != l_originPos ||
                rDoor.transform.position != r_originPos)
            {
                lDoor.transform.position = Vector3.MoveTowards(lDoor.transform.position, l_originPos, speed * Time.deltaTime);
                rDoor.transform.position = Vector3.MoveTowards(rDoor.transform.position, r_originPos, speed * Time.deltaTime);
            }
        }
    }

    private Vector3 SetHorizontalTargetPosition(Vector3 origin, float distance)
    {
        Vector3 tmpPos = origin;
        tmpPos.z = tmpPos.z + distance;
        return tmpPos;
    }

    public void OpenGate()
    {
        Debug.Log("OpenGate");
        PlayerController.instance.rb.velocity = Vector3.zero;
        PlayerController.instance.InputEnabled = true;

        _openDoor = true;
    }
    public void CloseGate()
    {
        Debug.Log("CloseGate");
        _openDoor = false;
        PlayerController.instance.InputEnabled = true;
    }

    public void Toggle()
    {
        if (_openDoor)
            CloseGate();
        else
            OpenGate();
    }
}
