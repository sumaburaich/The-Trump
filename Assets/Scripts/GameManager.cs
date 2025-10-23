using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        // Escキーでアプリケーション終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
