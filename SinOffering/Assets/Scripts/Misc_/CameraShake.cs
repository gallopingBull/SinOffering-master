
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour {

    #region variables
    [HideInInspector]
    public static CameraShake instance;

    private CameraManager cm;
    private CinemachineVirtualCamera currentCam;

    private float ShakeTime, ShakeTimeTotal, ShakeIntensity;
    private bool SmoothTransition = false;

    #endregion

    #region functions
    void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        cm = CameraManager.instance;
        currentCam = cm.GetCurrentCam();
    }

    private void Update()
    {
        if (ShakeTime > 0)
        {
            ShakeTime -= Time.deltaTime;
            if (!SmoothTransition)
            {
                if (ShakeTime <= 0)
                {
                    cm.cmBasicMultiChannelPerlin.m_AmplitudeGain = 0;
                }
            }
            else
            {
                cm.cmBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(ShakeIntensity, 0f, 1-(ShakeTime/ShakeTimeTotal));
            }
          
        }
    }

    public void Shake(float time, float intensity, bool smoothTransition)
    {
        ShakeTime = time;
        if (smoothTransition)
            ShakeTimeTotal = time;
        ShakeIntensity = intensity;
        SmoothTransition = smoothTransition;
        currentCam = cm.GetCurrentCam();
            
        cm.cmBasicMultiChannelPerlin.m_AmplitudeGain = ShakeIntensity;
    }
    #endregion
}
