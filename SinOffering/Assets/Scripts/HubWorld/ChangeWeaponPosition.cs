using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// moves weapon sprite objects in scene for weapon store to show
/// purchase_weapon_button is seleceted. 
/// 
/// </summary>
public class ChangeWeaponPosition : MonoBehaviour
{
    public float WeaponHeight = 1f;
    
    private Vector3 originalPos;
    private GameObject spriteObect;
    
    //public float speed = 1f;
    //private bool canMove = false; 

    void Start() {
        originalPos = transform.position; 
    }

    #region smooth interpolation test

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

    #endregion  

    public void MoveWeapon()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(originalPos.x, originalPos.y + WeaponHeight, originalPos.z),
            1);
    }
    public void ResetPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPos, 1);
    }
}
