using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField]
    private int sceneID = 0;
    private LoadScene loadScene;

    private void Start()
    {
        loadScene = GetComponent<LoadScene>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
            loadScene.LoadSceneByIndex(sceneID);
    }
}
