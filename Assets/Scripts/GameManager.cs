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
        // Esc�L�[�ŃA�v���P�[�V�����I��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
