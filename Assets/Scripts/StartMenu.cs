using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    public void LoadLevelByCode()
    {
        TextMeshProUGUI codeInputText = GameObject.Find("CodeInputText").GetComponent<TextMeshProUGUI>();
        Level level = Levels.GetLevelByCode(codeInputText.text);

        if (level is not null)
        {

            GameData.currentLevel = level.number;
            SceneManager.LoadScene("level_dynamic");
        }
        else
        {
            // TODO This does not work
            Image inputImage = GameObject.Find("CodeInput").GetComponent<Image>();
            inputImage.color = new Color(255, 115, 115, 255);
        }
    }
}
