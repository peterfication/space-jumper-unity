using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCode : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        SetLevelCode();
    }

    // Set the current level code in the text field.
    private void SetLevelCode()
    {
        Level currentLevel = Levels.GetCurrentLevel();
        textMeshPro.text = $"Code: {currentLevel.code}";
    }
}
