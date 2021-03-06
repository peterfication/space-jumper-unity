using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private Player playerScript;
    private GameObject platforms;

    // The won boolean needs to be public because it is used in the Player script.
    public bool won = false;
    // This boolean is necessary to only load the next level once
    private bool loadNextLevel = false;

    private void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        platforms = GameObject.Find("Platforms");
    }

    private void Update()
    {
        MaybeLoadGameWonScene();
        StartCoroutine(CheckForWin());
    }

    // If there is no level anymore, load end_won scene
    private void MaybeLoadGameWonScene()
    {
        Level currentLevel = Levels.GetCurrentLevel();
        if (currentLevel is null)
        {
            SceneManager.LoadScene("end_won");
        }
    }

    // If there is only one platform left and the player is not dead,
    // the player has won the current level.
    private IEnumerator CheckForWin()
    {
        // We need to wait a bit, in order to not get a race condition
        // of the collision logic in Player.
        yield return new WaitForSeconds(0.3f);

        if (!playerScript.dead && playerScript.onPlatform && platforms.transform.childCount == 1)
        {
            won = true;
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        if (loadNextLevel)
            yield return null;

        loadNextLevel = true;

        yield return new WaitForSeconds(1);

        GameData.currentLevel = GameData.currentLevel + 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
