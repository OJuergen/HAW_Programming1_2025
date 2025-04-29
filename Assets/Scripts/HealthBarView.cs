using System;
using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Controller component for the visual representation of an object's health.
    /// </summary>
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField, Tooltip("The filling of the health bar indicating the health value.")]
        private RectTransform _filling;

        [SerializeField, Tooltip("If checked, will automatically orient towards the main camera.")]
        private bool _faceCamera = true;

        [Header("State. Inspector changed will be overwritten at runtime.")]
        [SerializeField, Range(0, 100)] private float _percentage = 100;

        private void Awake()
        {
            Refresh();
        }

        private void OnValidate()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_filling != null)
            {
                _filling.anchorMax = new Vector2(_percentage / 100f, _filling.anchorMax.y);
            }
        }

        public void SetPercentage(float value)
        {
            if (value < 0 || value > 100) throw new ArgumentException("Percentage must be between 0 and 100");
            _percentage = value;
            Refresh();
        }

        private void LateUpdate()
        {
            if (_faceCamera && Camera.main)
            {
                transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
            }
        }
    }
}