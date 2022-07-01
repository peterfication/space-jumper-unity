using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCode : MonoBehaviour
{
    private GameLogic gameLogic;
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        textMeshPro = GetComponent<TMPro.TextMeshProUGUI>();
        SetLevelCode();
    }

    // Set the current level code in the text field.
    private void SetLevelCode()
    {
        Level currentLevel = gameLogic.GetCurrentLevel();
        textMeshPro.text = $"Code: {currentLevel.code}";
    }
}
