using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTargetCam : MonoBehaviour
{

    public bool CamEnabled = false;
    public bool AddCrates = false;
    public List<Transform> targets;
    public List<Transform> targetsDashAttack;


    public Vector3 offset;

    private Vector3 velocity;
    public float smooothTime = .5f;
    private float zPos = 0;

    [Tooltip("keep over 50 to zoom out for player/crates/enemies")]
    public float minZoom = 40f; //keep over 50 to zoom out for player/crates/enemies 
    [Tooltip("keep between 30 and 40 to zoom in for player only")]
    public float maxZoom = 10f; //keep between 30 and 40 to zoom in for player only

    public float zoomLimiter;

    private Camera cam;
    // Update is called once per frame

    [HideInInspector]
    public static MultiTargetCam instance;

    //[HideInInspector]
    private PlayerController pc;
    //[HideInInspector]
    public GameObject playerDashMarker;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cam = GetComponent<Camera>();
        pc = PlayerController.instance;
        playerDashMarker = pc.gameObject.GetComponent<DashCommand>().dashDestinationImage;
        //PlayerController.instance.Test();
    }

    void LateUpdate()
    {
        if (enabled)
        {
            //print("enabled");
            //camera should only move when player is still active in scene
            if (pc != null)
            {
                if (targets != null)
                {
                    //print("set camerea targets");
                    SetCameraTargets();
                }
                SetCameraLocation();
                SetCameraZoom();
            }
        }
    }

    private void SetCameraTargets()
    {
        if (pc.gameObject.GetComponent<DashCommand>().EnableDashCommand)
        {
            if (AddCrates)
                AddCrates = false;
            targets.Clear();
            if (targets.Count == 0)
            {
                targets.Add(pc.gameObject.transform);

                print(playerDashMarker.name);
                targets.Add(playerDashMarker.transform);

                return;
            }
        }
        else
        {
            targets.Clear();
            if (!AddCrates)
                AddCrates = true;
            targets.Add(pc.gameObject.transform);
            if (GameObject.Find("Alter(Clone)") != null)
            {
                targets.Add(GameObject.Find("Alter(Clone)").transform);
            }
            if (GameObject.Find("Crate(Clone)") != null)
            {
                targets.Add(GameObject.Find("Crate(Clone)").transform);
            }
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                print(enemy.name);
                targets.Add(enemy.transform);
                /*if (enemy.GetComponent<EnemyController>().InArena)
                { 
                    targets.Add(enemy.transform);
                }*/
                
            }

            return;
        }
    }

    private float GetGreatestDistance()
    {
        if (targets != null)
        {
            var bounds = new Bounds(Vector3.zero, Vector3.zero);

            if (targets[0] != null)
                bounds = new Bounds(targets[0].position, Vector3.zero);

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null)
                    continue;
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.size.x;
        }
        return 0;
    }

    Vector3 GetCenterPoint()
    {
        if (targets != null)
        {
            if (targets.Count == 1)
            {
                if (targets[0] != null)
                {
                    return targets[0].position;
                }
            }

            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            if (targets[0] != null)
            {
                bounds = new Bounds(targets[0].position, Vector3.zero);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null)
                    continue;
                bounds.Encapsulate(targets[i].position);

            }
            return bounds.center;
        }
        return Vector3.zero;
    }


    private void SetCameraLocation()
    {
        switch(pc.gameObject.GetComponent<DashCommand>().dashState) {

            case DashCommand.DashState.completed:
                Move();
                break;
            case DashCommand.DashState.init:
                //print("move initdash");
                MoveInitDash();
                break;
            case DashCommand.DashState.inDashAttack:
                MoveDashing();
                break;
            default:
                break;
        }
    }

    private void SetCameraZoom()
    {
        switch (pc.gameObject.GetComponent<DashCommand>().dashState)
        {
            case DashCommand.DashState.completed:
                Zoom();
                break;
            case DashCommand.DashState.init:
                //print("zoom initdash");
                //ZoomInitDash();
                break;
            case DashCommand.DashState.inDashAttack:
                //ZoomDashing();
                break;
            default:
                break;
        }
    }   
     
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        
        //print("new Position: " + newPosition);
        zPos = Mathf.Clamp(newPosition.z, -5f, -20);

        newPosition = new Vector3(newPosition.x, newPosition.y, zPos);
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smooothTime);
    }

    private void MoveInitDash()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + new Vector3(0,0,-5);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smooothTime * .055f);//.015-.15
    }

    private void MoveDashing()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + new Vector3(0, 0, -10);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smooothTime * .3f);
    }


    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private void ZoomInitDash()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom * .85f, Time.deltaTime * 10f);
    }

    private void ZoomDashing()
    { 
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom * 1.5f, Time.deltaTime * 10f);
    }
}
