using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBloodMask : MonoBehaviour
{
    public float bloodMaskLifeTime = 4f;
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("DeParentCaller", bloodMaskLifeTime);
    }

    protected void DeParentCaller()
    {
        if (transform.parent == null)
        {
            return;
        }
        MaskDecalPool.instance.DeParentMasks(gameObject);
    }
}