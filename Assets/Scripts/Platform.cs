using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Indicate whether the platform has been visited in the sense
    // of staying on the platform, not just passing over it.
    private bool visited = false;

    // Set visited to true if the player is colliding and not moving.
    // The not moving part is required for when the player is jumping over
    // a platform later on.
    void OnTriggerStay2D(Collider2D collider)
    {
        bool playerIsMoving = collider.gameObject.GetComponent<Player>().isMoving;
        if (collider.gameObject.name == "Player" && !playerIsMoving)
            visited = true;
    }

    // Destroy the platform if the player is leaving and the platform
    // has been visited before. If the player is jumping over a platform,
    // the OnTriggerExit2D will happen as well, even though it hasn't been
    // visited.
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player" && visited)
            Destroy(gameObject);
    }
}
