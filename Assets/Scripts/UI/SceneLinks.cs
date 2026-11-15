using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Temporary test helper: attach to Canvas in UI Scene.
/// Automatically opens Game shortly after entering Play mode.
/// </summary>
public sealed class SceneLinks : MonoBehaviour
{

    public void OpenGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("UI Scene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}