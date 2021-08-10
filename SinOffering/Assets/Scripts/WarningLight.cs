using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLight : MonoBehaviour
{
    public GameObject _light; 
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Enemy")
        {
            if (!_light.activeInHierarchy)
            {
                _light.SetActive(true);
                StartCoroutine(TurnOffLight());
            }
        }
    }

    private IEnumerator TurnOffLight()
    {
        yield return new WaitForSeconds(3);
        _light.SetActive(false);
        StopCoroutine(TurnOffLight());
    }
}
