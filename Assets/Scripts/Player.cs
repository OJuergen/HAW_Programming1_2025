using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// The playable character.
    /// This serves as an identifier to find the player object and retrieve references to its components.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public IHealth Health { get; private set; }
        [field: SerializeField] public PlayerController PlayerController { get; private set; }
        [field: SerializeField] public Transform CameraFocusPoint { get; private set; }

        private void Awake()
        {
            Health = GetComponent<IHealth>();

            if (Health == null || PlayerController == null)
            {
                Debug.LogError("Player object must have a Health and a PlayerController component. " +
                               "Disabling this.", this);
                gameObject.SetActive(false);
            }
        }
    }
}