using UnityEngine;

/// <summary>
/// PlayerController is responsible for managing the player's actions,
/// movement, and interaction within the game environment.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private void Update()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
        transform.position += direction * (_speed * Time.deltaTime);
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}