using UnityEngine;

namespace SixtyMeters.logic.player
{
    // Used to detect when the player enters or leaves rooms
    // should be placed on a body part with a collider
    public class PlayerBodyCollider : MonoBehaviour
    {
        public PlayerBodyColliderType bodyColliderType;
    }

    public enum PlayerBodyColliderType
    {
        Ignore, // Colliders that should be ignored by interactable objects
        LookAt, // The collider for the camera, extends far into the distance to trigger doors
        Exact // The exact location of the players body
    }
}