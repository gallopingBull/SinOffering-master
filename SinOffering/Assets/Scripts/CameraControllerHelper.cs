using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControllerHelper : MonoBehaviour
{
    public bool AddTargets = false;
    private CameraManager camManager;

    private void Start()
    {
        camManager = CameraManager.instance;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (AddTargets)
            {
                //col.GetComponent<EnemyController>().InArena = true;
                camManager.AddCameraTargets(col.transform, 1);
            }
        }
    }
}
