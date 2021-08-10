using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float SelfDestructTime;
    //public Material mat; 

    // Start is called before the first frame update

    void Update()
    {
        //mat = GetComponent<Renderer>().material;
        if (Input.GetKeyDown(KeyCode.K))
        {
            Self_Destruct();
        }
    }

    void Start()
    {
        //mat = GetComponent<Renderer>().material;
        Self_Destruct();        
    }

    private void Self_Destruct()
    {
        //mat.DisableKeyword("FADE_ON");
        //mat.EnableKeyword("FADE_ON");
        Destroy(gameObject, SelfDestructTime);
    }
}
