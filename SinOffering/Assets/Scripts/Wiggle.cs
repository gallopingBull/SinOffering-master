using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{

    public AnimationCurve curve;
    public Vector3 distance;
    public float speed;

    private Vector3 startPos, toPos;
    private float timeStart;

    private bool canWiggle = false;


    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
        //randomToPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (canWiggle)
        {
            float d = (Time.time - timeStart) / speed, m = curve.Evaluate(d);
            if (d > 1)
            {
                randomToPos();
            }
            else if (d < 0.5)
            {
                transform.position = Vector3.Lerp(startPos, toPos, m * 2.0f);
            }
            else
            {
                transform.position = Vector3.Lerp(toPos, startPos, (m - 0.5f) * 2.0f);
            }
        }
    }

    public void EnableWiggle()
    {
        if (!canWiggle)
        {
            //print("Activate Wiggle");
            startPos = transform.position;
            canWiggle = true;
            randomToPos();
        }
    }

    public void DisableWiggle()
    {
        if (canWiggle)
        {
            //print("Disable Wiggle");
            canWiggle = false;
        }
    }

    private void randomToPos()
    {
        toPos = startPos;
        toPos.x += Random.Range(-1.0f, +1.0f) * distance.x;
        toPos.y += Random.Range(-1.0f, +1.0f) * distance.y;
        //toPos.z += Random.Range(-1.0f, +1.0f) * distance.z;
        timeStart = Time.time;
    }
}
