using System;
using System.Collections.Generic;
using System.Linq;
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
        private Camera _camera;

        // blockable
        private readonly HashSet<object> _blockers = new();
        public bool IsBlocked => _blockers.Any();
        public void AddBlocker(object blocker) => _blockers.Add(blocker);
        public void RemoveBlocker(object blocker) => _blockers.Remove(blocker);

        private enum Mode
        {
            Axes,
            Keys,
            Actions
        }

        private void Start()
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                Debug.LogWarning("No camera found. Disabling player controller.", this);
                enabled = false;
            }
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
            Vector2 input = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) input += Vector2.up;
            if (Input.GetKey(KeyCode.A)) input += Vector2.left;
            if (Input.GetKey(KeyCode.S)) input += Vector2.down;
            if (Input.GetKey(KeyCode.D)) input += Vector2.right;

            HandleMoveInput(input);
            HandleLookInput(input);
        }

        private void HandleInputAxes()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            HandleMoveInput(input);
            HandleLookInput(input);
        }

        private void HandleInputActions()
        {
            var moveInput = _moveAction.action.ReadValue<Vector2>();
            HandleMoveInput(moveInput);

            var lookInput = _lookAction.action.ReadValue<Vector2>();
            if (lookInput == Vector2.zero) lookInput = moveInput;
            HandleLookInput(lookInput);
        }

        private void HandleMoveInput(Vector2 inputDirection)
        {
            if (IsBlocked) return;
            Vector3 inputDirection3D = new Vector3(inputDirection.x, 0, inputDirection.y);
            Vector3 direction = Quaternion.AngleAxis(_camera.transform.eulerAngles.y, Vector3.up) * inputDirection3D;
            transform.Translate(direction * (_speed * Time.deltaTime), Space.World);
        }

        private void HandleLookInput(Vector2 inputDirection)
        {
            if (IsBlocked) return;
            Vector3 inputDirection3D = new Vector3(inputDirection.x, 0, inputDirection.y);
            Vector3 direction = Quaternion.AngleAxis(_camera.transform.eulerAngles.y, Vector3.up) * inputDirection3D;
            if (direction != Vector3.zero)
            {
                transform.LookAt(transform.position + direction);
            }
        }
    }
}