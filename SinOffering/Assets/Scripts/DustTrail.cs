using UnityEngine;

public class DustTrail : MonoBehaviour
{
    [HideInInspector]
    public GameObject actor;

    private Vector3 newPos;

    public float maxScale = .25f;

    private Vector3 scaleChange;
    public float minScaleRate = -.009f;
    public float maxScaleRate = .01f;
    public float alphaScaleRate = .005f;

    //add minimum acale value before killing player
    public float minDeathScale = .045f;

    private Material mat;
    private bool fadeOut = false;

    public float DestoryTime = 5f;

    public bool isExplosion; 
    // Start is called before the first frame update
    private void Awake()
    {
        mat = GetComponent<Renderer>().material;  
    }
    void Start()
    {
        if (!isExplosion)
        {
            //this is for foot dust trails
            transform.localScale = new Vector3(.05f, -.05f, .05f);
        }
        else
        {
            //this is just for explosions
            transform.localScale = new Vector3(.05f, .05f, .05f);
            Invoke("DisableTrail", DestoryTime);
        }

        //transform.position = newPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.localScale.x >= 0)
            transform.localScale += scaleChange;

        if (fadeOut && mat.color.a > 0) 
        {
            float tmpColor = mat.color.a - alphaScaleRate;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, tmpColor);
        }

        if (transform.localScale.x >= maxScale)
        {
            if (scaleChange.x != minScaleRate && !fadeOut )
            {
                scaleChange = new Vector3(minScaleRate, minScaleRate, minScaleRate);
                fadeOut = true;
            }
        }


        if (transform.localScale.x < minDeathScale && !isExplosion)
            Invoke("DisableTrail", DestoryTime);
    }

    //for now deleting dusttrail instance
    //afterwards change this for object pooler
    private void DisableTrail()
    {   
       // print("disabled trail called");
        Destroy(gameObject);
    }

    public void EnableTrail()
    {
        if (!isExplosion)
        {
            newPos = new Vector3(actor.transform.position.x,
                actor.transform.position.y - actor.GetComponent<Entity>().AmmountToGround,
                actor.transform.position.z);

            transform.position = newPos;
        }

        scaleChange = new Vector3(maxScaleRate, maxScaleRate, maxScaleRate);
    }
}
