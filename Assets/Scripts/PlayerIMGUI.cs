using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Utility for manual testing the player object and related functionalities using IMGUI buttons.
    /// </summary>
    public class PlayerIMGUI : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private float _width = 200f;
        [SerializeField] private float _padding = 20f;
        [SerializeField] private Color _backgroundColor = new(0f, 0f, 0f, 0.7f);

        // cache
        private Player _player;

        // state
        private int _amount = 5;
        private bool _showGUI;

        private void OnGUI()
        {
            GUI.backgroundColor = _backgroundColor;

            float width = Mathf.Min(_width, Screen.width - 2 * _padding);
            GUILayout.BeginArea(new Rect(_padding, _padding, width, Screen.height - 2 * _padding));
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUI.backgroundColor = Color.white;
                _showGUI = GUILayout.Toggle(_showGUI, "Show Debug GUI");
                if (_showGUI)
                {
                    _player ??= FindAnyObjectByType<Player>();
                    GUILayout.Space(10);
                    GUILayout.Label("Damage/Heal");
                    _amount = Mathf.RoundToInt(GUILayout.HorizontalSlider(_amount, 0, 100));

                    GUILayout.Space(10);

                    if (_player.Health != null && GUILayout.Button($"Take {_amount} Damage"))
                    {
                        _player.Health.TakeDamage(_amount);
                    }

                    if (_player.Health != null && GUILayout.Button($"Heal {_amount}"))
                    {
                        _player.Health.Heal(_amount);
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}