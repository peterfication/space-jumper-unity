using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    // isMoving needs to be public because it is used in Platform.OnTriggerStay2D.
    public bool isMoving = false;
    private bool onPlatform = true;
    private bool dead = false;
    public Sprite spriteStay;
    public Sprite spriteWalk;
    public ContactFilter2D filter;

    private void Start()
    {
        MoveToStartPlatform();

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        // Make sure the player can only move if it is not dead.
        if (!dead)
        {
            FlipSprite();
            MovePlayer();
        }
    }

    private void Update()
    {
        HandleCollision();
        CheckForDead();
        HandleDeath();
    }

    // Find the platform tagged with "StartPlatform" and move the player there.
    // This makes it easier to change the start platform without manually aligning
    // the player in the scene editor, because the player is not on the grid, but a little
    // bit below.
    private void MoveToStartPlatform()
    {
        GameObject[] startingPlatforms = GameObject.FindGameObjectsWithTag("StartPlatform");
        transform.position = startingPlatforms[0].transform.position + new Vector3(0, (float)-0.25, 0);
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
        spriteRenderer.sprite = spriteWalk;
        animator.enabled = true;

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
        // Make sure we _exactly_ hit the targetPos
        transform.position = targetPos;

        animator.enabled = false;
        spriteRenderer.sprite = spriteStay;
        isMoving = false;
    }

    // Go through all collisions and check whether there is at least one
    // collision with a platform by checking that the parents collision object
    // is the "Platforms" GameObject.
    private void HandleCollision()
    {
        Collider2D[] hits = new Collider2D[10];
        boxCollider.OverlapCollider(filter, hits);

        // We need a temporay variable here, because the player might
        // collide with other things, and then, the onPlatform would be
        // false for a short amount of time, even though the player does
        // collider with a platform.
        bool tempOnPlatform = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] != null && hits[i].transform.parent.name == "Platforms")
                tempOnPlatform = true;
        }
        onPlatform = tempOnPlatform;
    }

    // Check whether dead has to be set to true. A player is dead if it is
    // either not moving or not on a platform. Both checks are necessary
    // because during a jump, the player is not on platform but is moving,
    // so it is not dead even though it is not on a platform.
    private void CheckForDead()
    {
        if (!isMoving && !onPlatform)
        {
            dead = true;
        }
    }

    // TODO do something when the player dies.
    private void HandleDeath()
    {
        if (dead)
        {
            Debug.Log("Die!");
        }
    }
}
