using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PostProcessManager : MonoBehaviour
{

    #region variables 
    [HideInInspector]
    public static PostProcessManager intance;
    private bool PostProcessEnabled = false;
    [HideInInspector]
    public bool ppEnabled = false; //public variable to check if any PP FX is active

    private float m_intensity;
    private float m_time, m_timeTotal;
    private bool SmoothTransition = false;
    private bool onDash = false;

    //orignal/base values for postprocess fx
    private float chromaticAberrationIntensity;
    private float vignetteIntensity;
    private float grainIntensity;

    private PostProcessVolume m_volume;
    private ChromaticAberration m_chromaticAberration;
    private Vignette m_vignette;
    private Grain m_grain;

    #endregion

    #region functions
    private void Awake()
    {
        intance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitPPManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (PostProcessEnabled)
            PostProcessHandler();
        //m_vignette.intensity.value = Mathf.Sin(Time.realtimeSinceStartup);
    }

    private void InitPPManager()
    {
        m_volume = GetComponent<PostProcessVolume>();
        m_volume.profile.TryGetSettings<ChromaticAberration>(out m_chromaticAberration);
        m_volume.profile.TryGetSettings<Vignette>(out m_vignette);
        m_volume.profile.TryGetSettings<Grain>(out m_grain);

        chromaticAberrationIntensity = m_chromaticAberration.intensity.value;
        vignetteIntensity = m_vignette.intensity.value;
        grainIntensity = m_grain.intensity.value;
    }

    private void PostProcessHandler()
    {
        if (m_time > 0)
        {
            m_time -= Time.deltaTime;
            if (!SmoothTransition)
            {
                if (m_time <= 0)
                {
                    PostProcessEnabled = false;
                    m_chromaticAberration.intensity.value = chromaticAberrationIntensity;
                    m_grain.intensity.value = grainIntensity;
                  
                }
            }
            
            else
            {
                if (m_time <= 0)
                {
                    PostProcessEnabled = false;
                    onDash = false;
                    ppEnabled = false; 
                    m_vignette.intensity.value = vignetteIntensity;

                    m_chromaticAberration.intensity.value = chromaticAberrationIntensity;
                    m_grain.intensity.value = grainIntensity;
                }
                   
                if (onDash)
                {
                    m_vignette.intensity.value = Mathf.Lerp(.8f, vignetteIntensity, 1 - (m_time / m_timeTotal));
                    m_chromaticAberration.intensity.value = Mathf.Lerp(5f, chromaticAberrationIntensity, 1 - (m_time / m_timeTotal));
                    m_grain.intensity.value = Mathf.Lerp(.9f, grainIntensity, 1 - (m_time / m_timeTotal));
                }
                //onfire
                else
                {
                    m_grain.intensity.value = Mathf.Lerp(m_intensity, grainIntensity, 1 - (m_time / m_timeTotal));
                    m_chromaticAberration.intensity.value = Mathf.Lerp(m_intensity, chromaticAberrationIntensity, 1 - (m_time / m_timeTotal));
                }
            }
        }
    }

    public void OnFire(float time, float intensity, bool smoothTransition)
    {
        m_time = time;
        m_intensity = intensity;
        SmoothTransition = smoothTransition;
        if (SmoothTransition)
            m_timeTotal = time;

        m_chromaticAberration.intensity.value = m_intensity;
        m_grain.intensity.value = m_intensity;

        PostProcessEnabled = true;
    }


    public void InitDash()
    {
        //print("InitDash() being called");
        m_chromaticAberration.intensity.value = 2;
        m_grain.intensity.value = .7f;
        m_vignette.intensity.value = .6f;
    }

    public void ExitInitDash(float time, bool smoothTransition)
    {
        //print("ExitInitDash() being called");
        if (!PostProcessEnabled)
        {
            m_time = time;
            SmoothTransition = smoothTransition;
            if (SmoothTransition)
                m_timeTotal = time;

            m_chromaticAberration.intensity.value = 2;
            m_grain.intensity.value = .7f;
            m_vignette.intensity.value = .6f;

            PostProcessEnabled = true;
            onDash = true;
        }
       
    }

    public void OnDash(float time, float intensity, bool smoothTransition)
    {
        m_chromaticAberration.intensity.value = m_intensity;
        m_grain.intensity.value = .9f;
        m_vignette.intensity.value = .8f;

        ppEnabled = true; 
        /*
        print("OnDash() being called");
        if (!PostProcessEnabled)
        {
            m_time = time;
            m_intensity = intensity;
            SmoothTransition = smoothTransition;
            if (SmoothTransition)
                m_timeTotal = time;

            m_chromaticAberration.intensity.value = m_intensity;
            m_grain.intensity.value = .9f;
            m_vignette.intensity.value = .8f;

            PostProcessEnabled = true;
            onDash = true;
        }*/

    }
    #endregion

}
