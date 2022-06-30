using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    void Update()
    {
        HandleEnter();
        HandleEsc();
    }

    private void HandleEnter()
    {
        if (Input.GetKey(KeyCode.Return))
            LoadFirstLevel();
    }

    private void HandleEsc()
    {
        if (Input.GetKey(KeyCode.Escape))
            ExitGame();
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("level_dynamic");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
