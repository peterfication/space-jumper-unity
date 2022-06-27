using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private Player playerScript;
    private GameObject platforms;

    public string nextSceneName;
    // The won boolean needs to be public because it is used in the Player script.
    public bool won = false;

    private void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        platforms = GameObject.Find("Platforms");
    }

    private void Update()
    {
        CheckForWin();
    }

    // If there is only one platform left and the player is not dead,
    // the player has won the current level.
    private void CheckForWin()
    {
        if (!playerScript.dead && platforms.transform.childCount == 1)
        {
            won = true;
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextSceneName);
    }
}
