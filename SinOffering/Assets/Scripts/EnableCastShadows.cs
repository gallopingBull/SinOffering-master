using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCastShadows : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {

        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = true;

    }

}
