using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeaponPosition : MonoBehaviour
{
    public float WeaponHeight = 1f;
    public float speed = 1f;
    private bool canMove = false; 
    private Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position; 
    }

    // Update is called once per frame
    /*void Update()
    {
        if (canMove)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
                                              //move up
            if (transform.position.y < originalPos.y + WeaponHeight)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(originalPos.x, originalPos.y + WeaponHeight, originalPos.z),
                    step);
    

            }
            if (transform.position.y == originalPos.y + WeaponHeight && !MoveUp)
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPos, step);
            }
        } 
    }*/

    public void MoveWeapon()
    {
        //canMove = true;

        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(originalPos.x, originalPos.y + WeaponHeight, originalPos.z),
            1);

    }
    public void ResetPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPos, 1);
        //canMove = true;
        
    }
}
