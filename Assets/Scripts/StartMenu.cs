using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private GameObject returnButton;

    private void Start()
    {
        returnButton = GameObject.Find("ReturnToGameButton");
    }

    private void Update()
    {
        HandleEnter();
        HandleEsc();
        DisplayReturnButton();
    }

    // Only display the ReturnToGameButton when the current level
    // is not the first level.
    private void DisplayReturnButton()
    {
        if (GameData.currentLevel == 0)
        {
            returnButton.SetActive(false);
        }
        else
        {
            returnButton.SetActive(true);
        }
    }

    private void HandleEnter()
    {
        if (Input.GetKey(KeyCode.Return))
            LoadCurrentLevel();
    }

    private void HandleEsc()
    {
        if (Input.GetKey(KeyCode.Escape))
            ExitGame();
    }

    public void LoadFirstLevel()
    {
        GameData.currentLevel = 0;
        SceneManager.LoadScene("level_dynamic");
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene("level_dynamic");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
