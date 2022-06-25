using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        MoveToStartPlatform();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void MoveToStartPlatform()
    {
        GameObject[] startingPlatforms = GameObject.FindGameObjectsWithTag("StartPlatform");
        transform.position = startingPlatforms[0].transform.position + new Vector3(0, (float)-0.25, 0);
    }

    private void FixedUpdate()
    {
        FlipSprite();
    }

    // Flip sprite depending on going left or right
    private void FlipSprite()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (x > 0)
            spriteRenderer.flipX = false;
        else if (x < 0)
            spriteRenderer.flipX = true;
    }
}
