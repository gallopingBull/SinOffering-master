// Interface for unity's scene manager class that contains several methods
// to load a different scene

using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour {
    public int LevelIndex;

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadAssignedLevel()
    {
        SceneManager.LoadScene(LevelIndex);
    }

    public void AssignLevel(int index)
    {
        LevelIndex = index;
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
