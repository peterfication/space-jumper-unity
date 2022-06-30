using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
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
        SceneManager.LoadScene("start");
    }
}
