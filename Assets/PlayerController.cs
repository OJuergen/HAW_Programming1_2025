using System;
using UnityEngine;

/// <summary>
/// PlayerController is responsible for managing the player's actions,
/// movement, and interaction within the game environment.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("The maximum speed with which the player moves.")]
    private float _speed = 1f;
    [SerializeField, Tooltip("Input handling mode. Keys process WASD. " +
                             "Axes uses input configured in ProjectSettings.InputManager")]
    private Mode _mode = Mode.Axes;

    private enum Mode
    {
        Axes,
        Keys
    }

    private void Update()
    {
        switch (_mode)
        {
            case Mode.Axes:
                HandleInputAxes();
                break;
            case Mode.Keys:
                HandleInputKeys();
                break;
            default:
                enabled = false;
                throw new ArgumentOutOfRangeException($"Unknown input mode: {_mode}");
        }
    }

    private void HandleInputKeys()
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
        
        MovePlayer(direction);
    }

    private void HandleInputAxes()
    {
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        MovePlayer(direction);
    }

    private void MovePlayer(Vector3 direction)
    {
        transform.Translate(direction * (_speed * Time.deltaTime), Space.World);
        transform.LookAt(transform.position + direction);
    }
}