using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightTarget : MonoBehaviour
{

    Transform target; 
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.Find("Player").transform; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(target);
    }
}
