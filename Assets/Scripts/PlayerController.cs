using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameProgramming1
{
    /// <summary>
    /// PlayerController is responsible for managing the player's actions,
    /// movement, and interaction within the game environment.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Tooltip("The maximum speed with which the player moves.")]
        private float _speed = 1f;
        [SerializeField, Tooltip("Movement input when using actions mode.")]
        private InputActionReference _moveAction;
        [SerializeField, Tooltip("Look input when using actions mode.")]
        private InputActionReference _lookAction;
        [SerializeField, Tooltip("Input handling mode. Keys process WASD. " +
                                 "Axes uses input configured in ProjectSettings.InputManager")]
        private Mode _mode = Mode.Axes;

        private enum Mode
        {
            Axes,
            Keys,
            Actions
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
                case Mode.Actions:
                    HandleInputActions();
                    break;
                default:
                    enabled = false;
                    throw new ArgumentOutOfRangeException($"Unknown input mode: {_mode}");
            }

            // stay upright
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
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

        private void HandleInputActions()
        {
            var moveInput = _moveAction.action.ReadValue<Vector2>();
            var direction = new Vector3(moveInput.x, 0, moveInput.y);
            MovePlayer(direction);
            var lookInput = _lookAction.action.ReadValue<Vector2>();
            var lookDirection = new Vector3(lookInput.x, 0, lookInput.y);
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }

        private void MovePlayer(Vector3 direction)
        {
            transform.Translate(direction * (_speed * Time.deltaTime), Space.World);
            transform.LookAt(transform.position + direction);
        }
    }
}