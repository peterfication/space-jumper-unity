using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public GameObject platformPrefab;

    void Start()
    {
        BuildLevel(level_1);
    }

    private void BuildLevel(int[][] level)
    {
        for (int y = 0; y < level.Length; y++)
        {
            int[] row = level[y];
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

    private int[][] level_1 = new int[][] {
      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      new int[] { 0, 0, 0, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };
}
