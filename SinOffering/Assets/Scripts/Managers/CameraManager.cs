using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region variables
    [HideInInspector]
    public static CameraManager instance;

    private PlayerController pc;

    private CinematicBars cinematicBars;

    [SerializeField]
    private CinemachineVirtualCamera currentCam; 
    public CinemachineVirtualCamera MainCam; 
    public CinemachineVirtualCamera DashTargetingCam;
    public CinemachineVirtualCamera DashCam;
    public CinemachineBrain cinemachineBrain;

    [HideInInspector]
    public CinemachineBasicMultiChannelPerlin cmBasicMultiChannelPerlin; //used for camera shake

    private CinemachineTargetGroup targetGroup;
    #endregion


    #region functions
    private void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currentCam = MainCam;
        cmBasicMultiChannelPerlin = currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        pc = PlayerController.instance;
        cinematicBars = GameObject.Find("CinematicBars").GetComponent<CinematicBars>();
        targetGroup = GameObject.Find("CM TargetGroup_Arena").GetComponent<CinemachineTargetGroup>();
        cmBasicMultiChannelPerlin = currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pc != null)
            CameraHandler();
    }

    private void CameraHandler()
    {
        switch (pc.gameObject.GetComponent<DashCommand>().dashState)
        {
            case DashCommand.DashState.completed:
                if (MainCam.Priority == 10)
                {
                    if (currentCam != null)
                        currentCam.Priority = 10;
                    cinemachineBrain.m_DefaultBlend.m_Time = .5f;

                    if (cinematicBars.isActive)
                    {
                        cinematicBars.Hide(.1f);
                    }
                    
                    MainCam.Priority = 11;
                    currentCam = MainCam;
                    cmBasicMultiChannelPerlin = 
                        currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                }
                break;

            case DashCommand.DashState.init:
                if (DashTargetingCam.Priority == 10)
                {
                    if (currentCam != null)
                        currentCam.Priority = 10;
                    cinemachineBrain.m_DefaultBlend.m_Time = .1f;

                    DashTargetingCam.Priority = 11;
                    currentCam = DashTargetingCam;
                    cmBasicMultiChannelPerlin = 
                        currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                }
                break;

            case DashCommand.DashState.inDashAttack:

                if (pc.gameObject.GetComponent<DashCommand>().dashAttack &&
                    pc.gameObject.GetComponent<DashCommand>().targets.Count > 0)
                {
                    cinematicBars.Show(.1f);
                    cmBasicMultiChannelPerlin = 
                        currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                }

                if (DashCam.Priority == 10)
                {
                    cinematicBars.Show(.1f);

                }
                break;

            default:
                break;
        }
    }

    public void AddCameraTargets(Transform target, float weight)
    {
        targetGroup.AddMember(target, weight, 1);
    }

    public void RemoveCameraTargets(Transform target)
    {
        targetGroup.RemoveMember(target);
    }

    public CinemachineVirtualCamera GetCurrentCam()
    {
        if (currentCam == null)
        {
            return null;
        }
        return currentCam;
    }

    public void SetCamera(CinemachineVirtualCamera cam)
    {
        currentCam = cam;
    }
    
    #endregion
}
