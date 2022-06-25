using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public bool isMoving = false;

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
        MovePlayer();
    }

    // Flip sprite depending on going left or right.
    private void FlipSprite()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (x > 0)
            spriteRenderer.flipX = false;
        else if (x < 0)
            spriteRenderer.flipX = true;
    }

    // Move the player up, down, left and right depending on
    // GetAxisRaw.
    private void MovePlayer()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (!isMoving)
        {
            if (x == -1)
                StartCoroutine(MovePlayerMovement(Vector3.left));
            if (x == 1)
                StartCoroutine(MovePlayerMovement(Vector3.right));
            if (y == 1)
                StartCoroutine(MovePlayerMovement(Vector3.up));
            if (y == -1)
                StartCoroutine(MovePlayerMovement(Vector3.down));
        }
    }

    // Actually move the player to the specified direction
    // in a smooth animation.
    private IEnumerator MovePlayerMovement(Vector3 direction)
    {
        isMoving = true;

        float timeToMove = 0.2f;

        Vector3 origPos = transform.position;
        Vector3 targetPos = origPos + direction;

        float elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}
