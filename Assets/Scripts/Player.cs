using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private GameLogic gameLogic;

    // isMoving needs to be public because it is used in Platform.OnTriggerStay2D.
    public bool isMoving = false;
    // The waitForNextMove is necessary to have a private variable next to isMoving
    // so we can make sure to have one move after another, even when holding down
    // an arrow key.
    private bool waitForNextMove = false;
    private bool jump = false;
    // onPlatform needs to be public because it is used in GameLogic.CheckForWin
    public bool onPlatform = true;
    // dead needs to be public because it is used in Platforms.Update
    public bool dead = false;
    private bool winningJumpDone = false;
    public Sprite spriteStay;
    public Sprite spriteWalk;
    public Sprite spriteJump;
    public RuntimeAnimatorController animatorWalk;
    public RuntimeAnimatorController animatorJump;
    public ContactFilter2D filter;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();

        StartCoroutine(MoveToStartPlatform());
    }

    private void FixedUpdate()
    {
        // Make sure the player can only move if it is not dead.
        if (!dead && !gameLogic.won)
        {
            FlipSprite();
            CheckForJump();
            MovePlayer();
        }
    }

    private void Update()
    {
        HandleCollision();
        CheckForDead();
        HandleDeath();
        HandleWin();
    }

    // Find the platform tagged with "StartPlatform" and move the player there.
    // This makes it easier to change the start platform without manually aligning
    // the player in the scene editor, because the player is not on the grid, but a little
    // bit below.
    private IEnumerator MoveToStartPlatform()
    {
    // Wait for the platforms to be build
    yield return new WaitForSeconds(0.1f);

        GameObject[] startingPlatforms = GameObject.FindGameObjectsWithTag("StartPlatform");
        if (startingPlatforms.Length > 0)
            transform.position = startingPlatforms[0].transform.position + new Vector3(0, -0.25f, 0);
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

    // Set jump to true if the "Fire2" button is being hold down
    // and hence change the sprite for a visual feedback.
    // But only if the player is not moving.
    private void CheckForJump()
    {
        if (isMoving)
            return;

        jump = Input.GetButton("Fire2");

        if (jump)
        {
            spriteRenderer.sprite = spriteJump;
            animator.runtimeAnimatorController = animatorJump;
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
            spriteRenderer.sprite = spriteStay;
        }
    }

    // Move the player up, down, left and right depending on
    // GetAxisRaw.
    // But only if the player is not moving already.
    private void MovePlayer()
    {
        if (waitForNextMove)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // When jumping, the player moves two platforms at a time.
        int movementLength = 1;
        if (jump)
            movementLength = 2;

        if (x == -1)
            StartCoroutine(MovePlayerMovement(movementLength * Vector3.left));
        if (x == 1)
            StartCoroutine(MovePlayerMovement(movementLength * Vector3.right));
        if (y == 1)
            StartCoroutine(MovePlayerMovement(movementLength * Vector3.up));
        if (y == -1)
            StartCoroutine(MovePlayerMovement(movementLength * Vector3.down));
    }

    // Actually move the player to the specified direction
    // in a smooth animation.
    private IEnumerator MovePlayerMovement(Vector3 direction)
    {
        isMoving = true;
        waitForNextMove  = true;
        // Only change the sprite and animator, if it's not a jump,
        // because the jump has an animated sprite already.
        if (!jump)
        {
            spriteRenderer.sprite = spriteWalk;
            animator.runtimeAnimatorController = animatorWalk;
            animator.enabled = true;
        }

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

        // Wait a little bit, so the platform can be marked as visited.
        // If this does not happen, a player can walk over platforms when walking
        // fast, because isMoving is always true.
        // An alternative would be to mark the platform as visited here and not
        // in Platform.OnTriggerStay2D.
        yield return new WaitForSeconds(0.1f);
        waitForNextMove  = false;
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
            if (hits[i] == null)
                continue;

            // Check for a regular platform, so a platform that
            // is a child of the "Platforms" GameObject.
            if (hits[i].transform.parent != null && hits[i].transform.parent.name == "Platforms")
                tempOnPlatform = true;

            // The OutOfScreen Platform where the player resides before
            // it is moved to the StartPlatform.
            if (hits[i].name == "OutOfScreen Platform")
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

    // Handle all the things that should happen when a player dies.
    private void HandleDeath()
    {
        if (!dead)
            return;

        // Player should now be behind platforms
        spriteRenderer.sortingLayerName = "Default";
        // No further collisions should happen anymore
        boxCollider.enabled = false;

        StartCoroutine(AnimatePlayerFallingDown());
        StartCoroutine(ReloadScene());
    }

    private IEnumerator AnimatePlayerFallingDown()
    {
        float timeToMove = 2f;
        Vector3 direction = new Vector3(0, -20, 0);
        Vector3 origPos = transform.position;
        Vector3 targetPos = origPos + direction;

        float elapsedTime = 0;
        while (elapsedTime < timeToMove)
        {
            transform.Rotate(0, 0, 0.4f);
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Wait for 3 seconds so the player can fall and then reload the current level
    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HandleWin()
    {
        if (!gameLogic.won)
            return;

        if (!winningJumpDone)
            StartCoroutine(JumpUpAndDown());
    }

    // Move the player sprite up and down to simulate jumping happily
    private IEnumerator JumpUpAndDown()
    {
        winningJumpDone = true;

        // We need to wait, because the win happens, as soon the
        // player leaves the last platform.
        yield return new WaitForSeconds(0.1f);

        float timeToMove = 0.1f;
        Vector3 direction = new Vector3(0, 0.3f, 0);
        Vector3 origPos = transform.position;
        Vector3 targetPos = origPos + direction;

        transform.position = origPos;

        float elapsedTime = 0;
        // Jump multiple times
        for (int i = 0; i < 2; ++i)
        {
            // Jump up
            elapsedTime = 0;
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPos;

            // // Jump down
            elapsedTime = 0;
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(targetPos, origPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = origPos;
        }
    }
}
