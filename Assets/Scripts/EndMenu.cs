using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    void Update()
    {
        HandleEsc();
    }

    private void HandleEsc()
    {
        if (Input.GetKey(KeyCode.Escape))
            BackToMenu();
    }

    public void BackToMenu()
    {
        GameData.currentLevel = 0;
        SceneManager.LoadScene("start");
    }
}
