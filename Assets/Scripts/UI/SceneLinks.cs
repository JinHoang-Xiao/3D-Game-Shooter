using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Temporary test helper: attach to Canvas in UI Scene.
/// Automatically opens Game shortly after entering Play mode.
/// </summary>
public sealed class SceneLinks : MonoBehaviour
{
    [SerializeField] private float delaySeconds = 1f;

    private void Start()
    {
        Invoke(nameof(OpenGame), delaySeconds);
    }

    public void OpenGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("UI Scene");
    }
}