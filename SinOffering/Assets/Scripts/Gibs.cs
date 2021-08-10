using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gibs : MonoBehaviour
{
    public GameObject[] GibSprites;
    // public ParticleSystem particle;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)){
            print("pressed k");
            BlowUpGibs();
        }
    }

    public void Start()
    {
        BlowUpGibs();
    }
    public void BlowUpGibs() {
        GetComponentInChildren<ParticleSystem>().Play();
        for (int i = 0; i < GibSprites.Length; i++)
        {
            GameObject tmp = Instantiate(GibSprites[i], transform);
            tmp.transform.localScale *= 7;
            tmp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5, 5), Random.Range(1, 3)), ForceMode2D.Impulse);
        }
    }
}
