using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platforms : MonoBehaviour
{
    public GameObject platformPrefab;

    private void Start()
    {
        LoadLevel();
    }

    // If there is a current level, load it.
    private void LoadLevel()
    {
        Level currentLevel = Levels.GetCurrentLevel();
        if (currentLevel is not null)
        {
            BuildLevel(currentLevel);
        }
        else
        {
            SceneManager.LoadScene("end_won");
        }
    }

    private void BuildLevel(Level level)
    {
        for (int y = 0; y < level.platforms.Length; y++)
        {
            int[] row = level.platforms[y];
            for (int x = 0; x < row.Length; x++)
            {
                int cell = row[x];

                if (cell > 0)
                {
                    // Create platform
                    var platform = Instantiate(platformPrefab, transform);
                    // Left upper platform position: -8, 4, 0
                    // Maximum platforms size: 17x9
                    platform.transform.position = new Vector3(x - 8, -1 * (y - 4));
                    platform.name = $"Platform {y} {x}";

                    if (cell == 2)
                    {
                        // Tag the platform with "StartPlatform"
                        platform.tag = "StartPlatform";
                    }
                }
            }
        }
    }
}
