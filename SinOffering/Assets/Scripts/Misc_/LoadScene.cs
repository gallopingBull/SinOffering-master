// Interface for unity's scene manager class that contains several methods
// to load a different scene

using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour {
    public int level;

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadAssignedLevel()
    {
        SceneManager.LoadScene(level);
    }

    public void AssignLevel(int index)
    {
        level = index;
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
