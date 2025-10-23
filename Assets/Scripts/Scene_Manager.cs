using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene_Manager: LoadScene called with empty scene name!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
        // Rキーで現在のシーンをリロード
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
