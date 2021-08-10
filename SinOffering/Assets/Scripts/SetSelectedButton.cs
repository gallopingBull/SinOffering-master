using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// 
/// dirty method/workaround to force EventSystem to
/// select a button before it's enabled.
/// 
/// </summary>

public class SetSelectedButton : MonoBehaviour
{
    [Tooltip("Small delay before assigning button " +
        "gameobject to SetSelectedGameObject().")]
    public float Delay = .25f;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Invoke("Set_Selected_Button", Delay);
    }

    private void Set_Selected_Button()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
